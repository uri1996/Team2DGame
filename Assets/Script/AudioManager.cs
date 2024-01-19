using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class AudioManager : MonoBehaviour
{
    [SerializeField] AudioSource musicSource;

    public AudioClip BGM;
    public static AudioManager instance;
    // Start is called before the first frame update
    void Start()
    {
        musicSource.clip = BGM;
        musicSource.Play();
    }

    // Update is called once per frame
    void Update()
    {
        if (SceneManager.GetActiveScene().name == "EndingScene")
        {
            Destroy(gameObject);
        } 
    }
   

    private void Awake()
    {
        if (instance != null)
        {
            Destroy(gameObject);
        }
        else
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        } 
    }
}
