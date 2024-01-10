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

    void Start()
    {
        Application.targetFrameRate = 60;
        this.rigid2D = GetComponent<Rigidbody2D>();
        this.animator = GetComponent<Animator>();  //���s�A�j���[�V�����̍Đ����x�ǉ��v���O����
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
    }

}
