using CloudData;
using Controllers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using System.IO;

public class SaveRotation : MonoBehaviour
{/*
    public Vector3 eulerAngles;
    private MeshFilter mf;
    private Vector3[] origVerts;
    private Vector3[] newVerts;

    void Start()
    {
        GameObject child = gameObject.GetComponent<GameObject>();
        child.GetComponent<MeshFilter>();
    }

    void Update()
    {
        Quaternion rotation = Quaternion.Euler(eulerAngles.x, eulerAngles.y, eulerAngles.z);
        Matrix4x4 m = Matrix4x4.Rotate(rotation);
        int i = 0;
        while (i < origVerts.Length)
        {
            newVerts[i] = m.MultiplyPoint3x4(origVerts[i]);
            i++;
        }
        mf.mesh.vertices = newVerts;
    }*/

    // Use this for initialization

    /*public bool Save = false;

    private void Update()
    {
        if (Save)
        {
            Save = false;
            mlapp.MLApp matlab = new mlapp.MLApp();
            matlab.Execute(@"cd C:\Users\David\Desktop\PotreeConverter_1.6_2018_07_29_windows_x64");
            float x = gameObject.transform.rotation.x;
            float y = gameObject.transform.rotation.y;
            float z = gameObject.transform.rotation.z;
                //string name = "C:/Users/David/Desktop/PotreeConverter_1.6_2018_07_29_windows_x64/seaport.ply";
            object result = null;
            matlab.Feval("rotatecloud", 1, out result, x, y, z);

            
        }
    }
    */
}

