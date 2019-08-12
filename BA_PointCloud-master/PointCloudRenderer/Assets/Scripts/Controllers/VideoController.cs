using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Controllers;
using ObjectCreation;
using System;

public class VideoController : MonoBehaviour {

    public bool Hide = false;
    public bool active = false;
    public bool save = false;
    public bool initialize = false;
    public bool Show = false;
    public bool paused = false;
    public bool ActivateVideo = false;
    public CloudsFromDirectoryLoader loader;

    void Update ()
    {
		if (Hide)
        {
            Deactive();
            Hide = false;
        }
        if (Show)
        {
            ShowFrame();
            Show = false;
        }
        if (active)
        {
            active = false;
            StartCoroutine(NewShowVideo());
        }
        if (initialize)
        {
            initialize = false;
            GroupSetPivot();
        }
        if (save)
        {
            save = false;
            GroupSave();
        }
	}
    IEnumerator ShowVideo()
    {
        int a = 1;
        while (a < 101)
        {
            Debug.Log(a);
            GameObject frame = gameObject.transform.Find("frame" + a.ToString()).gameObject;
            frame.SetActive(true);
            while (paused)
            {
                yield return null;
            }
            yield return new WaitForSeconds(.1f);
            frame.SetActive(false);
            a++;
        }
    }

    private void Deactive()
    {
        foreach (Transform child in gameObject.transform)
        {
            child.gameObject.SetActive(false);
        }
    }

    private void GroupSetPivot()
    {
        foreach (Transform child in gameObject.transform)
        {
            var script = child.gameObject.GetComponent<SaveController>();
            script.SetPivot = true;
            script.SaveEnabled = true;
        }
    }

    private void GroupSave()
    {
        foreach (Transform child in gameObject.transform)
        {
            child.Rotate(90,0,0);
            //Debug.Log(gameObject.transform.eulerAngles);
            var script = child.gameObject.GetComponent<SaveController>();
            //script.Overwrite = true;
            script.SaveEnabled = true;
        }
    }

    private void ShowFrame()
    {
        foreach (Transform child in gameObject.transform)
        {
            child.gameObject.SetActive(true);
        }
    }

    IEnumerator NewShowVideo()
    {
        int a = 1;
        int i = 21;
        while (a < 101)
        {
            if (i < 101)
            {
                try
                {
                    GameObject holder = gameObject.transform.Find("frame" + i.ToString()).gameObject;
                }
                catch (Exception e)
                {
                    GameObject holder = loader.LoadFrame("frame" + i.ToString());
                    holder.SetActive(false);
                }
            }
            GameObject frame = gameObject.transform.Find("frame" + a.ToString()).gameObject;
            frame.SetActive(true);
            while (paused)
            {
                yield return null;
            }
            yield return new WaitForSeconds(.1f);
            //GeoQuadMeshConfiguration meshconfig = GameObject.FindWithTag("MeshConfig").GetComponent<GeoQuadMeshConfiguration>();
            //meshconfig.holdername.Remove(frame.name);
            //meshconfig.holders.Remove(frame);
            frame.SetActive(false);
            a++;
            i++;
        }
    }

}