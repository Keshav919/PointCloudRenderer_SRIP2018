using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using CloudData;
using ObjectCreation;

public class OVRController_Advanced : MonoBehaviour {

    //camera used by player in scene
    Camera maincamera;
    
    //player object in scene
    private GameObject player;

    //default speed of player
    public static float speed = 1;

    //orientation of the cloud
    private Vector3 rot;
    
    private float xrot = 0, yrot = 0, zrot = 0;
    private Vector3 nil;
    

    private float movehorizontal=0,movevertical=0,moveup=0;

	// Use this for initialization
	void Start () {

        UnityEngine.XR.InputTracking.Recenter();
      
        //set the player object
        player = GameObject.FindGameObjectWithTag("Player");
        
        //set the camera
        maincamera = Camera.main;
        
        nil = new Vector3(0, 0, 0);
    }

    // Update is called once per frame
    void Update()
    {

        //take the key input from user
        movehorizontal = Input.GetKey(KeyCode.A) || OVRInput.Get(OVRInput.Button.DpadLeft) ? -1 :
            Input.GetKey(KeyCode.D) || OVRInput.Get(OVRInput.Button.DpadRight) ? 1 : 0;
        movevertical = Input.GetKey(KeyCode.S) || OVRInput.Get(OVRInput.Button.DpadDown) ? -1 :
            Input.GetKey(KeyCode.W) || OVRInput.Get(OVRInput.Button.DpadUp) ? 1 : 0;
        moveup = Input.GetKey(KeyCode.Q) || OVRInput.Get(OVRInput.Button.PrimaryShoulder) ? 1 :
            Input.GetKey(KeyCode.E) || OVRInput.Get(OVRInput.Button.SecondaryShoulder) ? -1 : 0;

        
        

        //find direction of camera
        Vector3 front = maincamera.transform.forward;
        Vector3 right = maincamera.transform.right;
        Vector3 up = maincamera.transform.up;

        //set the orientation of player based on cloud
        //SetRotation();
        
        //move camera to simulate moving player
        transform.Translate(front * movevertical * speed);
        transform.Translate(right * movehorizontal * speed);
        transform.Translate(up * moveup * speed);
    }
    public void SetRotation()
    {
        //store the slider values
        xrot = OVRSliderLabelUpdater.xrot;
        yrot = OVRSliderLabelUpdater.yrot;
        zrot = OVRSliderLabelUpdater.zrot;

        //create the orientation vector
        rot = new Vector3(xrot, yrot, zrot);
        
        //set the player orietnation
        player.transform.rotation = Quaternion.Euler(rot);
        
    }
        
}
