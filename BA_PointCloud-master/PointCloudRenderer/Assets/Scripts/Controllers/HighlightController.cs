using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HighlightController : MonoBehaviour {

    public Material[] Newmat = new Material[2];
    /*public void Highlight_Cloud()
    {
        if (selected == true)
        {
            foreach (Transform child in transform)
            {
                MeshRenderer point = child.transform.gameObject.GetComponent<MeshRenderer>();
                Material[] mat = point.materials;
                Newmat[0] = mat[0];
                point.materials = Newmat;
            }
        }

    }*/
}
