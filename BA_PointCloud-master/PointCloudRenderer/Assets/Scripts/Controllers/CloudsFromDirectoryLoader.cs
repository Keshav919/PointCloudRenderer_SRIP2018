using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using System.Text;
using CloudData;
using ObjectCreation;
using Controllers;

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
        public static Dictionary<string, Vector3> boxscale = new Dictionary<string, Vector3>();
        private PointCloudMetaData data;
        public VideoController video;

        void Start()
        {
            if (video.ActivateVideo)
            {
                NewInitialize();
            }
            OldInitialize();
        }
        public void OldInitialize()
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
                if (data.ScaleX == 0)
                {
                    boxscale[sub.Name] = new Vector3(1, 1, 1);
                }
                else
                {
                    boxscale[sub.Name] = new Vector3(data.ScaleX, data.ScaleY, data.ScaleZ);
                }
                holder.transform.Translate(new Vector3(boxoffset[sub.Name].x, boxoffset[sub.Name].z, boxoffset[sub.Name].y));
            }
        }

        public void NewInitialize()
        {
            DirectoryInfo dir = new DirectoryInfo(path);
            int i = 1;
            while (i < 21)
            {
                foreach (DirectoryInfo sub in dir.GetDirectories())
                {
                    if (sub.Name == "frame" + i.ToString())
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
                        if (data.ScaleX == 0)
                        {
                            boxscale[sub.Name] = new Vector3(1, 1, 1);
                        }
                        else
                        {
                            boxscale[sub.Name] = new Vector3(data.ScaleX, data.ScaleY, data.ScaleZ);
                        }
                        holder.transform.Translate(new Vector3(boxoffset[sub.Name].x, boxoffset[sub.Name].z, boxoffset[sub.Name].y));
                    }
                }
                i++;
            }
        }

        public GameObject LoadFrame(string name)
        {
            var geomesh = GameObject.FindWithTag("MeshConfig").GetComponent<GeoQuadMeshConfiguration>();
            DirectoryInfo[] temp = new DirectoryInfo(path).GetDirectories(name);
            DirectoryInfo sub = temp[0];
            GameObject go = new GameObject(sub.Name);
            filename = sub.Name;
            go.transform.parent = GameObject.FindGameObjectWithTag("Directories").transform;
            GameObject holder = new GameObject(sub.Name);
            holder.transform.parent = GameObject.FindGameObjectWithTag("Holder").transform;
            holder.AddComponent<DrawOutline>();
            holder.AddComponent<MaintainView>();
            holder.AddComponent<SaveController>();
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
            if (data.ScaleX == 0)
            {
                boxscale[sub.Name] = new Vector3(1, 1, 1);
            }
            else
            {
                boxscale[sub.Name] = new Vector3(data.ScaleX, data.ScaleY, data.ScaleZ);
            }
            holder.transform.Translate(new Vector3(boxoffset[sub.Name].x, boxoffset[sub.Name].z, boxoffset[sub.Name].y));
            geomesh.holders.Add(holder);
            //geomesh.holdername.Add(holder.name);
            return holder;
        }
    }
}