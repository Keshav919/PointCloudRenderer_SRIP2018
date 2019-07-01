using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DrawOutline : MonoBehaviour {

    public bool selected = false;
    public bool reloaded = false;
    public Material[] Newmat;
    public Material[] Oldmat = new Material[1];
    // Use this for initialization
    void Start () {
        GameObject OutlineController = GameObject.Find("OutlineController");
        HighlightController HighlightController = OutlineController.GetComponent<HighlightController>();
        Newmat = HighlightController.Newmat;
    }

    // Update is called once per frame
    private void Update()
    {
        if (selected == true && reloaded == false)
        {
            foreach (Transform child in transform)
            {
                MeshRenderer point = child.transform.gameObject.GetComponent<MeshRenderer>();
                Newmat[0] = point.material;
                point.materials = Newmat;
            }
            Oldmat[0] = Newmat[0];
            reloaded = true;
            selected = false;
        }

        else if (selected == true && reloaded == true)
        {
            foreach (Transform child in transform)
            {
                MeshRenderer point = child.transform.gameObject.GetComponent<MeshRenderer>();
                point.materials = Oldmat;
            }
            reloaded = false;
            selected = false;
        }

    }
}
