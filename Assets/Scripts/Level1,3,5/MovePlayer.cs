using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovePlayer : MonoBehaviour
{
    [SerializeField] GameObject J;

    public AudioClip PoopSound;
    AudioSource audioSource;

    public GameObject Poop;

    public float speed = 1; //プレイヤーの速度

    private Vector2 PlayerPos;

    // Start is called before the first frame update
    void Start()
    {
        audioSource = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void FixedUpdate()
    {
        this.MovingRestrictions();

        if (!J.GetComponent<Judge>().ClickFlag)
        {
            if (Input.GetKey("a"))
            {
                this.transform.Translate(-speed / 50, 0, 0);
            }
            if (Input.GetKey("d"))
            {
                this.transform.Translate(speed / 50, 0, 0);
            }
        }
    }

    private void MovingRestrictions() //移動制限関数
    {
        //変数に自分の今の位置を入れる
        this.PlayerPos = transform.position;

        //PlayerPos変数のxに制限した値を入れる
        //PlayerPos.xという値を-6.0fから2.5fの間に収める
        this.PlayerPos.x = Mathf.Clamp(this.PlayerPos.x, -6.0f, 2.5f);

        transform.position = new Vector2(this.PlayerPos.x, this.PlayerPos.y);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        //あたった相手のtagがPoopだったら
        if(collision.gameObject.tag == "Poop")
        {
            //!!が出ないようにする
            J.GetComponent<Judge>().PushFlag = true;
            audioSource.PlayOneShot(PoopSound);
        }
    }
}
