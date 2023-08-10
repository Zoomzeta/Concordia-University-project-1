using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BarScript : MonoBehaviour
{
    //[SerializeField]  //Serialization is temporary for testing purposes
    private float fillAmount;

    [SerializeField]
    private float lerpSpeed;

    [SerializeField]
    private Image imageContent;

    [SerializeField]
    private Text valueText;

    //Set health Max value
    public float MaxValue { get; set; }
   
    // This method sets the value of fillAmount
    public float Value
    {
        set
        {
            valueText.text = "" + value;
            fillAmount = Map(value, 0, MaxValue, 0, 1);
            
        }
    }


    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        HandleHealthBar();
    }

    private void HandleHealthBar()
    {
        //Update only if necessary
        if(imageContent.fillAmount != fillAmount)
        {
            imageContent.fillAmount = Mathf.Lerp(imageContent.fillAmount, fillAmount, Time.deltaTime * lerpSpeed);
        }
        
       // Debug.Log(imageContent.fillAmount);
    }

    //handle translation between integer health values and float values

    private float Map(float value, float inMin, float inMax, float outMin, float outMax)
    {
        //Translate scale

        return (value - inMin) * (outMax - outMin) / (inMax -inMin) +outMin;

         
    }
}
