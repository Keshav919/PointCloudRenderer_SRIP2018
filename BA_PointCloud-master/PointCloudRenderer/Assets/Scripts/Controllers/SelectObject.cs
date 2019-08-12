using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using ObjectCreation;
using Loading;
using Controllers;
using UnityEngine.UI;

public class SelectObject : MonoBehaviour {

    public List<string> cloud;
    public Transform holder;
    public int count = 0;
    public Dropdown dropdown;
    public int dropdownvalue;


    void Start () {
        cloud = new List<string>();
        holder = GameObject.FindGameObjectWithTag("Holder").transform;
        foreach (Transform pointcloud in holder)
        {
            cloud.Add(pointcloud.name);
        }
        dropdown.ClearOptions();
        dropdown.AddOptions(cloud);
	}

    void Update()
    {
        dropdownvalue = dropdown.value;
    }


    public void SavePointCloud()
    {
        GameObject selected = holder.Find(dropdown.options[dropdownvalue].text).gameObject;
        var save = selected.GetComponent<SaveController>();
        if (!save.saved)
        {
            //if (CloudsFromDirectoryLoader.boxlist[gameObject.name] != new Vector3(0, 0, 0) && gameObject.transform.eulerAngles != new Vector3(0,0,0))
           // {
//save.Overwrite = true;
           // }
            save.SaveEnabled = true;
        }
        return;
    }

    public void RestartScene()
    {
        GameObject selected = holder.Find(dropdown.options[dropdownvalue].text).gameObject;
        selected.GetComponent<SaveController>().restart = true;
    }

    public void SetPivot()
    {
        GameObject selected = holder.Find(dropdown.options[dropdownvalue].text).gameObject;
        selected.GetComponent<SaveController>().SetPivot = true;
    }


    public void MoveCloud()
    {
        GameObject selected = holder.Find(dropdown.options[dropdownvalue].text).gameObject;
        selected.GetComponent<MaintainView>().AdjustPosition = true;
    }

}
