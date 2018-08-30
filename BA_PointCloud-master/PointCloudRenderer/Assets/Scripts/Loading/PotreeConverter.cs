using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Diagnostics;
using System;
using Controllers;

public class PotreeConverter : MonoBehaviour
{
    private GameObject loader;
    public string input;
    public string output;
    public bool convert = false;
    public int spacing;

    // Use this for initialization
    void Start()
    {
        DynamicLoaderController cont;
        loader = GameObject.FindGameObjectWithTag("Loader");
        cont = loader.GetComponent<DynamicLoaderController>();

        output = output + "_S" + spacing;

        try
        {
            Process myProcess = new Process();
            myProcess.StartInfo.FileName = "C:\\dev\\workspaces\\PotreeConverter\\master\\"
                + "PotreeConverter_1.6_windows_x64\\PotreeConverter_1.6_windows_x64\\"
                + "PotreeConverter.exe";

            myProcess.StartInfo.Arguments = "/c" + "-i " + input + " -o " + output + " -s " + spacing;

            if (convert)
            {
                myProcess.Start();
                myProcess.WaitForExit();
            }
            
            cont.cloudPath = output;
        }
        catch (Exception e)
        {
            print(e);
        }
    }

}
