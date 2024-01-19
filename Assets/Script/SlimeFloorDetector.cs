using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlimeFloorDetector : MonoBehaviour
{
    [SerializeField] GameObject slime;
    SlimeController controller;
    bool isCollision = true;
    // Start is called before the first frame update
    void Start()
    {
        controller = slime.GetComponent<SlimeController>();
    }

    void Update()
    {
        if (!isCollision)
        {
            //controller.SetAngle(-controller.GetAngle());
            //transform.localScale = new Vector3((float)controller.GetAngle(), 1.0f, 1.0f);//êeÇ™îΩì]Ç∑ÇÈÇ©ÇÁÇ¢ÇÁÇ»Ç¢
        }
    }
    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("TileMap"))
        {
            isCollision = true;
        }
    }

    void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("TileMap"))
        {
            isCollision = false;
        }
    }
}
