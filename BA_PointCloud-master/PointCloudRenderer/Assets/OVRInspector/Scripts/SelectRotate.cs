using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SelectRotate : MonoBehaviour {

    public SelectObject script;
    public Slider slider;
    public OVRController_Advanced player;

    private void Start()
    {
        script = GameObject.FindGameObjectWithTag("Menu").GetComponent<SelectObject>();
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<OVRController_Advanced>();
    }

    public void SetRotationX()
    {
        if (script.chosen)
        {
            GameObject selected = script.holder.Find(script.cloud[script.count - 1]).gameObject;
            Vector3 rotation = selected.transform.rotation.eulerAngles;
            selected.transform.rotation = Quaternion.Euler(slider.value, rotation.y, rotation.z);
            GameObject value = slider.transform.Find("Value").gameObject;
            value.GetComponent<Text>().text = slider.value.ToString();
        }
        return;
    }

    public void SetRotationY()
    {
        if (script.chosen)
        {
            GameObject selected = script.holder.Find(script.cloud[script.count - 1]).gameObject;
            Vector3 rotation = selected.transform.rotation.eulerAngles;
            selected.transform.rotation = Quaternion.Euler(rotation.x, slider.value, rotation.z);
            GameObject value = slider.transform.Find("Value").gameObject;
            value.GetComponent<Text>().text = slider.value.ToString();
        }
        return;
    }

    public void SetRotationZ()
    {
        if (script.chosen)
        {
            GameObject selected = script.holder.Find(script.cloud[script.count - 1]).gameObject;
            Vector3 rotation = selected.transform.rotation.eulerAngles;
            selected.transform.rotation = Quaternion.Euler(rotation.x, rotation.y, slider.value);
            GameObject value = slider.transform.Find("Value").gameObject;
            value.GetComponent<Text>().text = slider.value.ToString();
        }
        return;
    }

    public void SetSpeed()
    {
        player.speed = slider.value;
        GameObject value = slider.transform.Find("Value").gameObject;
        value.GetComponent<Text>().text = slider.value.ToString();
    }

}
