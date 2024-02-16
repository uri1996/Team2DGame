using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Transition : MonoBehaviour
{
    readonly float waitTime = 2f;

    // Start is called before the first frame update
    void Start()
    {
        DontDestroyOnLoad(gameObject);

        StartCoroutine(nameof(SimpleTransition));
    }
    IEnumerator SimpleTransition()
    {
        yield return new WaitForSeconds(waitTime);

        Destroy(gameObject);
    }
    // Update is called once per frame
    void Update()
    {
        
    }
}
