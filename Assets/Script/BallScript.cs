using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BallScript : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        //�{�[�����X���C���ƂԂ�������A�G���j�󂳂��
        if (collision.gameObject.CompareTag("Enemy"))
        {
            Destroy(collision.gameObject);
        }
    }

    private void Update()
    {
        //�J���[���Q�[�~���O��������
        //GetComponent<SpriteRenderer>().color = Color.HSVToRGB((Time.time * 0.5f) % 1, 1, 1);
    }
}
