using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FallDummy : MonoBehaviour
{
    [SerializeField] GameObject J;

    public GameObject CreatePrefab; //��������v���t�@�u�̔�


    // Start is called before the first frame update
    void Start()
    {
        Invoke("DummyCreate", J.GetComponent<Level4Judge>().WaitTime - 1.0f);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void DummyCreate()
    {
        //��ʒ�����Dummy�������Ă���
        GameObject dummy = Instantiate(CreatePrefab) as GameObject;
        dummy.transform.position = new Vector2(0, 5);
    }
}
