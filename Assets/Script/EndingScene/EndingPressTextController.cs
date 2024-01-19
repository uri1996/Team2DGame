using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System;

public class EndingPressTextController : MonoBehaviour
{
    public GameObject PressText;
    public float AlphaChangingSpaad = 0.02f;

    private bool isWakeUp;
    private bool isExtinguishing;

    private float minValue = 0.4f;
    private float maxValue = 1.0f;
    private float frequency = 5.4f;//点滅スピード
    void Start()
    {
        isWakeUp = false;
        isExtinguishing = false;
    }

    void FixedUpdate()
    {
        if (isWakeUp == true)
        {
            if (AlphaChanging())//アルファ値を変更
            {
                isWakeUp = false;
                isExtinguishing = true;
            }
        }
        if (isExtinguishing == true)
        {
            AlphaExtinguishing();//点滅
        }
    }

    private bool AlphaChanging()//アルファ値を変更
    {
        PressText.GetComponent<TextMeshProUGUI>().color += new Color(0f, 0.0f, 0.0f, 0.02f);
  
        if (PressText.GetComponent<TextMeshProUGUI>().color.a >= 1.0f)
        {
            return true;
        }
        return false;
    }

    private void AlphaExtinguishing()//点滅
    {
        float sinValue = Mathf.Sin(Time.time * frequency);
        float result = Mathf.Lerp(minValue, maxValue, (sinValue + 1f) / 2f);
        PressText.GetComponent<TextMeshProUGUI>().color = new Color(1f, 1f, 1f, result);
    }

    private void OnEnable()
    {
        EndingSceneEvent.EndingListScrollOver += OnWakeUpText;//イベントにインストール
    }
    private void OnDisable()
    {
        EndingSceneEvent.EndingListScrollOver -= OnWakeUpText;
    }

    private void OnWakeUpText() //イベントが実行されたらこのメソッドが動く
    {
        isWakeUp = true;
    }
}
