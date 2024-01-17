using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    Rigidbody2D rigid2D;
    Animator animator;  //���s�A�j���[�V�����̍Đ����x�ǉ��v���O����

    SpriteRenderer rendererC;
    AudioSource audioSource;

    float jumpForce = 250.0f;   
    float walkForce = 15.0f;    
    float maxWalkSpeed = 4.0f;

    bool isBallAttached = false;
    bool isChangedColor = false;
    private GameObject attachedBall;

    public GameObject blackDoorLocation;

    public Color playerColor;//�v���C���̐F

    public AudioClip MirrorSE;
    Color tmpColor;
    void Start()
    {
        Application.targetFrameRate = 60;
        this.rigid2D = GetComponent<Rigidbody2D>();
        this.animator = GetComponent<Animator>();  //���s�A�j���[�V�����̍Đ����x�ǉ��v���O����
        this.rendererC = GetComponent<SpriteRenderer>();
        this.audioSource = GetComponent<AudioSource>();
    }

    void Update()
    {
        //�W�����v����
        if (Input.GetKeyDown(KeyCode.Space)) //GetKeyDown���\�b�h���g���ăX�y�[�X�L�[�������ꂽ���𒲂ׂ�B
            {
                this.rigid2D.AddForce(transform.up * this.jumpForce);
            animator.SetTrigger("Jump");
        }

		//���E�Ɉړ�����
		int key = 0;
        if (Input.GetKey(KeyCode.RightArrow))
        {
            key = 1;
            animator.SetTrigger("Walk");
        }
        if (Input.GetKey(KeyCode.LeftArrow))
        {
            key = -1;
            animator.SetTrigger("Walk");
        }


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
        /*�O�̃v���O����

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


if (collision.CompareTag("Mirror"))
{
    if (!isChangedColor)
    {
        isChangedColor = true;
        if (playerColor == Color.white)
        {
            playerColor = Color.black;
        }
        else
        {
            playerColor = Color.white;
        }
        //�F��ύX
        rendererC.color = playerColor;
    }
}
*/
        //���:switch���ɂ����ق����悢�̂ł�?
        
        switch(collision.tag)
        {
            //�{�[����������A�{�[���̃I�u�W�F�N�g��ϐ��ɓ����
            case "Ball":
                attachedBall = collision.gameObject;
                break;

            //���h�A��������A���̎w�肳�ꂽ���h�A�̏ꏊ�Ɉړ�
            case "WhiteDoor":
                //�����
                break;

            //���h�A��������A���̎w�肳�ꂽ���h�A�̏ꏊ�Ɉړ�
            case "BlackDoor":
                transform.position = blackDoorLocation.transform.position;
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
                    //rendererC.color = playerColor;//�����ɐF��ύX
                    isChangedColor = true;
                    this.audioSource.PlayOneShot(MirrorSE);
                }
            break;

        }
    }
    /*�����ɐ؂�ւ��鎞����
    void OnTriggerExit2D(Collider2D collision)
    {
        //�g���K�[�����ɂ���
        if (collision.CompareTag("Mirror"))
        {
            isChangedColor = false;
        }
    }
    */

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

    //�F�؂�ւ��̃t�F�[�h����
    void ChangeColorProcess()
    {
        if (playerColor == Color.white)
        {
            tmpColor += new Color(1.0f, 1.0f, 1.0f, 1.0f) * Time.deltaTime;
            if (tmpColor.r >= playerColor.r)//r,g,b�͑S�ē����Ȃ���
            {
                rendererC.color = playerColor;
                isChangedColor = false;
                return;
            }
        }
        else
        {
            tmpColor -= new Color(1.0f, 1.0f, 1.0f) * Time.deltaTime;
            if (tmpColor.r <= playerColor.r)//r,g,b�͑S�ē����Ȃ���
            {
                 rendererC.color = playerColor;
                isChangedColor = false;
                return;
            }
        }
        rendererC.color = tmpColor;
        tmpColor.a = 1.0f;//�����x�͕ω������Ȃ�
    }
}

