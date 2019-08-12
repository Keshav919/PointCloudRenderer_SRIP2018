using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CloudOffset : MonoBehaviour {

    private Vector3 originalpos;
    public Vector3 offset;
    public string cloudName;
    public Vector3 original;
	// Use this for initialization
	void Start () {
        originalpos = new Vector3(0, 0, 0);
        offset = new Vector3(0, 0, 0);
        
        cloudName = gameObject.name;
	}

    void Update()
    {
        offset = GetOffset();
        if (originalpos != transform.position)
        {
            UpdateChild();
        }
    }

    // Update is called once per frame
    public Vector3 GetOffset()
    {
        if (originalpos != transform.position)
        {
            offset = (transform.position - originalpos);
            Debug.Log("there is offset");
            Debug.Log(offset.ToString());
            //currentpos = transform.position;
            return offset;
        }
        else
        {
            //Debug.Log(transform.ToString() + offset.ToString());
            //return originalpos;
            Debug.Log("there is no offset");
            return new Vector3(0, 0, 0);
        }
    }

    public void UpdateChild()
    {
        foreach (Transform child in transform)
        {
            child.localPosition += offset;
        }
        originalpos = transform.position;
    }
}
