using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;


//Since stats class does does inherit from monobehavior, it will not appear in inpector 
//unless we make it serializable

[Serializable]
public class Stats
{
    [SerializeField]
    private BarScript bar;

    [SerializeField]
    private float maxVal;

    [SerializeField]
    private float currentValue;

    

    public float CurrentValue
    {
        get => currentValue;
        set
        {
           //Use clamp function to clamp value between 0 and 100
            this.currentValue = Mathf.Clamp(value, 0, MaxVal);
            bar.Value = currentValue;
        }
    }

    public float MaxVal
    {
        get => maxVal;

        set
        {
            this.maxVal = value;
            bar.MaxValue = maxVal;
        }
    }


    public void InitializeMaxValue()
    {
        this.MaxVal = maxVal;
        this.CurrentValue = currentValue;
    }
}
