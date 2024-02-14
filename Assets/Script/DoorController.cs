using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorController : MonoBehaviour
{
    public GameObject nextDoor; //çsêÊ
    GameObject player;
    public Color color;
    public bool clearDoor;
    Animator animator;
    AudioSource audioSource;
    void Start()
    {
        player = GameObject.Find("player");
        animator = GetComponent<Animator>();
        audioSource = GetComponent<AudioSource>();
    }
    public void MoveToDoor()
    {
        player.GetComponent<PlayerController>().Appear();
        player.transform.position = nextDoor.transform.position;
        nextDoor.GetComponent<DoorController>().AnimationDoor();
        player.GetComponent<PlayerController>().isAbleToMove = true;
    }

    public void AnimationDoor()
    {
        animator.SetTrigger("Open");
        audioSource.Play();
    }
}
