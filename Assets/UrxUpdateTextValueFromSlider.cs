using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UniRx;
using System;

public class UrxUpdateTextValueFromSlider : MonoBehaviour
{
    [SerializeField]
    Slider MySlider;
    [SerializeField]
    Text MyText;

    void Start()
    {
        MySlider.OnValueChangedAsObservable()
                .SubscribeToText(MyText, x => Math.Round(x, 2).ToString());
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
