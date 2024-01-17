using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    Rigidbody2D rigid2D;
    Animator animator;  //���s�A�j���[�V�����̍Đ����x�ǉ��v���O����
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
        this.animator = GetComponent<Animator>();  //���s�A�j���[�V�����̍Đ����x�ǉ��v���O����
        ball = GameObject.FindGameObjectWithTag("Ball").GetComponent<BallScript>();
    }

    void Update()
    {
        //�W�����v����
        if (Input.GetKeyDown(KeyCode.Space)) //GetKeyDown���\�b�h���g���ăX�y�[�X�L�[�������ꂽ���𒲂ׂ�B
            {
                this.rigid2D.AddForce(transform.up * this.jumpForce);
        }

		//���E�Ɉړ�����
		int key = 0;
		if (Input.GetKey(KeyCode.RightArrow)) key = 1;
		if (Input.GetKey(KeyCode.LeftArrow)) key = -1;


		//�v���C���̑��x
		float speedx = Mathf.Abs(this.rigid2D.velocity.x);

        //�X�s�[�h����
        //���E�������ꂼ��Ɉړ������̏����𕪂���
        if ((key > 0 && this.rigid2D.velocity.x < this.maxWalkSpeed) ||
            (key < 0 && this.rigid2D.velocity.x > -this.maxWalkSpeed))
        {
            this.rigid2D.AddForce(transform.right * key * this.walkForce);
        }

        //���������ɉ����Ĕ��]
        if (key !=0)
        {
            transform.localScale = new Vector3(key, 1, 1);
        }

        //�v���C���[�̑��x�ɉ����ăA�j���[�V�������x��ς���
        this.animator.speed = speedx / 2.0f;�@//���s�A�j���[�V�����̍Đ����x�ǉ��v���O����

        //�{�[���ƍ���/���􂷂�
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
        //�{�[����������A�{�[���̃I�u�W�F�N�g��ϐ��ɓ����
        if(collision.CompareTag("Ball"))
        {
            attachedBall = collision.gameObject;
        }

        //���h�A��������A���̎w�肳�ꂽ���h�A�̏ꏊ�Ɉړ�
        if(collision.CompareTag("WhiteDoor"))
        {

        }
        else if(collision.CompareTag("BlackDoor"))      //���h�A��������A���̎w�肳�ꂽ���h�A�̏ꏊ�Ɉړ�
        {
            transform.position = blackDoorLocation.transform.position;
        }

    }

    //�{�[���ƍ��̂���
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

    //�{�[���ƕ��􂷂�
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

