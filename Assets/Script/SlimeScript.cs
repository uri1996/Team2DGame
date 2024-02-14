using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlimeScript : MonoBehaviour
{
    public GameObject[] waypoints;
    private PlayerController player;
    public bool chaseableStage;
    [Range(0f, 10f)] public float moveSpeed = 3f;
    [Range(0f,10f)]public float chaseSpeed = 5f;
    private int waypointIndex = 0;

    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>();
    }

    // Update is called once per frame
    void Update()
    {
        if(CanChase())
        {
            ChasePlayer();
        }
        else
        {
            Patrol();
        }

    }

    void Patrol()
    {
        //���݂̈ʒu�Ɍ�����
        float direction = Mathf.Sign(waypoints[waypointIndex].transform.position.x - transform.position.x);
        transform.localScale = new Vector3(-Mathf.Abs(transform.localScale.x) * direction, transform.localScale.y, transform.localScale.z);

        //���݂̈ʒu�֍s��
        transform.position = Vector2.MoveTowards(transform.position, waypoints[waypointIndex].transform.position, moveSpeed * Time.deltaTime);

        if (CanGoToNextLocation())
        {
            waypointIndex = (waypointIndex + 1) % waypoints.Length;
        }
    }

    bool CanGoToNextLocation()
    {
        return Vector2.Distance(transform.position, waypoints[waypointIndex].transform.position) < 0.1f;
    }

    bool CanChase()
    {
        return chaseableStage && player.playerColor == Color.white;
    }

    void ChasePlayer()
    {
        //�v���C���\�֍s��
        transform.position = Vector2.MoveTowards(transform.position, player.transform.position, chaseSpeed * Time.deltaTime);

        if(transform.position.x < player.transform.position.x )
        {
            transform.localScale = new Vector3(-Mathf.Abs(transform.localScale.x), transform.localScale.y, transform.localScale.z);
        }
        else
        {
            transform.localScale = new Vector3(Mathf.Abs(transform.localScale.x), transform.localScale.y, transform.localScale.z);
        }
    }
}
