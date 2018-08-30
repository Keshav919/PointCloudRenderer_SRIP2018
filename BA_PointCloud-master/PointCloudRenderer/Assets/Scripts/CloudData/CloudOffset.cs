using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CloudOffset : MonoBehaviour {

    private Vector3 originalpos;
    public Vector3 offset;
    public string cloudName;
	// Use this for initialization
	void Start () {
        originalpos = new Vector3(0, 0, 0);
        offset = new Vector3(0, 0, 0);
        
        cloudName = gameObject.name;
	}

    // Update is called once per frame
    void Update()
    {
        if (originalpos != transform.position)
        {
            offset = (transform.position - originalpos);
            //currentpos = transform.position;
        }
        else
        {
            //Debug.Log(transform.ToString() + offset.ToString());
        }
    }
}
