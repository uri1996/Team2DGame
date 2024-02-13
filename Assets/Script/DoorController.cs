using System.Collections;
using System.Collections.Generic;
using UnityEditor.Build.Content;
using UnityEngine;

public class DoorController : MonoBehaviour
{
    public GameObject nextDoor; //çsêÊ
    GameObject player;
    public enum DoorColor
    {
        White,Black
    }
    public DoorColor color;
    void Start()
    {
        player = GameObject.Find("player");
    }
    public void MoveDoor()
    {
        player.GetComponent<PlayerController>().Appear();
        player.transform.position = nextDoor.transform.position;
        nextDoor.GetComponent<Animator>().SetTrigger("Open");
        nextDoor.GetComponent<AudioSource>().Play();
        player.GetComponent<PlayerController>().isAbleToMove = true;
    }
}
