using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using CloudData;
using ObjectCreation;

public class MenuControl : MonoBehaviour {

    Camera maincamera;

    Vector3 campos, menupos, dis;
    float distance;
    public bool hide = true;
    private GameObject menu;
	// Use this for initialization
	void Start () {

        maincamera = Camera.main;
        campos = maincamera.transform.position;
        
        menu = GameObject.FindGameObjectWithTag("Menu");
        menupos = menu.transform.position;

        dis = menupos - campos;
        distance = Mathf.Sqrt(dis.x * dis.x + dis.y * dis.y + dis.y * dis.y);
        
    }

    // Update is called once per frame
    void LateUpdate()
    {
        if (OVRInput.GetDown(OVRInput.Button.Two) || Input.GetKeyDown(KeyCode.M))
        {
            hide = !hide;
        }

        menu.SetActive(!hide);

        if (hide == true)
        {
            menupos = campos + maincamera.transform.forward * distance;
            menu.transform.forward = maincamera.transform.forward;
            
            menu.transform.position = menupos;
        }
        if(hide == false)
        {
            menu.transform.localRotation = Quaternion.Euler(new Vector3(menu.transform.localEulerAngles.x,
                menu.transform.localEulerAngles.y, -1.7f));
        }
        campos = maincamera.transform.position;
    }
    
}
