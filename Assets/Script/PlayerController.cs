using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    Rigidbody2D rigid2D;
    Animator animator;  //歩行アニメーションの再生速度追加プログラム

    SpriteRenderer rendererC;
    AudioSource audioSource;

    float jumpForce = 250.0f;   
    float walkForce = 15.0f;    
    float maxWalkSpeed = 4.0f;

    bool isBallAttached = false;
    bool isChangedColor = false;
    private GameObject attachedBall;

    public GameObject blackDoor;
    public GameObject whiteDoor;

    public Color playerColor;//プレイヤの色

    public AudioClip MirrorSE;
    Color tmpColor;
    void Start()
    {
        Application.targetFrameRate = 60;
        this.rigid2D = GetComponent<Rigidbody2D>();
        this.animator = GetComponent<Animator>();  //歩行アニメーションの再生速度追加プログラム
        this.rendererC = GetComponent<SpriteRenderer>();
        this.audioSource = GetComponent<AudioSource>();
        tmpColor = playerColor;
    }

    void Update()
    {
        Gamepad gamepad = Gamepad.current;
        //ジャンプする
        if (Input.GetKeyDown(KeyCode.Space)||
            gamepad.buttonSouth.wasPressedThisFrame) //GetKeyDownメソッドを使ってスペースキーが押されたかを調べる。
            {
                this.rigid2D.AddForce(transform.up * this.jumpForce);
            animator.SetTrigger("Jump");
        }

		//左右に移動する
		int key = 0;
        float horizontalInput = gamepad.leftStick.x.ReadValue();
        if (Input.GetKey(KeyCode.RightArrow) || horizontalInput> 0.5)
        {
            key = 1;
            animator.SetTrigger("Walk");
        }
        if (Input.GetKey(KeyCode.LeftArrow) || horizontalInput < -0.5)
        {
            key = -1;
            animator.SetTrigger("Walk");
        }


            //プレイヤの速度
            float speedx = Mathf.Abs(this.rigid2D.velocity.x);

        //スピード制限
        //左右方向それぞれに移動制限の条件を分ける
        if ((key > 0 && this.rigid2D.velocity.x < this.maxWalkSpeed) ||
            (key < 0 && this.rigid2D.velocity.x > -this.maxWalkSpeed))
        {
            this.rigid2D.AddForce(transform.right * key * this.walkForce);
        }

        //動く方向に応じて反転
        if (key !=0)
        {
            transform.localScale = new Vector3(key, 1, 1);
        }

        //プレイヤーの速度に応じてアニメーション速度を変える
        this.animator.speed = speedx / 2.0f;　//歩行アニメーションの再生速度追加プログラム

        //ボールと合体/分裂する
        if(Input.GetKeyDown(KeyCode.F) ||
            gamepad.buttonEast.wasPressedThisFrame)
        {
            if (isBallAttached)
            {
                DetachBall();
            }
            else
            {
                if(!isBallAttached && attachedBall != null)
                {
                    AttachBall(attachedBall);
                    animator.SetTrigger("Push");
                }
            }
        }
        if (isChangedColor)
        {
            ChangeColorProcess();
        }
    }
    void OnTriggerEnter2D(Collider2D collision)
    {   
        switch(collision.tag)
        {
            //ボールだったら、ボールのオブジェクトを変数に入れる
            case "Ball":
                attachedBall = collision.gameObject;
                break;

            //白ドアだったら、次の指定された白ドアの場所に移動
            case "WhiteDoor":
                if (playerColor == Color.white)
                {
                    transform.position = whiteDoor.transform.position;
                }
                    break;

            //黒ドアだったら、次の指定された黒ドアの場所に移動
            case "BlackDoor":
                if (playerColor == Color.black)
                    transform.position = blackDoor.transform.position;
                break;

            //白ドア（クリア用）だったら、～～～～
            case "WhiteClearDoor":
                if (playerColor == Color.white)
                {
                    //「仮の処理」…現在のシーンの登録番号の次のシーンへ飛ぶ
                    SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
                }
                break;

            //黒ドア（クリア用）だったら、～～～～
            case "BlackClearDoor":
                if (playerColor == Color.black)
                {
                    //「仮の処理」…現在のシーンの登録番号の次のシーンへ飛ぶ
                    SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
                }
                break;

            case "Mirror":
                if (!isChangedColor)
                {
                    if (playerColor == Color.white)
                    {
                        playerColor = Color.black;
                    }
                    else
                    {
                        playerColor = Color.white;
                    }
                    //rendererC.color = playerColor;//即座に色を変更
                    isChangedColor = true;
                    this.audioSource.PlayOneShot(MirrorSE);
                }
            break;

        }
    }
    /*即座に切り替える時限定
    void OnTriggerExit2D(Collider2D collision)
    {
        //トリガー方式にする
        if (collision.CompareTag("Mirror"))
        {
            isChangedColor = false;
        }
    }
    */

    //ボールと合体する
    void AttachBall(GameObject ball)
    {
        if(!isBallAttached)
        {
            isBallAttached = true;
            attachedBall = ball;
            attachedBall.transform.parent = transform;
            attachedBall.GetComponent<Rigidbody2D>().isKinematic = true;
        }
    }

    //ボールと分裂する
    void DetachBall()
    {
        if(isBallAttached)
        {
            isBallAttached = false;
            attachedBall.transform.parent = null;
            attachedBall.GetComponent<Rigidbody2D>().isKinematic = false;
            attachedBall = null;
        }
    }

    //色切り替えのフェード処理
    void ChangeColorProcess()
    {
        if (playerColor == Color.white)
        {
            tmpColor += new Color(1.0f, 1.0f, 1.0f, 1.0f) * Time.deltaTime;
            if (tmpColor.r >= playerColor.r)//r,g,bは全て同じなため
            {
                rendererC.color = playerColor;
                isChangedColor = false;
                return;
            }
        }
        else
        {
            tmpColor -= new Color(1.0f, 1.0f, 1.0f) * Time.deltaTime;
            if (tmpColor.r <= playerColor.r)//r,g,bは全て同じなため
            {
                 rendererC.color = playerColor;
                isChangedColor = false;
                return;
            }
        }
        rendererC.color = tmpColor;
        tmpColor.a = 1.0f;//透明度は変化させない
    }
}

