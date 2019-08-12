using CloudData;
using Controllers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using UnityEngine.SceneManagement;
using System.IO;
using ObjectCreation;

public class SaveController : MonoBehaviour
{

    public bool SaveEnabled = false;
    public bool Overwrite = false;
    public bool restart = false;
    public bool SetPivot = false;
    public bool saved = false;

    void Update()
    {
        if (SaveEnabled)
        {
            saved = true;
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
            if (Overwrite)
            {
                data.RotateX = 0f;
                data.RotateY = 0f;
                data.RotateZ = 0f;
            }
            else
            {
                data.RotateX += gameObject.transform.eulerAngles.x;
                data.RotateY += gameObject.transform.eulerAngles.y;
                data.RotateZ += gameObject.transform.eulerAngles.z;
            }
            if (data.ScaleX == 0)
            {
                data.ScaleX = 1;
                data.ScaleY = 1;
                data.ScaleZ = 1;
            }
            else
            {
                data.ScaleX *= gameObject.transform.localScale.x;
                data.ScaleY *= gameObject.transform.localScale.y;
                data.ScaleZ *= gameObject.transform.localScale.z;
            }

            Vector3 scale = CloudsFromDirectoryLoader.boxscale[gameObject.name];
            data.boundingBox.lx *= data.ScaleX / scale.x;
            data.boundingBox.ly *= data.ScaleY / scale.y;
            data.boundingBox.lz *= data.ScaleZ / scale.z;
            data.boundingBox.ux *= data.ScaleX / scale.x;
            data.boundingBox.uy *= data.ScaleY / scale.y;
            data.boundingBox.uz *= data.ScaleZ / scale.z;
            data.tightBoundingBox.lx *= data.ScaleX / scale.x;
            data.tightBoundingBox.ly *= data.ScaleY / scale.y;
            data.tightBoundingBox.lz *= data.ScaleZ / scale.z;
            data.tightBoundingBox.ux *= data.ScaleX / scale.x;
            data.tightBoundingBox.uy *= data.ScaleY / scale.y;
            data.tightBoundingBox.uz *= data.ScaleZ / scale.z;
            data.boundingBox.olx *= data.ScaleX / scale.x;
            data.boundingBox.oly *= data.ScaleY / scale.y;
            data.boundingBox.olz *= data.ScaleZ / scale.z;
            data.boundingBox.oux *= data.ScaleX / scale.x;
            data.boundingBox.ouy *= data.ScaleY / scale.y;
            data.boundingBox.ouz *= data.ScaleZ / scale.z;

            Vector3d vector = ToVector3d(gameObject.transform.position);
            data.boundingBox.lx += vector.x - CloudsFromDirectoryLoader.boxoffset[gameObject.name].x;
            data.boundingBox.ly += vector.z - CloudsFromDirectoryLoader.boxoffset[gameObject.name].y;
            data.boundingBox.lz += vector.y - CloudsFromDirectoryLoader.boxoffset[gameObject.name].z;
            data.boundingBox.ux += vector.x - CloudsFromDirectoryLoader.boxoffset[gameObject.name].x;
            data.boundingBox.uy += vector.z - CloudsFromDirectoryLoader.boxoffset[gameObject.name].y;
            data.boundingBox.uz += vector.y - CloudsFromDirectoryLoader.boxoffset[gameObject.name].z;
            data.tightBoundingBox.lx += vector.x - CloudsFromDirectoryLoader.boxoffset[gameObject.name].x;
            data.tightBoundingBox.ly += vector.z - CloudsFromDirectoryLoader.boxoffset[gameObject.name].y;
            data.tightBoundingBox.lz += vector.y - CloudsFromDirectoryLoader.boxoffset[gameObject.name].z;
            data.tightBoundingBox.ux += vector.x - CloudsFromDirectoryLoader.boxoffset[gameObject.name].x;
            data.tightBoundingBox.uy += vector.z - CloudsFromDirectoryLoader.boxoffset[gameObject.name].y;
            data.tightBoundingBox.uz += vector.y - CloudsFromDirectoryLoader.boxoffset[gameObject.name].z;
            if (SetPivot)
            {
                data.boundingBox.olx = data.boundingBox.lx;
                data.boundingBox.oly = data.boundingBox.ly;
                data.boundingBox.olz = data.boundingBox.lz;
                data.boundingBox.oux = data.boundingBox.ux;
                data.boundingBox.ouy = data.boundingBox.uy;
                data.boundingBox.ouz = data.boundingBox.uz;
            }
            string output = JsonUtility.ToJson(data);
            File.WriteAllText(cloud + "cloud.js", output);
            Debug.Log("updated!");
            SaveEnabled = false;
        }
        if (restart)
        {
            GeoQuadMeshConfiguration.rotatelist.Clear();
            var script = GameObject.Find("PointSetController").GetComponent<PointCloudSetRealTimeController>();
            script.PointRenderer.ShutDown();
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
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