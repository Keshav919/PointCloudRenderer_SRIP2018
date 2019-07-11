using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SelectObject : MonoBehaviour {

    public List<string> cloud;
    public bool chosen = true;
    public Transform holder;

    public int count = 0;

	void Start () {
        cloud = new List<string>();
        holder = GameObject.FindGameObjectWithTag("Holder").transform;
        foreach (Transform pointcloud in holder)
        {
            cloud.Add(pointcloud.name);
        }
	}
	
    public void SelectCloud()
    {
        if (count == cloud.Count)
        {
            GameObject previous = holder.Find(cloud[count - 1]).gameObject;
            previous.GetComponent<DrawOutline>().selected = true;
            count = 0;
            return;
        }
        if (count != 0)
        {
            GameObject previous = holder.Find(cloud[count - 1]).gameObject;
            previous.GetComponent<DrawOutline>().selected = true;
        }
        GameObject selected = holder.Find(cloud[count]).gameObject;
        selected.GetComponent<DrawOutline>().selected = true;
        count++;
    }

    public void SelectPosition()
    {
        if (count == 0)
        {
            return;
        }

        else if (chosen)
        {
            GameObject selected = holder.Find(cloud[count-1]).gameObject;
            selected.GetComponent<DrawOutline>().selected = true;
            selected.GetComponent<MaintainView>().AdjustPosition = true;
            chosen = !chosen;
        }
        else if (!chosen)
        {
            GameObject selected = holder.Find(cloud[count-1]).gameObject;
            selected.GetComponent<MaintainView>().AdjustPosition = true;
            selected.GetComponent<DrawOutline>().selected = true;
            chosen = !chosen;
        }

    }

    public void SavePointCloud()
    {
        GameObject selected = holder.Find(cloud[count - 1]).gameObject;
        var save = selected.GetComponent<SaveController>();
        save.SaveEnabled = true;
    }

}
