using UnityEngine;

public class SlimeController : MonoBehaviour
{
    float speed = 3.0f;

    float angle = -1;//-1Ç≈ç∂,1Ç≈âE
    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        transform.Translate(speed * (float)angle * Time.deltaTime, 0.0f, 0.0f);
        transform.localScale = new Vector3((float)-angle, 1.0f, 1.0f);
    }

    public void SetAngle(float angle_)
    {
        angle = angle_;
    }

    public float GetAngle()
    {
        return angle;
    }
}
