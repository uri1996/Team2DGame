using System.Collections;
using System.Collections.Generic;
using UnityEditorInternal;
using UnityEngine;

public class MirrorController : MonoBehaviour
{
    bool isChangedColor = false;
    PlayerController player;
    AudioSource audioSource;
    [SerializeField] AudioClip MirrorSE;
    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.Find("player").GetComponent<PlayerController>();
        audioSource = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        if (isChangedColor)
        {
            //ChangeColorProcess();
        }
    }

    public void ChangeColor()
    {
        if (!isChangedColor)
        {
            if (player.playerColor == Color.white)
            {
                player.rendererC.material.SetFloat("_Threshold", 0.93f);
                player.rendererC.color = player.playerColor = Color.black;
            }
            else
            {
                player.rendererC.material.SetFloat("_Threshold", 0.0f);
                player.rendererC.color = player.playerColor = Color.white;
            }

            audioSource.PlayOneShot(MirrorSE);
        }
    }

    //色切り替えのフェード処理
    //void ChangeColorProcess()
    //{
    //    if (player.playerColor == Color.white)
    //    {
    //        player.tmpColor += new Color(1.0f, 1.0f, 1.0f) * Time.deltaTime;
    //        if (player.tmpColor.r >= player.playerColor.r)//r,g,bは全て同じなため
    //        {
    //            player.rendererC.color = player.playerColor;
    //            player.rigid2D.isKinematic = false;
    //            isChangedColor = false;
    //            player.isAbleToMove = true;
    //            return;
    //        }
    //    }
    //    else
    //    {
    //        player.tmpColor -= new Color(1.0f, 1.0f, 1.0f) * Time.deltaTime;
    //        if (player.tmpColor.r <= player.playerColor.r)//r,g,bは全て同じなため
    //        {
    //            player.rendererC.color = player.playerColor;
    //            player.rigid2D.isKinematic = false;
    //            isChangedColor = false;
    //            player.isAbleToMove = true;
    //            return;
    //        }
    //    }
    //    player.rendererC.color = player.tmpColor;
    //    player.tmpColor.a = 1.0f;//透明度は変化させない
    //}
}
