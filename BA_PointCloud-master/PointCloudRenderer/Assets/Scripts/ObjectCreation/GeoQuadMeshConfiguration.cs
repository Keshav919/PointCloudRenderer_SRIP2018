using CloudData;
using Controllers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using System.IO;

namespace ObjectCreation
{

    /// <summary>
    /// What kind of interpolation to use
    /// </summary>
    enum InterpolationMode
    {
        /// <summary>
        /// No interpolation
        /// </summary>
        OFF,
        /// <summary>
        /// Exact paraboloids
        /// </summary>
        FRAGMENT_PARA,
        /// <summary>
        /// Exact cones
        /// </summary>
        FRAGMENT_CONE,
        /// <summary>
        /// Paraboloids approximated with 8 triangles
        /// </summary>
        GEOMETRY0,
        /// <summary>
        /// Paraboloids approximated with 16 triangles
        /// </summary>
        GEOMETRY1,
        /// <summary>
        /// Paraboloids approximated with 32 triangles
        /// </summary>
        GEOMETRY2,
        /// <summary>
        /// Paraboloids approximated with 48 triangles
        /// </summary>
        GEOMETRY3
    }

    /// <summary>
    /// Geometry Shader Quad Rendering, as described in the Bachelor Thesis in chapter 3.3.3.
    /// Creates a screen facing square or circle for each point using the Geometry Shader.
    /// Also supports various interpolation modes (see Thesis chapter 3.3.4 "Interpolation").
    /// This configuration also supports changes of the parameters while the application is running. Just change the parameters and check the checkbox "reload".
    /// </summary>
    class GeoQuadMeshConfiguration : MeshConfiguration
    {

        /// <summary>
        /// Radius of the point (in pixel or world units, depending on variable screenSize)
        /// </summary>
        public float pointRadius = 10;
        /// <summary>
        /// Whether the quads should be rendered as circles (true) or as squares (false)
        /// </summary>
        public bool renderCircles = true;
        /// <summary>
        /// True, if pointRadius should be interpreted as pixels, false if it should be interpreted as world units
        /// </summary>
        public bool screenSize = true;
        /// <summary>
        /// Wether and how to use interpolation
        /// </summary>
        public InterpolationMode interpolation = InterpolationMode.OFF;
        /// <summary>
        /// If changing the parameters should be possible during execution, this variable has to be set to true in the beginning! Later changes to this variable will not change anything
        /// </summary>
        public bool reloadingPossible = true;
        /// <summary>
        /// Set this to true to reload the shaders according to the changed parameters
        /// </summary>
        public bool reload = false;

        public bool RotateObject = false;

        public GameObject holder;

        public Vector3 offset;

        public Vector3 rotate;

        public Vector3 originalpos = new Vector3(0, 0, 0);

        public List<GameObject> holders;


        public Material[] Newmat;

        public static Dictionary<string, Vector3> boxlist = new Dictionary<string, Vector3>();
        public static Dictionary<string, List<string>> rotatelist = new Dictionary<string, List<string>>();
        //public Vector3[] rotatepoints;

        //public GameObject Cloudholder;


        public GameObject subholder;

        private Material material;
        private Camera mainCamera;
        private HashSet<GameObject> gameObjectCollection = null;

        private void LoadShaders()
        {

            if (interpolation == InterpolationMode.OFF)
            {
                if (screenSize )
                {
                    material = new Material(Shader.Find("Custom/QuadGeoScreenSizeShader"));
                }
                else
                {
                    material = new Material(Shader.Find("Custom/QuadGeoWorldSizeShader"));
                }
            }
            if (interpolation == InterpolationMode.GEOMETRY0 || interpolation == InterpolationMode.GEOMETRY1 || interpolation == InterpolationMode.GEOMETRY2 || interpolation == InterpolationMode.GEOMETRY3)
            {
                if (screenSize)
                {
                    material = new Material(Shader.Find("Custom/ParaboloidGeoScreenSizeShader"));
                }
                else
                {
                    material = new Material(Shader.Find("Custom/ParaboloidGeoWorldSizeShader"));
                }
                switch (interpolation)
                {
                    case InterpolationMode.GEOMETRY0:
                        material.SetInt("_Details", 0);
                        break;
                    case InterpolationMode.GEOMETRY1:
                        material.SetInt("_Details", 1);
                        break;
                    case InterpolationMode.GEOMETRY2:
                        material.SetInt("_Details", 2);
                        break;
                    case InterpolationMode.GEOMETRY3:
                        material.SetInt("_Details", 3);
                        break;
                }
            }
            else if (interpolation == InterpolationMode.FRAGMENT_PARA || interpolation == InterpolationMode.FRAGMENT_CONE)
            {
                if (screenSize)
                {
                    material = new Material(Shader.Find("Custom/ParaboloidFragScreenSizeShader"));
                }
                else
                {
                    material = new Material(Shader.Find("Custom/ParaboloidFragWorldSizeShader"));
                }
                material.SetInt("_Cones", (interpolation == InterpolationMode.FRAGMENT_CONE) ? 1 : 0);
            }
            material.SetFloat("_PointSize", pointRadius);
            material.SetInt("_Circles", renderCircles ? 1 : 0);
        }

        public void Start()
        {

            GameObject OutlineController = GameObject.Find("OutlineController");
            HighlightController HighlightController = OutlineController.GetComponent<HighlightController>();
            Newmat = HighlightController.Newmat;

            if (reloadingPossible)
            {
                gameObjectCollection = new HashSet<GameObject>();
            }

            GameObject directory = GameObject.Find("Directories");
            foreach (Transform directoryobject in directory.transform)
            {
                boxlist.Add(directoryobject.gameObject.name, new Vector3(0, 0, 0));
                string cloud = directoryobject.GetComponent<DynamicLoaderController>().cloudPath;
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
                PointCloudMetaData data = JsonUtility.FromJson<PointCloudMetaData>(jsonfile);
                boxlist[directoryobject.name] = new Vector3(data.RotateX, data.RotateY, data.RotateZ);
            }
            

            holders = new List<GameObject>();
            LoadShaders();
            mainCamera = Camera.main;
            Transform[] ts = GameObject.FindGameObjectWithTag("Holder").transform.GetComponentsInChildren<Transform>();
            foreach (Transform child in ts)
            {
                holders.Add(child.gameObject);
            }
        }

        public void Update()
        {
            if (reload && gameObjectCollection != null)
            {
                LoadShaders();
                foreach (GameObject go in gameObjectCollection)
                {
                    go.GetComponent<MeshRenderer>().material = material;
                }
                reload = false;
            }
            if (screenSize)
            {
                if (interpolation != InterpolationMode.OFF)
                {
                    Matrix4x4 invP = (GL.GetGPUProjectionMatrix(mainCamera.projectionMatrix, true)).inverse;
                    material.SetMatrix("_InverseProjMatrix", invP);
                    material.SetFloat("_FOV", Mathf.Deg2Rad * mainCamera.fieldOfView);
                }
                Rect screen = Camera.main.pixelRect;
                material.SetInt("_ScreenWidth", (int)screen.width);
                material.SetInt("_ScreenHeight", (int)screen.height);
            }
        }

        public override GameObject CreateGameObject(string name, Vector3[] vertexData, Color[] colorData, BoundingBox boundingBox)
        {
            GameObject gameObject = new GameObject(name);

            //keep the heirarchy clean by creating the game objects in the proper place
            if (holders.Count > 1)
            {
                foreach (GameObject child in holders)
                {
                    if (name.Contains(child.name))
                    {
                        gameObject.transform.parent = child.transform;
                    }
                }
                //boundingBox.UpdateBox(gameObject.transform.parent.GetComponent<CloudOffset>().GetOffset());
            }
            else
            {
                gameObject.transform.parent = GameObject.Find("Points Holder").transform;
            }

            Mesh mesh = new Mesh();

            MeshFilter filter = gameObject.AddComponent<MeshFilter>();
            filter.mesh = mesh;
            MeshRenderer renderer = gameObject.AddComponent<MeshRenderer>();
            renderer.shadowCastingMode = UnityEngine.Rendering.ShadowCastingMode.Off;
            renderer.receiveShadows = false;
            renderer.material = material;

            //Create a highlight
            if (gameObject.GetComponentInParent<DrawOutline>().reloaded == true)
            {
                Newmat[0] = renderer.material;
                renderer.materials = Newmat;
            }

            int[] indecies = new int[vertexData.Length];
            for (int i = 0; i < vertexData.Length; ++i)
            {
                indecies[i] = i;
            }
            mesh.vertices = vertexData;
            mesh.colors = colorData;
            mesh.SetIndices(indecies, MeshTopology.Points, 0);
            subholder = GetHolder(name);
            //Set Translation
            //boundingBox.UpdateBox(gameObject.transform.parent.GetComponent<CloudOffset>().offset);
            //Vector3 temp = boxlist[subholder.name];
            //Quaternion rotation = Quaternion.Euler(temp.x, temp.y, temp.z);
            //Matrix4x4 m = Matrix4x4.Rotate(rotation);
            //Vector3 newmin = m.MultiplyPoint3x4(boundingBox.Min().ToFloatVector());
            //gameObject.transform.Translate(newmin);
            List<string> existing;
            if (!rotatelist.TryGetValue(subholder.name, out existing))
            {
                existing = new List<string>();
                rotatelist[subholder.name] = existing;
            }

            if (!rotatelist[subholder.name].Contains(name))
            {
                Vector3 temp = boxlist[subholder.name];
                Quaternion rotation = Quaternion.Euler(temp.x, temp.y, temp.z);
                Matrix4x4 m = Matrix4x4.Rotate(rotation);
                Vector3 newmin = m.MultiplyPoint3x4(boundingBox.Min().ToFloatVector());
                Vector3 newmax = m.MultiplyPoint3x4(boundingBox.Max().ToFloatVector());
                boundingBox.lx = newmin.x;
                boundingBox.ly = newmin.y;
                boundingBox.lz = newmin.z;
                boundingBox.ux = newmax.x;
                boundingBox.uy = newmax.y;
                boundingBox.uz = newmax.z;
                rotatelist[subholder.name].Add(name);
                /*
                Vector3 temp = boxlist[subholder.name];
                Vector3 direction = boundingBox.Min().ToFloatVector() - subholder.transform.position;
                direction = Quaternion.Euler(temp.x, temp.y, temp.z) * direction;
                direction += subholder.transform.position;
                boundingBox.lx = direction.x;                               
                boundingBox.ly = direction.y;
                boundingBox.lz = direction.z;
                direction = boundingBox.Max().ToFloatVector() - subholder.transform.position;
                Debug.Log(subholder.transform.position);
                direction = Quaternion.Euler(temp.x, temp.y, temp.z) * direction;
                direction += subholder.transform.position;
                boundingBox.ux = direction.x;
                boundingBox.uy = direction.y;
                boundingBox.uz = direction.z;
                rotatelist[subholder.name].Add(name);*/
            }
            gameObject.transform.Translate(boundingBox.Min().ToFloatVector());
            offset = GetOffsets(subholder);
            rotate = GetRotation(subholder);
            gameObject.transform.Translate(offset);
            gameObject.transform.RotateAround(subholder.transform.position, Vector3.forward, rotate.z);
            gameObject.transform.RotateAround(subholder.transform.position, Vector3.right, rotate.x);
            gameObject.transform.RotateAround(subholder.transform.position, Vector3.up, rotate.y);
            //gameObject.transform.Translate(gameObject.transform.parent.GetComponent<CloudOffset>().offset);
            //boundingBox.UpdateBox(gameObject.transform.parent.GetComponent<CloudOffset>().offset);
            /*if (name.Contains("potreeConverted_S0"))
            Debug.Log(name + " " + boundingBox.ToString());
            */
            if (gameObjectCollection != null)
            {
                gameObjectCollection.Add(gameObject);
            }

            //gameObject.transform.localPosition += 2*offset;
            //boundingBox.UpdateBox(offset);
            return gameObject;
        }

        public override int GetMaximumPointsPerMesh()
        {
            return 65000;
        }

        public override void RemoveGameObject(GameObject gameObject)
        {
            if (gameObjectCollection != null)
            {
                gameObjectCollection.Remove(gameObject);
            }
            Destroy(gameObject.GetComponent<MeshFilter>().sharedMesh);
            Destroy(gameObject);
        }

        /*public Vector3 GetOffset()
        {
            holder = GameObject.FindGameObjectWithTag("Holder");
            if (originalpos != holder.transform.position)
            {
                offset = (holder.transform.position - originalpos);
                //Debug.Log("there is offset");
                //Debug.Log(offset.ToString());
                //currentpos = transform.position;
                return offset;
            }
            else
            {
                //Debug.Log(transform.ToString() + offset.ToString());
                //return originalpos;
                //Debug.Log("there is no offset");
                return new Vector3(0, 0, 0);
            }
        }
        */

        public Vector3 GetOffsets(GameObject subholder)
        {
            if (originalpos != subholder.transform.position)
            {
                offset = (subholder.transform.position - originalpos);
                return offset;
            }
            else
            {
                return new Vector3(0, 0, 0);
            }
        }

        public Vector3 GetRotation(GameObject subholder)
        {
            if (originalpos != subholder.transform.eulerAngles)
            {
                rotate = (subholder.transform.eulerAngles - originalpos);
                return rotate;
            }
            else
            {
                return new Vector3(0, 0, 0);
            }
        }

        public GameObject Getcloudholder()
        {
            GameObject position;
            position = GameObject.Find("Cloud_Holder");
            return position;
        }

        public GameObject GetHolder(string name)
        {
            GameObject subholder;
            string[] result;
            result = name.Split(new string[] { "/r" }, StringSplitOptions.None);
            name = result[0];
            holder = GameObject.FindGameObjectWithTag("Holder");
            subholder = holder.transform.Find(name).gameObject;
            return subholder;
        }
        private static Vector3d ToVector3d(Vector3 vector)
        {
            Vector3d newvector = new Vector3d(0, 0, 0);
            double x = vector.x;
            double y = vector.y;
            double z = vector.z;
            newvector.x = x;
            newvector.y = y;
            newvector.z = z;
            return newvector;
        }
        /*public void GetRotatePoints(GameObject subholder, Vector3 rotateangle)
        {
            if (RotateSwitch == true)
            {
                if (rotateangle.x != 0 && rotatepoints[0] != subholder.transform.position) 
                {
                    rotatepoints[0] = subholder.transform.position;
                }
                else if (rotateangle.y != 0 && rotatepoints[1] != subholder.transform.position)
                {
                    rotatepoints[1] = subholder.transform.position;
                }
                else if (rotateangle.z != 0 && rotatepoints[2] != subholder.transform.position)
                {
                    rotatepoints[2] = subholder.transform.position;
                }
            }
            else
            {
                return;
            }
        }*/
    }
}
