/************************************************************************************

Copyright   :   Copyright 2017 Oculus VR, LLC. All Rights reserved.

Licensed under the Oculus VR Rift SDK License Version 3.4.1 (the "License");
you may not use the Oculus VR Rift SDK except in compliance with the License,
which is provided at the time of installation or download, or which
otherwise accompanies this software in either electronic or hard copy form.

You may obtain a copy of the License at

https://developer.oculus.com/licenses/sdk-3.4.1


Unless required by applicable law or agreed to in writing, the Oculus VR SDK
distributed under the License is distributed on an "AS IS" BASIS,
WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
See the License for the specific language governing permissions and
limitations under the License.

************************************************************************************/

using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using CloudData;
using ObjectCreation;


/// <summary>
/// Keeps a label associated with a slider reflecting the current value of the slider
/// </summary>
public class OVRSliderLabelUpdater : MonoBehaviour {
    public Text sliderValueField;
    public Slider slider;
    public bool setLabelAtStart = true;
    private GameObject player;

    public static float xrot, yrot, zrot;

    private static GeoQuadMeshConfiguration mesh;
    

    private GameObject meshconfig;


	// Use this for initialization
	void Start () {

        meshconfig = GameObject.FindGameObjectWithTag("MeshConfig");
        player = GameObject.FindGameObjectWithTag("Player");
        xrot = player.transform.eulerAngles.x;
        yrot = player.transform.eulerAngles.y;
        zrot = player.transform.eulerAngles.z;

        if (setLabelAtStart)
            SetValue(slider.value);
        mesh = meshconfig.GetComponent<GeoQuadMeshConfiguration>();
        mesh.reload = true;
    }

    /*
     * The functions that will control the features of the point cloud like the point radius, rotation of the cloud
     */

    public void SetValue(float v)
    {
        if (sliderValueField != null)
            sliderValueField.text = string.Format("{0:0.00}",v);
        
    }
    public void SetRadiusValue(float v)
    {
        if (sliderValueField != null)
            sliderValueField.text = string.Format("{0:0.00}", v);
        
        mesh.pointRadius = slider.value;
        mesh.reload = true;
    }
    public void SetXRotValue(float v)
    {
        if (sliderValueField != null)
            sliderValueField.text = string.Format("{0:0.00}", v);

        xrot = slider.value;
        

    }

    public void SetYRotValue(float v)
    {
        if (sliderValueField != null)
            sliderValueField.text = string.Format("{0:0.00}", v);

        yrot = slider.value;

    }

    public void SetZRotValue(float v)
    {
        if (sliderValueField != null)
            sliderValueField.text = string.Format("{0:0.00}", v);

        zrot = slider.value;
    }
    public void SetSpeed(float v)
    {
        if (sliderValueField != null)
            sliderValueField.text = string.Format("{0:0.00}", v);

        OVRController.speed = slider.value;
        Debug.Log(slider.value);
    }
}
