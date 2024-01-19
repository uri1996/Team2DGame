using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightController : MonoBehaviour
{
    private float flashTime = 0.1f;
    private Renderer myRenderer;
    void Start()
    {  
        IniGameObject();
        StartCoroutine(LightConTrol());
    }

    private void IniGameObject()//�_����Ԃ�������
    {
        myRenderer = GetComponent<Renderer>();
        myRenderer.enabled = true;
    }

    IEnumerator LightConTrol()
    {
        while (true)
        {
            //���̎��Ԃ�҂i���͓_����ԁj
            yield return new WaitForSecondsRealtime(Random.Range(3f, 12f));

            //�t���b�V���J�n
            myRenderer.enabled=false;
            yield return new WaitForSecondsRealtime(flashTime);
            myRenderer.enabled = true;
            yield return new WaitForSecondsRealtime(flashTime);
            myRenderer.enabled=false;
            yield return new WaitForSecondsRealtime(flashTime);
            myRenderer.enabled=true;
            yield return new WaitForSecondsRealtime(Random.Range(0.8f, 2.8f));
            myRenderer.enabled=false; //�Ō�͏������

            //�t���b�V���I���i���͏�����ԁj
            //���̎��Ԃ�҂�
            yield return new WaitForSecondsRealtime(Random.Range(3f, 10f));

            //�t���b�V���J�n
            myRenderer.enabled=true;
            yield return new WaitForSecondsRealtime(flashTime);
            myRenderer.enabled=false;
            yield return new WaitForSecondsRealtime(flashTime);
            myRenderer.enabled=true; //�Ō�͓_�����
            //�ŏ��ɖ߂��i�z�j
        }
    }
}
