using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovePlayer : MonoBehaviour
{
    [SerializeField] GameObject J;

    public AudioClip PoopSound;
    AudioSource audioSource;

    public GameObject Poop;

    public float speed = 1; //�v���C���[�̑��x

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

    private void MovingRestrictions() //�ړ������֐�
    {
        //�ϐ��Ɏ����̍��̈ʒu������
        this.PlayerPos = transform.position;

        //PlayerPos�ϐ���x�ɐ��������l������
        //PlayerPos.x�Ƃ����l��-6.0f����2.5f�̊ԂɎ��߂�
        this.PlayerPos.x = Mathf.Clamp(this.PlayerPos.x, -6.0f, 2.5f);

        transform.position = new Vector2(this.PlayerPos.x, this.PlayerPos.y);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        //�������������tag��Poop��������
        if(collision.gameObject.tag == "Poop")
        {
            //!!���o�Ȃ��悤�ɂ���
            J.GetComponent<Judge>().PushFlag = true;
            audioSource.PlayOneShot(PoopSound);
        }
    }
}
