using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BallScript : MonoBehaviour
{
    private PlayerController player;
    private SpriteRenderer ballSprite;

    private void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>();
        ballSprite = GetComponent<SpriteRenderer>();
        ballSprite.color = Color.gray;
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        //ボールがスライムとぶつかったら、敵が破壊される
        if (collision.gameObject.CompareTag("Slime") && ballSprite.color == Color.black)
        {
            Destroy(collision.gameObject);
        }

        if (collision.gameObject.CompareTag("Player"))
        {
            ballSprite.color = player.playerColor;
            player.GetComponent<Animator>().SetBool("NewPush", true);
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if(collision.gameObject.CompareTag("Player"))
        {
            ballSprite.color = Color.gray;
            player.GetComponent<Animator>().SetBool("NewPush", false);
        }
    }
}
