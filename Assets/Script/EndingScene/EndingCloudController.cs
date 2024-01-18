using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EndingCloudController : MonoBehaviour
{
    public GameObject Cloud0;
    public GameObject Cloud1;
    public GameObject Cloud2;

    private float Cloud0Speed = 0.002f;
    private float Cloud1Speed = 0.0014f;
    private float Cloud2Speed = 0.001f;
 
    void FixedUpdate()
    {
        CloudMovement();
    }

    private void CloudMovement()
    {
        Cloud0.transform.position += new Vector3(Cloud0Speed, 0f, 0f);
        Cloud1.transform.position += new Vector3(Cloud1Speed, 0f, 0f);
        Cloud2.transform.position += new Vector3(Cloud2Speed, 0f, 0f);
    }
}
