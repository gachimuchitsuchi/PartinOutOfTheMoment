using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveDown : MonoBehaviour
{
    public float speed = 5;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    private void FixedUpdate() //1�b�Ԃ�50����s�����
    {
        this.transform.Translate(0, -speed/50, 0); //speed�̒l�ŉ��ɗ���
    }
}
