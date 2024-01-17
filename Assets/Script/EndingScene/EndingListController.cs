using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class EndingListController : MonoBehaviour
{
    /// <summary>
    /// EndingListがスクロール終わるかどうか
    /// </summary>
    public bool IsEndingListScrollOver { get; set; }

    [Range(0.0f, 0.18f)] public float Speed = 0.02f;
    [Range(0.0f, 0.18f)] public float ButtonIsPressedSpeed = 0.1f;

    private Vector2 imgPostion = new Vector2(1.74f, -11.42f);
    private bool isMoving;

    void Start()
    {
        isMoving = false;
        IsEndingListScrollOver = false;
        IniImg();
        StartCoroutine(ImgUpdate());
    }
    IEnumerator ImgUpdate()
    {
        while (true)
        {
            //エンディング画面に入ると、何秒後に制作者リストがスクロール始めるか
            yield return new WaitForSecondsRealtime(0.5f);
            isMoving = true;
            yield break;
        }
    }

    private void IniImg()
    {
        transform.position = imgPostion;
    }

    void FixedUpdate()
    {
        if (isMoving)
        {
            ImgMovement();
        }
    }

    private void ImgMovement()
    {
        float value = Speed;
        Gamepad gamepad = Gamepad.current;
        if (gamepad != null)
        {
            if (gamepad.buttonEast.isPressed)//ボタンが押されたらスクロールスピードアップ
            {
                value = ButtonIsPressedSpeed;
            }
            else
            {
                value = Speed;
            }
        }
        transform.position += new Vector3(0f, value, 0f);

        if (transform.position.y > -529.0f)//EndingListがスクロール終わる
        {
            IsEndingListScrollOver=true;
        }
    }

}
