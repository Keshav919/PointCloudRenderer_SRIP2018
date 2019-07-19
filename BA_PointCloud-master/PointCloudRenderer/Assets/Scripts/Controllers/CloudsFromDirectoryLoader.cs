using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using System.Text;
using CloudData;

namespace Controllers
{
    /// <summary>
    /// Use this loader, if you have several pointcloud-folders in the same directory and want to load all of them at once.
    /// This controller will create a DynamicLoaderController for each of the point clouds.
    /// </summary>
    public class CloudsFromDirectoryLoader : MonoBehaviour
    {

        /// <summary>
        /// Path of the directory containing the point clouds
        /// </summary>
        public string path;
        /// <summary>
        /// The PointSetController
        /// </summary>
        public AbstractPointSetController pointset;

        public string filename;
        public static Dictionary<string, Vector3> boxlist = new Dictionary<string, Vector3>();
        public static Dictionary<string, Vector3> boxoffset = new Dictionary<string, Vector3>();
        private PointCloudMetaData data;

        void Start()
        {
            DirectoryInfo dir = new DirectoryInfo(path);
            foreach (DirectoryInfo sub in dir.GetDirectories())
            {
                GameObject go = new GameObject(sub.Name);
                filename = sub.Name;
                //store the loaders in the proper game object
                go.transform.parent = GameObject.FindGameObjectWithTag("Directories").transform;

                //create a holder for the point clouds and store the point clouds in them
                GameObject holder = new GameObject(sub.Name);
                //gameObject.tag = filename;
                holder.transform.parent = GameObject.FindGameObjectWithTag("Holder").transform;
                holder.AddComponent<DrawOutline>();
                holder.AddComponent<MaintainView>();
                holder.AddComponent<SaveController>();
                //holder.transform.Translate(10, 0, 0);
                //holder.AddComponent<SaveRotation>();
                DynamicLoaderController loader = go.AddComponent<DynamicLoaderController>();
                loader.cloudPath = sub.FullName;
                loader.setController = pointset;
                pointset.RegisterController(loader);
                string cloud = loader.cloudPath;
                if (!cloud.EndsWith("\\"))
                {
                    cloud = cloud + "\\";
                }
                string jsonfile;
                using (StreamReader reader = new StreamReader(cloud + "cloud.js", Encoding.Default))
                {
                    jsonfile = reader.ReadToEnd();
                    reader.Close();
                }
                data = JsonUtility.FromJson<PointCloudMetaData>(jsonfile);
                boxlist[sub.Name] = new Vector3(data.RotateX, data.RotateY, data.RotateZ);
                boxoffset[sub.Name] = new Vector3((float)(data.boundingBox.lx - data.boundingBox.olx), (float)(data.boundingBox.ly - data.boundingBox.oly), (float)(data.boundingBox.lz - data.boundingBox.olz));
            }
        }
    }
}