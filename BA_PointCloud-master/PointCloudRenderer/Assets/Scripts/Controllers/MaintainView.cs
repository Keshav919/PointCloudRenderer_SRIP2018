using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MaintainView : MonoBehaviour {

    public Transform CameraTransform;
    public float distance;
    public bool AdjustPosition = false;
    public bool recorded = false;
    public Transform firstChild;
    public Vector3 firstChildIni = new Vector3 (0,0,0);

    // Update is called once per frame
    private void Start()
    {
        CameraTransform = GameObject.FindGameObjectWithTag("MainCamera").transform;
    }


    void Update () {
        if (AdjustPosition && !recorded)
        {
            firstChild = gameObject.transform.GetChild(0);
            if (firstChildIni == new Vector3(0, 0, 0))
            {
                firstChildIni = firstChild.position;
            }
            distance = Mathf.Abs(CameraTransform.position.z - firstChild.position.z);
            recorded = true;
            AdjustPosition = false;
        }
        else if (recorded)
        {
            Vector3 newposition = CameraTransform.position + CameraTransform.forward * distance - firstChildIni;
            var script = gameObject.GetComponent<DrawOutline>();
            if (!script.reloaded)
            {
                script.selected = true;
            }
            gameObject.transform.position = newposition;
            if (AdjustPosition)
            {
                recorded = false;
                AdjustPosition = false;
                script.selected = true;
            }
        }
    }
}
