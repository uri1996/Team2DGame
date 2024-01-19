using UnityEngine;
using UnityEngine.Rendering;

public class SlimeController : MonoBehaviour
{
    float speed = 3.0f;

    float angle = -1;//-1‚Å¶,1‚Å‰E
    GameObject player;
    PlayerController controller;
    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.Find("player");
        controller = player.GetComponent<PlayerController>();
    }

    // Update is called once per frame
    void Update()
    {
        //ƒvƒŒƒCƒ„‚ª“®‚¯‚È‚¢‚Æ‚«‚Í“®‚©‚³‚È‚¢
        if (!controller.isAbleToMove) { return; }
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
