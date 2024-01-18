using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlimeDetector : MonoBehaviour
{
    [SerializeField] GameObject slime;
    SlimeController controller;
    // Start is called before the first frame update
    void Start()
    {
        controller = slime.GetComponent<SlimeController>();
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("TileMap"))
        {
            controller.SetAngle(-controller.GetAngle());
            //transform.localScale = new Vector3((float)controller.GetAngle(), 1.0f, 1.0f);//êeÇ™îΩì]Ç∑ÇÈÇ©ÇÁÇ¢ÇÁÇ»Ç¢
        }
    }
}
