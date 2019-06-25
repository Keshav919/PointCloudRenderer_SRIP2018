using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

namespace Controllers {
    /// <summary>
    /// Use this loader, if you have several pointcloud-folders in the same directory and want to load all of them at once.
    /// This controller will create a DynamicLoaderController for each of the point clouds.
    /// </summary>
    public class CloudsFromDirectoryLoader : MonoBehaviour {

        /// <summary>
        /// Path of the directory containing the point clouds
        /// </summary>
        public string path;
        /// <summary>
        /// The PointSetController
        /// </summary>
        public AbstractPointSetController pointset;

        public string filename;


        void Start() {
            DirectoryInfo dir = new DirectoryInfo(path);
            foreach (DirectoryInfo sub in dir.GetDirectories()) {
                GameObject go = new GameObject(sub.Name);
                filename = sub.Name;
                //store the loaders in the proper game object
                go.transform.parent = GameObject.FindGameObjectWithTag("Directories").transform;
                
                //create a holder for the point clouds and store the point clouds in them
                GameObject holder = new GameObject(sub.Name);
                //gameObject.tag = filename;
                holder.transform.parent = GameObject.FindGameObjectWithTag("Holder").transform;
                //holder.AddComponent<Rotate>();

                DynamicLoaderController loader = go.AddComponent<DynamicLoaderController>();
                loader.cloudPath = sub.FullName;
                loader.setController = pointset;
                pointset.RegisterController(loader);
            }
        }
    }
}