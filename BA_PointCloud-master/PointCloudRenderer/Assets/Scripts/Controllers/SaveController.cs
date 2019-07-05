using CloudData;
using Controllers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using System.IO;

public class SaveController : MonoBehaviour {

    public bool SaveEnabled;

	void Update ()
    {
        if (SaveEnabled)
        {
            Vector3 newposition = gameObject.transform.position;
            GameObject directory = GameObject.Find("Directories");
            GameObject dirctoryobject = directory.transform.Find(gameObject.name).gameObject;
            string cloud = dirctoryobject.GetComponent<DynamicLoaderController>().cloudPath;
            string jsonfile;
            using (StreamReader reader = new StreamReader(cloud + "cloud.js", Encoding.Default))
            {
                jsonfile = reader.ReadToEnd();
                reader.Close();
            }
            PointCloudMetaData data = JsonUtility.FromJson<PointCloudMetaData>(jsonfile);
            data.RotateX = gameObject.transform.eulerAngles.x;
            data.RotateY = gameObject.transform.eulerAngles.y;
            data.RotateZ = gameObject.transform.eulerAngles.z;
            Vector3d vector = ToVector3d(gameObject.transform.position);
            data.boundingBox.lx += vector.x;
            data.boundingBox.ly += vector.z;
            data.boundingBox.lz += vector.y;
            data.boundingBox.ux += vector.x;
            data.boundingBox.uy += vector.z;
            data.boundingBox.uz += vector.y;
            data.tightBoundingBox.lx += vector.x;
            data.tightBoundingBox.ly += vector.z;
            data.tightBoundingBox.lz += vector.y;
            data.tightBoundingBox.ux += vector.x;
            data.tightBoundingBox.uy += vector.z;
            data.tightBoundingBox.uz += vector.y;
            string output = JsonUtility.ToJson(data);
            File.WriteAllText(cloud + "cloud.js", output);
            Debug.Log("updated!");
            SaveEnabled = false;

        }
	}

    public Vector3d ToVector3d(Vector3 vector)
    {
        Vector3d newvector = new Vector3d(0, 0, 0);
        Double x = vector.x;
        Double y = vector.y;
        Double z = vector.z;
        newvector.x = x;
        newvector.y = y;
        newvector.z = z;
        return newvector;
    }

}
