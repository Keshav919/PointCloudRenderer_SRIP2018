using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class OVRController : MonoBehaviour {

    Camera maincamera;
    public int speed;
	// Use this for initialization
	void Start () {
        maincamera = Camera.main;
	}

    // Update is called once per frame
    void Update()
    {

        float movehorizontal = Input.GetKey(KeyCode.A) || OVRInput.Get(OVRInput.Button.DpadLeft) ? -1 :
            Input.GetKey(KeyCode.D) || OVRInput.Get(OVRInput.Button.DpadRight) ? 1 : 0;
        float movevertical = Input.GetKey(KeyCode.S) || OVRInput.Get(OVRInput.Button.DpadDown) ? -1 :
            Input.GetKey(KeyCode.W) || OVRInput.Get(OVRInput.Button.DpadUp) ? 1 : 0;
        
        Vector3 front = maincamera.transform.forward;
        Vector3 right = maincamera.transform.right;
        transform.Translate(front * movevertical*speed);
        transform.Translate(right * movehorizontal*speed);
        
        
    }
        
}
