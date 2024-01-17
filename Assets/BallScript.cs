using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BallScript : MonoBehaviour
{
    Color[] colorArray;
    int colorTimer;
    int colorDuration;

    SpriteRenderer spriteRenderer;
    public bool isPickedUP;

    int index;

    private void Start()
    {
        colorArray = new Color[] { Color.red, Color.yellow, Color.green, Color.blue };
        colorTimer = 0;
        colorDuration = 30;

        spriteRenderer = this.GetComponent<SpriteRenderer>();
        isPickedUP = false;

        index = 0;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        //ボールがスライムとぶつかったら、敵が破壊される
        if (collision.gameObject.CompareTag("Enemy"))
        {
            Destroy(collision.gameObject);
        }
    }

    private void Update()
    {
        if(isPickedUP) 
        {
            ChangeColor();
        }
        else
        {
            spriteRenderer.color = Color.blue;
        }
    }

    void ChangeColor()
    {
        if(colorTimer >= colorDuration)
        {
            spriteRenderer.color = colorArray[index];

            index++;
            colorTimer = 0;
        }
        if(index >= colorArray.Length)
        {
            index = 0;
        }

        colorTimer++;
    }
}
