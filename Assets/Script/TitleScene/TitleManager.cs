using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TitleManager : MonoBehaviour
{
    AudioSource sound;
    void Start()
    {
        sound = GetComponent<AudioSource>();
    }

    void Update()
    {
        if(Input.GetButtonDown("AButton")) 
        {
            sound.Play();
            Invoke("ToNextScene", 1);
        }
    }

    void ToNextScene()
    {
        SceneManager.LoadScene("Stage01");
    }
}
