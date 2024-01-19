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

    float jumpForce = 375.0f;
    float walkForce = 15.0f;
    float maxWalkSpeed = 4.0f;

    bool isBallAttached = false;
    bool isChangedColor = false;
    float targetAlpha = 1.0f;   //このアルファに向けて毎フレーム計算する
    public bool isAbleToMove = true;
    private GameObject attachedBall;

    public GameObject blackDoor;
    public GameObject whiteDoor;

    public Color playerColor;//プレイヤの色

    public AudioClip MirrorSE;
    public AudioClip DeadSE;
    Color tmpColor;
    void Start()
    {
        Application.targetFrameRate = 60;
        this.rigid2D = GetComponent<Rigidbody2D>();
        this.animator = GetComponent<Animator>();  //歩行アニメーションの再生速度追加プログラム
        this.rendererC = GetComponent<SpriteRenderer>();
        this.audioSource = GetComponent<AudioSource>();
        tmpColor = playerColor;
        tmpColor.a = playerColor.a = 1.0f;   //playerColorがたまにアルファ0になるので、代入しなおす
    }

    void Update()
    {
        if (playerColor == Color.black)
        {
            gameObject.layer = 7;//黒レイヤー
        }
        else
        {
            gameObject.layer = 8;//白レイヤー
        }
        if (isAbleToMove)
        {
            Debug.Log(gameObject.layer);

            Gamepad gamepad = Gamepad.current;
            //ジャンプする
            if (Input.GetKeyDown(KeyCode.Space) ||
                gamepad.buttonSouth.wasPressedThisFrame) //GetKeyDownメソッドを使ってスペースキーが押されたかを調べる。
            {
                this.rigid2D.AddForce(transform.up * this.jumpForce);
                animator.SetTrigger("Jump");
            }

            //左右に移動する
            int key = 0;
            float horizontalInput = gamepad.leftStick.x.ReadValue();
            if (Input.GetKey(KeyCode.RightArrow) || horizontalInput > 0.5)
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
            if (key != 0)
            {
                transform.localScale = new Vector3(key, 1, 1);
            }

            //プレイヤーの速度に応じてアニメーション速度を変える            
            string anim_name = animator.GetCurrentAnimatorClipInfo(0)[0].clip.name;
            if (anim_name.Contains("Walk"))
            {
                this.animator.speed = speedx / 2.0f; //歩行アニメーションの再生速度追加プログラム
            }
            else
            {
                this.animator.speed = 1.0f; //ジャンプ等の再生速度は一定
            }

            //ボールと合体/分裂する
            if (Input.GetKeyDown(KeyCode.F) ||
                gamepad.buttonEast.wasPressedThisFrame)
            {
                if (isBallAttached)
                {
                    DetachBall();
                }
                else
                {
                    if (!isBallAttached && attachedBall != null)
                    {
                        AttachBall(attachedBall);
                        animator.SetTrigger("Push");
                    }
                }
            }
        }
        else
        {
            //this.rigid2D.isKinematic = true;
            this.rigid2D.velocity = new Vector3(0.0f, this.rigid2D.velocity.y, 0.0f);
        }

        if (isChangedColor)
        {
            ChangeColorProcess();
        }
        ChangeAlphaProcess();
    }
    void OnTriggerEnter2D(Collider2D collision)
    {
        switch (collision.tag)
        {
            //ボールだったら、ボールのオブジェクトを変数に入れる
            case "Ball":
                attachedBall = collision.gameObject;
                break;

            //白ドアだったら、次の指定された白ドアの場所に移動
            case "WhiteDoor":
                if (playerColor == Color.white)
                {
                    //ぶつかったドアのスクリプトを取得する
                    DoorController doorScript = collision.gameObject.GetComponent<DoorController>();
                    //行先のドアの情報を書き換える
                    if (doorScript != null && doorScript.nextDoor != null)
                    {
                        //行先のドアがある
                        whiteDoor = doorScript.nextDoor;
                    }
                    else
                    {
                        //行先のドアがないので、これ以上処理をしない
                        break;
                    }

                    Invoke("Transparentize", 1);
                    Invoke("MoveWhiteDoor", 2);
                    isAbleToMove = false;
                    collision.GetComponent<Animator>().SetTrigger("Open");
                    collision.GetComponent<AudioSource>().Play();
                }
                break;

            //黒ドアだったら、次の指定された黒ドアの場所に移動
            case "BlackDoor":
                if (playerColor == Color.black)
                {
                    //ぶつかったドアのスクリプトを取得する
                    DoorController doorScript = collision.gameObject.GetComponent<DoorController>();
                    //行先のドアの情報を書き換える
                    if (doorScript != null && doorScript.nextDoor != null)
                    {
                        //行先のドアがある
                        blackDoor = doorScript.nextDoor;
                    }
                    else
                    {
                        //行先のドアがないので、これ以上処理をしない
                        break;
                    }

                    Invoke("Transparentize", 1);
                    Invoke("MoveBlackDoor", 2);
                    isAbleToMove = false;
                    collision.GetComponent<Animator>().SetTrigger("Open");
                    collision.GetComponent<AudioSource>().Play();
                }
                break;

            //白ドア（クリア用）だったら
            case "WhiteClearDoor":
                if (playerColor == Color.white)
                {
                    Invoke("Transparentize", 1);
                    isAbleToMove = false;
                    //ドアを開ける
                    collision.GetComponent<Animator>().SetTrigger("Open");
                    collision.GetComponent<AudioSource>().Play();
                    //3秒後(アニメーションが終わってから)に次のシーンへ
                    Invoke("ToNextScene", 3);
                }
                break;

            //黒ドア（クリア用）だったら
            case "BlackClearDoor":
                if (playerColor == Color.black)
                {
                    Invoke("Transparentize", 1);
                    isAbleToMove = false;
                    //ドアを開ける
                    collision.GetComponent<Animator>().SetTrigger("Open");
                    collision.GetComponent<AudioSource>().Play();
                    //3秒後(アニメーションが終わってから)に次のシーンへ
                    Invoke("ToNextScene", 3);
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
                    isAbleToMove = false;
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

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Slime"))
        {
            this.audioSource.PlayOneShot(DeadSE);
            Transparentize();
            Invoke("Retry", 2);
        }
    }

    //ボールと合体する
    void AttachBall(GameObject ball)
    {
        if (!isBallAttached)
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
        if (isBallAttached)
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
            tmpColor += new Color(1.0f, 1.0f, 1.0f) * Time.deltaTime;
            if (tmpColor.r >= playerColor.r)//r,g,bは全て同じなため
            {
                rendererC.color = playerColor;
                this.rigid2D.isKinematic = false;
                isChangedColor = false;
                isAbleToMove = true;
                return;
            }
        }
        else
        {
            tmpColor -= new Color(1.0f, 1.0f, 1.0f) * Time.deltaTime;
            if (tmpColor.r <= playerColor.r)//r,g,bは全て同じなため
            {
                rendererC.color = playerColor;
                this.rigid2D.isKinematic = false;
                isChangedColor = false;
                isAbleToMove = true;
                return;
            }
        }
        rendererC.color = tmpColor;
        tmpColor.a = 1.0f;//透明度は変化させない
    }
    void ChangeAlphaProcess()
    {
        //float targetAlpha;   //このアルファに向けて毎フレーム計算する

        if (rendererC.color.a < targetAlpha)
        {
            //アルファを上げる　＝　現れる方
            tmpColor.a += 1.0f * Time.deltaTime;
            if (tmpColor.a >= 1.0f)
            {
                tmpColor.a = 1.0f;
            }
            rendererC.color = new Color(rendererC.color.r, rendererC.color.g, rendererC.color.b, tmpColor.a);
        }
        else if(rendererC.color.a > targetAlpha)
        {
            //アルファを下げる　＝　消える方
            tmpColor.a -= 2.0f * Time.deltaTime;
            if (tmpColor.a <= 0.0f)
            {
                tmpColor.a = 0.0f;
            }
            rendererC.color = new Color(rendererC.color.r, rendererC.color.g, rendererC.color.b, tmpColor.a);
        }
    }

    void MoveBlackDoor()
    {
        Appear();
        transform.position = blackDoor.transform.position;
        blackDoor.GetComponent<Animator>().SetTrigger("Open");
        blackDoor.GetComponent<AudioSource>().Play();
        isAbleToMove = true;
    }
    void MoveWhiteDoor()
    {
        Appear();
        transform.position = whiteDoor.transform.position;
        whiteDoor.GetComponent<Animator>().SetTrigger("Open");
        whiteDoor.GetComponent<AudioSource>().Play();
        isAbleToMove = true;
    }

    void ToNextScene()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }
    void Retry()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
    void Transparentize()
    {
        //rendererC.color = new Color(rendererC.color.r, rendererC.color.g, rendererC.color.b, 0.0f);
        targetAlpha = 0.0f; //目標のアルファ値を設定
    }

    void Appear()
    {
        //rendererC.color = new Color(rendererC.color.r, rendererC.color.g, rendererC.color.b, 1.0f);
        targetAlpha = 1.0f; //目標のアルファ値を設定
    }
}

