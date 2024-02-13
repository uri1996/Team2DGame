using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.InputSystem;

//�S��:�{�열��
public class PlayerController : MonoBehaviour
{
    public Rigidbody2D rigid2D;
    Animator animator;  //���s�A�j���[�V�����̍Đ����x�ǉ��v���O����

    public SpriteRenderer rendererC;
    AudioSource audioSource;

    float jumpForce = 375.0f;
    float walkForce = 15.0f;
    float maxWalkSpeed = 2.0f;

    bool isBallAttached = false;
    bool isJump = false;
    bool isInvincible = false;
    public bool isAbleToMove = true;

    float targetAlpha = 1.0f;   //���̃A���t�@�Ɍ����Ė��t���[���v�Z����

    private GameObject attachedBall;

    //public GameObject blackDoor;
    //public GameObject whiteDoor;

    public Color playerColor;//�v���C���̐F

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
        this.animator = GetComponent<Animator>();  //���s�A�j���[�V�����̍Đ����x�ǉ��v���O����
        this.rendererC = GetComponent<SpriteRenderer>();
        this.audioSource = GetComponent<AudioSource>();
        tmpColor = playerColor;
        tmpColor.a = playerColor.a = 1.0f;   //playerColor�����܂ɃA���t�@0�ɂȂ�̂ŁA������Ȃ���
    }

    void Update()
    {
        //�v���C���[�̐F�Ń��C���[�𕪂���
        if (playerColor == Color.black)
        {
            gameObject.layer = 7;//�����C���[
        }
        else
        {
            gameObject.layer = 8;//�����C���[
        }
        if (isAbleToMove)
        {
            //�W�����v����
            if (!isJump)
            {
                if (Input.GetButtonDown("Jump"))
                {
                    this.rigid2D.AddForce(transform.up * this.jumpForce);
                    animator.SetBool("Jump", true);
                    isJump = true;
                }
            }

            //���E�Ɉړ�����
            float inputMove = Input.GetAxis("Horizontal");
            if (Mathf.Abs(inputMove) > 0.0f)
            {
                angle = (Angle)Mathf.Sign(inputMove);
                animator.SetTrigger("Walk");
            }


            //�v���C���̑��x
            float speedx = Mathf.Abs(this.rigid2D.velocity.x);

            if (inputMove != 0.0f)
            {
                //�X�s�[�h����
                if ((speedx < this.maxWalkSpeed))
                {
                    this.rigid2D.AddForce(transform.right * (int)angle * this.walkForce);
                }
            }

            //���������ɉ����Ĕ��]
            transform.localScale = new Vector3((int)angle, 1, 1);

            //�v���C���[�̑��x�ɉ����ăA�j���[�V�������x��ς���            
            string animName = animator.GetCurrentAnimatorClipInfo(0)[0].clip.name;
            if (animName.Contains("Walk"))
            {
                this.animator.speed = speedx / 2.0f; //���s�A�j���[�V�����̍Đ����x�ǉ��v���O����
            }
            else
            {
                this.animator.speed = 1.0f; //�W�����v���̍Đ����x�͈��
            }

            //�{�[���ƍ���/���􂷂�
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
            //�{�[����������A�{�[���̃I�u�W�F�N�g��ϐ��ɓ����
            case "Ball":
                attachedBall = collision.gameObject;
                break;

            //���h�A��������A���̎w�肳�ꂽ���h�A�̏ꏊ�Ɉړ�
            case "WhiteDoor":
                this.animator.speed = 0.0f;
                if (playerColor != Color.white) { break; }
                //�Ԃ������h�A�̃X�N���v�g���擾����
                DoorController doorScript = collision.gameObject.GetComponent<DoorController>();
                //�s��̃h�A�̏�������������
                if (doorScript == null || doorScript.nextDoor == null)
                {
                    //�s��̃h�A���Ȃ��̂ŁA����ȏ㏈�������Ȃ�
                    break;
                }

                Invoke("Transparentize", 1);
                doorScript.Invoke("MoveDoor", 2);
                isAbleToMove = false;
                collision.GetComponent<Animator>().SetTrigger("Open");
                collision.GetComponent<AudioSource>().Play();
                break;

            //���h�A��������A���̎w�肳�ꂽ���h�A�̏ꏊ�Ɉړ�
            case "BlackDoor":
                this.animator.speed = 0.0f;
                if (playerColor != Color.black) { break; }
                //�폜�\��
                //�Ԃ������h�A�̃X�N���v�g���擾����
                doorScript = collision.gameObject.GetComponent<DoorController>();
                //�s��̃h�A�̏�������������
                if (doorScript == null || doorScript.nextDoor == null)
                {
                    //�s��̃h�A���Ȃ��̂ŁA����ȏ㏈�������Ȃ�
                    break;
                }
                //�ȏ�
                Invoke("Transparentize", 1);
                doorScript.Invoke("MoveDoor", 2);
                isAbleToMove = false;
                collision.GetComponent<Animator>().SetTrigger("Open");
                collision.GetComponent<AudioSource>().Play();
                break;

            //���h�A�i�N���A�p�j��������
            case "WhiteClearDoor":
                this.animator.speed = 0.0f;
                if (playerColor == Color.white)
                {
                    Invoke("Transparentize", 1);
                    isAbleToMove = false;
                    //�h�A���J����
                    collision.GetComponent<Animator>().SetTrigger("Open");
                    collision.GetComponent<AudioSource>().Play();
                    //3�b��(�A�j���[�V�������I����Ă���)�Ɏ��̃V�[����
                    Invoke("ToNextScene", 3);
                }
                break;

            //���h�A�i�N���A�p�j��������
            case "BlackClearDoor":
                this.animator.speed = 0.0f;
                if (playerColor == Color.black)
                {
                    Invoke("Transparentize", 1);
                    isAbleToMove = false;
                    //�h�A���J����
                    collision.GetComponent<Animator>().SetTrigger("Open");
                    collision.GetComponent<AudioSource>().Play();
                    //3�b��(�A�j���[�V�������I����Ă���)�Ɏ��̃V�[����
                    Invoke("ToNextScene", 3);
                }
                break;

            case "Mirror":
                this.animator.speed = 0.0f;
                collision.GetComponent<MirrorController>().ChangeColor();//���̏���
                break;
            default://��
                isJump = false;
                animator.SetBool("Jump", false);
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

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Slime"))
        {
            this.audioSource.PlayOneShot(DeadSE);
            Transparentize();
            gameObject.layer = 7;//�����C���[
            Invoke("Retry", 2.0f);
        }
    }

    //�{�[���ƍ��̂���
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

    //�{�[���ƕ��􂷂�
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
        //float targetAlpha;   //���̃A���t�@�Ɍ����Ė��t���[���v�Z����

        if (rendererC.color.a < targetAlpha)
        {
            //�A���t�@���グ��@���@������
            tmpColor.a += 1.0f * Time.deltaTime;
            if (tmpColor.a >= 1.0f)
            {
                tmpColor.a = 1.0f;
            }
            rendererC.color = new Color(rendererC.color.r, rendererC.color.g, rendererC.color.b, tmpColor.a);
        }
        else if (rendererC.color.a > targetAlpha)
        {
            //�A���t�@��������@���@�������
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
        targetAlpha = 0.0f; //�ڕW�̃A���t�@�l��ݒ�
    }

    public void Appear()
    {
        //rendererC.color = new Color(rendererC.color.r, rendererC.color.g, rendererC.color.b, 1.0f);
        targetAlpha = 1.0f; //�ڕW�̃A���t�@�l��ݒ�
    }
}

