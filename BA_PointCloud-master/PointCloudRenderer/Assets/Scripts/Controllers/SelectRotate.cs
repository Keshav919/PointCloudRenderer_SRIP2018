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
        GameObject selected = script.holder.Find(script.dropdown.options[script.dropdownvalue].text).gameObject;
        Vector3 rotation = selected.transform.rotation.eulerAngles;
        selected.transform.rotation = Quaternion.Euler(slider.value, rotation.y, rotation.z);
        GameObject value = slider.transform.Find("Value").gameObject;
        value.GetComponent<Text>().text = slider.value.ToString();
    }

    public void SetRotationY()
    {
        GameObject selected = script.holder.Find(script.dropdown.options[script.dropdownvalue].text).gameObject;
        Vector3 rotation = selected.transform.rotation.eulerAngles;
        selected.transform.rotation = Quaternion.Euler(rotation.x, slider.value, rotation.z);
        GameObject value = slider.transform.Find("Value").gameObject;
        value.GetComponent<Text>().text = slider.value.ToString();
    }

    public void SetRotationZ()
    {
        GameObject selected = script.holder.Find(script.dropdown.options[script.dropdownvalue].text).gameObject;
        Vector3 rotation = selected.transform.rotation.eulerAngles;
        selected.transform.rotation = Quaternion.Euler(rotation.x, rotation.y, slider.value);
        GameObject value = slider.transform.Find("Value").gameObject;
        value.GetComponent<Text>().text = slider.value.ToString();
    }

    public void SetSpeed()
    {
        player.speed = slider.value;
        GameObject value = slider.transform.Find("Value").gameObject;
        value.GetComponent<Text>().text = slider.value.ToString();
    }

    public void SetScaleX()
    {
        GameObject selected = script.holder.Find(script.dropdown.options[script.dropdownvalue].text).gameObject;
        Vector3 scale = selected.transform.localScale;
        selected.transform.localScale = new Vector3(slider.value, scale.y, scale.z);
        GameObject value = slider.transform.Find("Value").gameObject;
        value.GetComponent<Text>().text = slider.value.ToString();
    }
    public void SetScaleY()
    {
        if (script.chosen)
        {
            GameObject selected = script.holder.Find(script.dropdown.options[script.dropdownvalue].text).gameObject;
            Vector3 scale = selected.transform.localScale;
            selected.transform.localScale = new Vector3(scale.x, slider.value, scale.z);
            GameObject value = slider.transform.Find("Value").gameObject;
            value.GetComponent<Text>().text = slider.value.ToString();
        }
    }
    public void SetScaleZ()
    {
        if (script.chosen)
        {
            GameObject selected = script.holder.Find(script.dropdown.options[script.dropdownvalue].text).gameObject;
            Vector3 scale = selected.transform.localScale;
            selected.transform.localScale = new Vector3(scale.x, scale.y, slider.value);
            GameObject value = slider.transform.Find("Value").gameObject;
            value.GetComponent<Text>().text = slider.value.ToString();
        }
    }

}
