﻿using System;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.Collections;
using System.ComponentModel;
using System.Runtime.InteropServices;

public class UniPowerManager : MonoBehaviour
{
    #region Library Import
    [DllImport("kernel32", SetLastError = true, CharSet = CharSet.Ansi)]
    static extern IntPtr LoadLibrary([MarshalAs(UnmanagedType.LPStr)]string lpFileName);
    [DllImport("kernel32.dll", SetLastError = true)]
    [return: MarshalAs(UnmanagedType.Bool)]
    static extern bool FreeLibrary(IntPtr hModule);
    [DllImport("EnergyLib32")]
    public static extern bool IntelEnergyLibInitialize();
    [DllImport("EnergyLib32")]
    public static extern bool GetNumMsrs(out int nMsr);
    #endregion

    #region Variables

    static IntPtr module;
    public static int pMSRCount = 0;

    [SerializeField]
    private Button Btn_1 = null; // assign in the editor
    [SerializeField]
    private Button Btn_2 = null; // assign in the editor

	[SerializeField]
    private static UniPowerManager menuManager;
    #endregion
	
    #region Initialization
    void Start()
    {
        Btn_1.onClick.AddListener(() => { Btn_1_OnClick(); });
        Btn_2.onClick.AddListener(() => { Btn_2_OnClick(); });
    }
    #endregion
    
    #region Callbacks

    private void Btn_1_OnClick()
    {
		Debug.Log("clicked 1");

        ///Load the Power Gadget library
        LoadNativeDll("C:\\Program Files\\Intel\\Power Gadget 3.0\\EnergyLib32.dll");

        //Initialize and connect to the driver
        if (IntelEnergyLibInitialize() != true)
        {
            Debug.Log("Failed to initialized!");
        }
        else
        {
            Debug.Log("Initialized!");
        }

        if (pMSRCount == 0)
        {
            //Get the number of supported MSRs for bulk reading and logging
            if (GetNumMsrs(out pMSRCount) == true)
            {
                Debug.Log("Total supported MSRs: " + pMSRCount);
            }
        }
        else
        {
            Debug.Log("MSRs already queried: " + pMSRCount);
        }
    }
    private void Btn_2_OnClick()
    {
        Debug.Log("clicked 2");
    }

    /// <summary>
    /// Load a native library
    /// </summary>
    /// <param name="FileName"></param>
    static bool LoadNativeDll(string FileName)
    {
        //Make sure that the module isn't already loaded
        if (module != IntPtr.Zero)
        {
            Debug.Log("Total supported MSRs: " + pMSRCount);
            Debug.Log("Library has alreay been loaded.");
            return false;
        }

        //Load the module
        module = LoadLibrary(FileName);
        //sDebug.Log("last error = " + Marshal.GetLastWin32Error());

        //Make sure the module has loaded sucessfully
        if (module == IntPtr.Zero)
        {
            throw new Win32Exception();
        }
        else
        {
            Debug.Log("Library loaded.");
            return true;
        }
    }
	#endregion 
}

