using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EmptyRoomObjectScript : MonoBehaviour
{
    public Text text;
    public Text selectedC;
    public Material usedKey;
    public float textCD = 1.5f;
    private float currentTextCD = 0;
    public GameObject player;
    void Update()
    {
        checkText();
    }
    public void setText(string t)
    {
        text.transform.parent.gameObject.SetActive(true);
        text.text = t;
        currentTextCD = Time.time + textCD;
    }
    public void setText(string t, float f)
    {
        text.transform.parent.gameObject.SetActive(true);
        text.text = t;
        currentTextCD = Time.time + f;
    }
    void checkText()
    {
        if (Time.time > currentTextCD)
        {
            text.transform.parent.gameObject.SetActive(false);
        }
    }

    public void setSelected(string t)
    {
        selectedC.text = t;
    }

    public Material GetMaterial()
    {
        return usedKey;
    }

}
