using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    Rigidbody2D rigid2D;
    Animator animator;  //歩行アニメーションの再生速度追加プログラム
    float jumpForce = 300.0f;   
    float walkForce = 30.0f;    
    float maxWalkSpeed = 4.0f;

    bool isBallAttached = false;
    private GameObject attachedBall;

    public GameObject blackDoorLocation;

    private BallScript ball;

    void Start()
    {
        Application.targetFrameRate = 60;
        this.rigid2D = GetComponent<Rigidbody2D>();
        this.animator = GetComponent<Animator>();  //歩行アニメーションの再生速度追加プログラム
        ball = GameObject.FindGameObjectWithTag("Ball").GetComponent<BallScript>();
    }

    void Update()
    {
        //ジャンプする
        if (Input.GetKeyDown(KeyCode.Space)) //GetKeyDownメソッドを使ってスペースキーが押されたかを調べる。
            {
                this.rigid2D.AddForce(transform.up * this.jumpForce);
        }

		//左右に移動する
		int key = 0;
		if (Input.GetKey(KeyCode.RightArrow)) key = 1;
		if (Input.GetKey(KeyCode.LeftArrow)) key = -1;


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
        if(Input.GetKeyDown(KeyCode.F))
        {
            if (isBallAttached)
            {
                DetachBall();
                ball.isPickedUP = false;
            }
            else
            {
                if(!isBallAttached && attachedBall != null)
                {
                    AttachBall(attachedBall);
                    ball.isPickedUP = true;
                }
            }

        }
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        //ボールだったら、ボールのオブジェクトを変数に入れる
        if(collision.CompareTag("Ball"))
        {
            attachedBall = collision.gameObject;
        }

        //白ドアだったら、次の指定された白ドアの場所に移動
        if(collision.CompareTag("WhiteDoor"))
        {

        }
        else if(collision.CompareTag("BlackDoor"))      //黒ドアだったら、次の指定された黒ドアの場所に移動
        {
            transform.position = blackDoorLocation.transform.position;
        }

    }

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
}

