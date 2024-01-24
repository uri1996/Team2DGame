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
        Application.targetFrameRate = 60;
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
