using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class OVRController : MonoBehaviour {

    //camera used by player in scene
    Camera maincamera;
    
    //player object in scene
    public GameObject player;

    //default speed of player
    public static float speed = 1;

    //orientation of the cloud
    private Vector3 rot;

    private float xrot = 0, yrot = 0, zrot = 0;
    private Vector3 nil;
	// Use this for initialization
	void Start () {
        
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
        float movehorizontal = Input.GetKey(KeyCode.A) || OVRInput.Get(OVRInput.Button.DpadLeft) ? -1 :
            Input.GetKey(KeyCode.D) || OVRInput.Get(OVRInput.Button.DpadRight) ? 1 : 0;
        float movevertical = Input.GetKey(KeyCode.S) || OVRInput.Get(OVRInput.Button.DpadDown) ? -1 :
            Input.GetKey(KeyCode.W) || OVRInput.Get(OVRInput.Button.DpadUp) ? 1 : 0;

        //set the player orientation to original to find direction of camera
        player.transform.rotation = Quaternion.Euler(nil);

        //find direction of camera
        Vector3 front = maincamera.transform.forward;
        Vector3 right = maincamera.transform.right;

        //set the orientation of player based on cloud
        setRotation();

        //move camera to simulate moving player
        transform.Translate(front * movevertical*speed);
        transform.Translate(right * movehorizontal*speed);
    }
    public void setRotation()
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
