using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PressAButtonTextController : MonoBehaviour
{
    public float AlphaChangingSpaad = 0.02f;

    private bool isExtinguishing;

    private float minValue = 0.4f;
    private float maxValue = 1.0f;
    private float frequency = 5.4f;//�_�ŃX�s�[�h

    void Start()
    {
        isExtinguishing = false;
    }

    void FixedUpdate()
    {
        if (AlphaChanging())//�A���t�@�l��ύX
        {
            isExtinguishing = true;
        }
        if (isExtinguishing == true)
        {
            AlphaExtinguishing();//�_��
        }
    }

    private bool AlphaChanging()//�A���t�@�l��ύX
    {
        GetComponent<SpriteRenderer>().color += new Color(0f, 0.0f, 0.0f, 0.02f);

        if (GetComponent<SpriteRenderer>().color.a >= 1.0f)
        {
            return true;
        }
        return false;
    }

    private void AlphaExtinguishing()//�_��
    {
        float sinValue = Mathf.Sin(Time.time * frequency);
        float result = Mathf.Lerp(minValue, maxValue, (sinValue + 1f) / 2f);
        GetComponent<SpriteRenderer>().color = new Color(1f, 1f, 1f, result);
    }
}
