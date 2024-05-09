using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FallPoop : MonoBehaviour
{
    [SerializeField] GameObject J; //Judge�X�N���v�g����l���擾���邽�߂̔�

    public GameObject CreatePrefab; //��������v���t�@�u�̔�

    //SE��炷����
    public AudioClip CrowSound;
    AudioSource audioSource;

    public int GameLevel; //level3��level5�̋�ʗp


    // Start is called before the first frame update
    void Start()
    {
        audioSource = GetComponent<AudioSource>();

        if (GameLevel == 3) //level3�̃X�e�[�W��������
        {
            Invoke("Create", 5); //5�b��Ɉ��poop�𐶐�
        }
        if(GameLevel == 5) //level5�̃X�e�[�W��������
        {
            InvokeRepeating("Create", 5, 2); //�T�b��ɂQ�b����poop�𐶐�
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Create()
    {
        GameObject poop; //prefab�����锠

        if (!J.GetComponent<Judge>().ClickFlag) //�v���C���[���N���b�N���Ă̓_���Ȏ�
        {
            //�J���X�̖���
            audioSource.PlayOneShot(CrowSound);

            //�v���C���[��T��
            GameObject player = GameObject.Find("Player");

            //�e�I�u�W�F�N�g��Crow��T��
            GameObject crow = GameObject.Find("Crow");

            //�v���C���[�̐^���poop�𐶐�
            poop = Instantiate(CreatePrefab) as GameObject;
            poop.transform.parent = crow.transform;
            poop.transform.position = new Vector3(player.transform.position.x, 5, 0);
        }
    }
}
