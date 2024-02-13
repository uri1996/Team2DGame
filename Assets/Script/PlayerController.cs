using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.InputSystem;

//担当:宮川龍希
public class PlayerController : MonoBehaviour
{
    public Rigidbody2D rigid2D;
    Animator animator;  //歩行アニメーションの再生速度追加プログラム

    public SpriteRenderer rendererC;
    AudioSource audioSource;

    float jumpForce = 375.0f;
    float walkForce = 15.0f;
    float maxWalkSpeed = 2.0f;

    bool isBallAttached = false;
    bool isJump = false;
    bool isInvincible = false;
    public bool isAbleToMove = true;

    float targetAlpha = 1.0f;   //このアルファに向けて毎フレーム計算する

    private GameObject attachedBall;

    //public GameObject blackDoor;
    //public GameObject whiteDoor;

    public Color playerColor;//プレイヤの色

    public AudioClip DeadSE;

    enum Angle
    {
        Right = 1, Left = -1,
    }

    Angle angle = Angle.Right;
    public Color tmpColor;

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
        //プレイヤーの色でレイヤーを分ける
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
            //ジャンプする
            if (!isJump)
            {
                if (Input.GetButtonDown("Jump"))
                {
                    this.rigid2D.AddForce(transform.up * this.jumpForce);
                    animator.SetBool("Jump", true);
                    isJump = true;
                }
            }

            //左右に移動する
            float inputMove = Input.GetAxis("Horizontal");
            if (Mathf.Abs(inputMove) > 0.0f)
            {
                angle = (Angle)Mathf.Sign(inputMove);
                animator.SetTrigger("Walk");
            }


            //プレイヤの速度
            float speedx = Mathf.Abs(this.rigid2D.velocity.x);

            if (inputMove != 0.0f)
            {
                //スピード制限
                if ((speedx < this.maxWalkSpeed))
                {
                    this.rigid2D.AddForce(transform.right * (int)angle * this.walkForce);
                }
            }

            //動く方向に応じて反転
            transform.localScale = new Vector3((int)angle, 1, 1);

            //プレイヤーの速度に応じてアニメーション速度を変える            
            string animName = animator.GetCurrentAnimatorClipInfo(0)[0].clip.name;
            if (animName.Contains("Walk"))
            {
                this.animator.speed = speedx / 2.0f; //歩行アニメーションの再生速度追加プログラム
            }
            else
            {
                this.animator.speed = 1.0f; //ジャンプ等の再生速度は一定
            }

            //ボールと合体/分裂する
            if (Input.GetButtonDown("Ball"))
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
                this.animator.speed = 0.0f;
                if (playerColor != Color.white) { break; }
                //ぶつかったドアのスクリプトを取得する
                DoorController doorScript = collision.gameObject.GetComponent<DoorController>();
                //行先のドアの情報を書き換える
                if (doorScript == null || doorScript.nextDoor == null)
                {
                    //行先のドアがないので、これ以上処理をしない
                    break;
                }

                Invoke("Transparentize", 1);
                doorScript.Invoke("MoveDoor", 2);
                isAbleToMove = false;
                collision.GetComponent<Animator>().SetTrigger("Open");
                collision.GetComponent<AudioSource>().Play();
                break;

            //黒ドアだったら、次の指定された黒ドアの場所に移動
            case "BlackDoor":
                this.animator.speed = 0.0f;
                if (playerColor != Color.black) { break; }
                //削除予定
                //ぶつかったドアのスクリプトを取得する
                doorScript = collision.gameObject.GetComponent<DoorController>();
                //行先のドアの情報を書き換える
                if (doorScript == null || doorScript.nextDoor == null)
                {
                    //行先のドアがないので、これ以上処理をしない
                    break;
                }
                //以上
                Invoke("Transparentize", 1);
                doorScript.Invoke("MoveDoor", 2);
                isAbleToMove = false;
                collision.GetComponent<Animator>().SetTrigger("Open");
                collision.GetComponent<AudioSource>().Play();
                break;

            //白ドア（クリア用）だったら
            case "WhiteClearDoor":
                this.animator.speed = 0.0f;
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
                this.animator.speed = 0.0f;
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
                this.animator.speed = 0.0f;
                collision.GetComponent<MirrorController>().ChangeColor();//鏡の処理
                break;
            default://床
                isJump = false;
                animator.SetBool("Jump", false);
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
            gameObject.layer = 7;//黒レイヤー
            Invoke("Retry", 2.0f);
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
        else if (rendererC.color.a > targetAlpha)
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

    void ToNextScene()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }
    void Retry()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
    public void Transparentize()
    {
        //rendererC.color = new Color(rendererC.color.r, rendererC.color.g, rendererC.color.b, 0.0f);
        targetAlpha = 0.0f; //目標のアルファ値を設定
    }

    public void Appear()
    {
        //rendererC.color = new Color(rendererC.color.r, rendererC.color.g, rendererC.color.b, 1.0f);
        targetAlpha = 1.0f; //目標のアルファ値を設定
    }
}

