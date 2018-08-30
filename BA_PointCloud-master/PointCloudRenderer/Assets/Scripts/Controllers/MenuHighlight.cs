using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using CloudData;
using ObjectCreation;

public class MenuHighlight : MonoBehaviour {

    public Image backgroundImage;
    public Color normalColor, highlightColor;

    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public void OnGazeEnter()
    {
        backgroundImage.color = highlightColor;
    }

    public void OnGazeExit()
    {
        backgroundImage.color = normalColor;
    }
    
}
