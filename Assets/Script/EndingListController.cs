using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EndingListController : MonoBehaviour
{
    Vector3 imgPostion = new Vector3(1.74f, -11.42f, 0f);
    private bool isMoving;

    // Start is called before the first frame update
    void Start()
    {
        isMoving = false;
        IniImg();
        StartCoroutine(ImgUpdate());
    }

    private void IniImg()
    {
        transform.position = imgPostion;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (isMoving)
        {
            ImgMovement();
        }
        
    }

    private void ImgMovement()
    {
        throw new NotImplementedException();
    }

    IEnumerator ImgUpdate()
    {
        while (true)
        {
            yield return new WaitForSecondsRealtime(0.5f);
            isMoving = true;

        }
    }
}
