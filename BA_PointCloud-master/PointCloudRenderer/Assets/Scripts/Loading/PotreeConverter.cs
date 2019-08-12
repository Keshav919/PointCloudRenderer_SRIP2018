using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Diagnostics;
using System;
using Controllers;
using System.IO;

public class PotreeConverter : MonoBehaviour
{
    private GameObject loader;
    public string output;
    public bool convert = false;
    public string CloudPath;

    // Use this for initialization
    void Start()
    {
        //DynamicLoaderController cont;
        //loader = GameObject.FindGameObjectWithTag("Loader");
        //cont = loader.GetComponent<DynamicLoaderController>();

        // output = output + "_S" + spacing;
        string[] filepath = Directory.GetFiles(CloudPath);
        int a = 1;
        try
        {
            foreach (string file in filepath)
            {
                Process myProcess = new Process();
                myProcess.StartInfo.FileName = "C:\\Users\\David\\Desktop\\PotreeConverter_1.6_2018_07_29_windows_x64\\PotreeConverter.exe";
                myProcess.StartInfo.Arguments = file + " -o " + output + a.ToString(); //+ " -s " + spacing;
                myProcess.Start();
                myProcess.WaitForExit();
                a++;
            }

            //cont.cloudPath = output;
        }
        catch (Exception e)
        {
            print(e);
        }
    }

}
