using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class PrismController : MonoBehaviour
{
    Vector3 defaultPos;
    Vector3 moveVec = Vector3.down;
    [SerializeField] float speed;
    // Start is called before the first frame update
    void Start()
    {
        defaultPos = transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        transform.position += moveVec * speed * Time.deltaTime;

        //éËÇÃï”ÇËÇâùïúÇ≥ÇπÇÈ
        if(transform.position.y <= defaultPos.y - 0.2f)
        {
            moveVec = Vector3.up;
        }
        else if (transform.position.y >= defaultPos.y)
        {
            moveVec = Vector3.down;
        }
    }
}
