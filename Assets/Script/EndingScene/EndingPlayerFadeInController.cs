using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EndingPlayerFadeInController : MonoBehaviour
{
    [Range(0.0f,1f)]public float FadeInSpeed = 0.25f;//fadeInのスピード
    public float UntilFadeIn_WaitingTime = 1.0f;//エンディング画面に入るからfadeInまでの待ち時間

    private bool isFadeIn_On;//フラグ

    void Start()
    {
        isFadeIn_On = false;
        StartCoroutine(FadeIn());
    }
    IEnumerator FadeIn()
    {
        while (true)
        {
            yield return new WaitForSecondsRealtime(UntilFadeIn_WaitingTime);
            isFadeIn_On = true;
            yield break;
        }
    }

    void Update()
    {
        if (isFadeIn_On)
        {
            GetComponent<SpriteRenderer>().color += new Color(0, 0, 0, FadeInSpeed) * Time.deltaTime;
        }
    }
}
