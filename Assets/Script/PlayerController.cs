using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    Rigidbody2D rigid2D;
    Animator animator;  //���s�A�j���[�V�����̍Đ����x�ǉ��v���O����

    SpriteRenderer rendererC;
    AudioSource audioSource;

    float jumpForce = 375.0f;
    float walkForce = 15.0f;
    float maxWalkSpeed = 4.0f;

    bool isBallAttached = false;
    bool isChangedColor = false;
    public bool isAbleToMove = true;
    private GameObject attachedBall;

    public GameObject blackDoor;
    public GameObject whiteDoor;

    public Color playerColor;//�v���C���̐F

    public AudioClip MirrorSE;
    public AudioClip DeadSE;
    Color tmpColor;
    void Start()
    {
        Application.targetFrameRate = 60;
        this.rigid2D = GetComponent<Rigidbody2D>();
        this.animator = GetComponent<Animator>();  //���s�A�j���[�V�����̍Đ����x�ǉ��v���O����
        this.rendererC = GetComponent<SpriteRenderer>();
        this.audioSource = GetComponent<AudioSource>();
        tmpColor = playerColor;
    }

    void Update()
    {
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
            Debug.Log(gameObject.layer);

            Gamepad gamepad = Gamepad.current;
            //�W�����v����
            if (Input.GetKeyDown(KeyCode.Space) ||
                gamepad.buttonSouth.wasPressedThisFrame) //GetKeyDown���\�b�h���g���ăX�y�[�X�L�[�������ꂽ���𒲂ׂ�B
            {
                this.rigid2D.AddForce(transform.up * this.jumpForce);
                animator.SetTrigger("Jump");
            }

            //���E�Ɉړ�����
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
            if (key != 0)
            {
                transform.localScale = new Vector3(key, 1, 1);
            }

            //�v���C���[�̑��x�ɉ����ăA�j���[�V�������x��ς���
            this.animator.speed = speedx / 2.0f; //���s�A�j���[�V�����̍Đ����x�ǉ��v���O����

            //�{�[���ƍ���/���􂷂�
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
                if (playerColor == Color.white && collision.gameObject != whiteDoor.gameObject)
                {
                    Invoke("Transparentize", 1);
                    Invoke("MoveWhiteDoor", 2);
                    isAbleToMove = false;
                    collision.GetComponent<Animator>().SetTrigger("Open");
                    collision.GetComponent<AudioSource>().Play();
                }
                break;

            //���h�A��������A���̎w�肳�ꂽ���h�A�̏ꏊ�Ɉړ�
            case "BlackDoor":
                if (playerColor == Color.black && collision.gameObject != blackDoor.gameObject)
                {
                    Invoke("Transparentize", 1);
                    Invoke("MoveBlackDoor", 2);
                    isAbleToMove = false;
                    collision.GetComponent<Animator>().SetTrigger("Open");
                    collision.GetComponent<AudioSource>().Play();
                }
                break;

            //���h�A�i�N���A�p�j��������
            case "WhiteClearDoor":
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
                    isAbleToMove = false;
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

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Slime"))
        {
            this.audioSource.PlayOneShot(DeadSE);
            Transparentize();
            Invoke("Retry", 2);
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

    //�F�؂�ւ��̃t�F�[�h����
    void ChangeColorProcess()
    {
        if (playerColor == Color.white)
        {
            tmpColor += new Color(1.0f, 1.0f, 1.0f) * Time.deltaTime;
            if (tmpColor.r >= playerColor.r)//r,g,b�͑S�ē����Ȃ���
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
            if (tmpColor.r <= playerColor.r)//r,g,b�͑S�ē����Ȃ���
            {
                rendererC.color = playerColor;
                this.rigid2D.isKinematic = false;
                isChangedColor = false;
                isAbleToMove = true;
                return;
            }
        }
        rendererC.color = tmpColor;
        tmpColor.a = 1.0f;//�����x�͕ω������Ȃ�
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
        rendererC.color = new Color(rendererC.color.r, rendererC.color.g, rendererC.color.b, 0.0f);
    }

    void Appear()
    {
        rendererC.color = new Color(rendererC.color.r, rendererC.color.g, rendererC.color.b, 1.0f);
    }
}

