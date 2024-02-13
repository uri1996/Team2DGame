using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class EndingListController : MonoBehaviour
{
    [Range(0.0f, 0.18f)] public float Speed = 0.02f;
    [Range(0.0f, 0.18f)] public float ButtonIsPressedSpeed = 0.2f;

    private Vector2 imgPostion = new Vector2(1.74f, -11.42f);
    private bool isMoving;
 

    void Start()
    {
        isMoving = false;
        IniImg();
        StartCoroutine(ImgUpdate());
    }
    IEnumerator ImgUpdate()
    {
        while (true)
        {
            yield return new WaitForSecondsRealtime(0.5f);//�G���f�B���O��ʂɓ����0.5�b�҂�
            isMoving = true;
            yield return new WaitUntil(() => transform.position.y > 8f);//EndingList���X�N���[���I���
            EndingSceneEvent.CallEndingListScrollOver();//�C�x���h�𓮂���
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
            if (gamepad.buttonEast.isPressed)//�{�^���������ꂽ��X�N���[���X�s�[�h�A�b�v
            {
                value = ButtonIsPressedSpeed;
            }
            else
            {
                value = Speed;
            }
        }
        else { Debug.Log("Gamepad is Null"); }
        transform.position += new Vector3(0f, value, 0f);
    }
}
