using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using System.Threading.Tasks; //�w�肵�����Ԃ�҂@�\

public class Level4Judge : MonoBehaviour
{
    /*Dummy�p�̊֐���ǉ�*/



    public GameObject Exclamation; //���b��ɕ\��������!!�}�[�N�̔�
    public GameObject Otetsuki;
    public GameObject Ready;
    public GameObject Go;
    public GameObject Win;
    public GameObject You;
    public GameObject DummyPrefab;
    private GameObject Dummy; //��������Dummy���ꎞ�ۑ����锠
    public GameObject SplitDummyPrefab;

    public AudioClip[] sounds;
    AudioSource audioSource;

    public string SceneName;

    private static int OtetsukiCount = 0;

    public float WaitTime; //�����擾�p,FallDummy�ɒl��n�����߂�public

    private bool ClickFlag = false; //�v���C���[���N���b�N�ł���悤�ɂ���Flag
    private bool PushFlag = false; //�v���C���[���N���b�N�����������𔻒f
    private bool EnemyWinFlag = false; //�G������Flag
    private bool StartFlag = false;


    // Start is called before the first frame update
    void Start()
    {
        audioSource = GetComponent<AudioSource>();

        WaitTime = Random.Range(5, 16); //5�ȏ�16�����̗����𐶐�

        StartCoroutine("ReadyGo");
        StartCoroutine("ChangeExclamationShow");
        StartCoroutine("EnemyPosChange");
        StartCoroutine("DummyCreate");

        //���g���C�p�ɍ��̃Q�[���V�[����NowSceneName�ɓn��
        Judge.NowSceneName = SceneManager.GetActiveScene().name;
    }

    // Update is called once per frame
    void Update()
    {
        if (StartFlag)
        {
            Invoke("DummyCreate", WaitTime - 1.0f);
            if (!ClickFlag)
            {
                if (Input.GetMouseButtonDown(0))
                {
                    PlayOtetsuki();
                }
            }

            if (ClickFlag) //�N���b�N�\�ɂȂ�����
            {
                if (Dummy == null)
                {
                    if (Input.GetMouseButtonDown(0))
                    {
                        Exclamation.SetActive(false);
                        PlayerWin();
                    }
                }
                else
                {
                    if (Input.GetMouseButtonDown(0))
                    {
                        Exclamation.SetActive(false);
                        ChangeDummy("Player");
                        Invoke("ChangeFlag", 1);
                    }
                }

                if (EnemyWinFlag)
                {
                    OtetsukiCount = 0;

                    PushFlag = false;
                    EnemyWinFlag = false;
                    EnemyWin();
                }
            }
        }
    }

    IEnumerator ReadyGo()
    {
        //�悧���̌��ʉ�
        audioSource.PlayOneShot(sounds[0]);
        yield return new WaitForSeconds(1.5f);

        //�悧���Ƃ��Ȃ������J�n��������悤��
        Ready.SetActive(false);
        You.SetActive(false);
        Go.SetActive(true);

        //�n�߂̌��ʉ�
        audioSource.PlayOneShot(sounds[1]);

        //update�֐����X�^�[�g
        StartFlag = true;

        yield return new WaitForSeconds(1.0f);
        //�J�n������
        Go.SetActive(false);
    }

    IEnumerator ChangeExclamationShow() //�҂����Ԍ��!!��������悤�ɂ���֐�
    {
        yield return new WaitForSeconds(1.5f + WaitTime);

        if (!PushFlag)
        {
            Exclamation.SetActive(true);

            //!!���o�����̌��ʉ�
            audioSource.PlayOneShot(sounds[2]);

            ClickFlag = true;
        }
    }

    IEnumerator EnemyPosChange() //�G���_�~�[�Ɉ���������
    {
        yield return new WaitForSeconds(WaitTime+4.5f);
        ChangeDummy("Samurai");
        PushFlag = false;
    }

    IEnumerator DummyCreate()
    {
        yield return new WaitForSeconds(WaitTime - 1.0f);
        //��ʒ�����Dummy�������Ă���
        Dummy = Instantiate(DummyPrefab);
        Dummy.transform.position = new Vector2(0, 5);
    }

    public async void PlayerWin()
    {
        Vector2 tmp; //�ꏊ�����ւ���Ƃ��̔�

        if (!PushFlag)
        {
            //�}�E�X��push�����t���O��true
            PushFlag = true;

            //�@�G�ƃv���C���[�̏����擾
            GameObject target = GameObject.Find("Samurai");
            GameObject player = GameObject.Find("Player");

            // �ꏊ�̓���ւ�
            tmp = target.transform.position;
            target.transform.position = player.transform.position;
            player.transform.position = tmp;

            //����ւ�莞�̌��ʉ�
            audioSource.PlayOneShot(sounds[3]);

            //�G���|���
            target.transform.Rotate(0, 0, -90.0f);

            //Debug.Log("������");

            await Task.Delay(1000);

            //����\��
            Win.SetActive(true);
            //���������̌��ʉ�
            audioSource.PlayOneShot(sounds[4]);

            await Task.Delay(1000);

            //���̃��x���Ɉڍs
            SceneManager.LoadScene(SceneName);

        }
    }

    public async void EnemyWin()
    {
        Vector2 tmp; //�ꏊ�����ւ���Ƃ��̔�

        if (!PushFlag) //�����}�E�X��push����Ă��Ȃ����
        {
            //�}�E�X���N���b�N�����t���O��true
            PushFlag = true;

            //�@�G�ƃv���C���[�̏����擾
            GameObject target = GameObject.Find("Samurai");
            GameObject player = GameObject.Find("Player");

            // �ꏊ�̓���ւ�
            tmp = target.transform.position;
            target.transform.position = player.transform.position;
            player.transform.position = tmp;

            //����ւ�莞�̌��ʉ�
            audioSource.PlayOneShot(sounds[3]);

            //�v���C���[���|���
            player.transform.Rotate(0, 0, 90.0f);

            await Task.Delay(1000); //1000ms=1s

            //�Q�[���I�[�o�[��ʂɈڍs
            SceneManager.LoadScene("GameOver");

        }
    }

    public async void PlayOtetsuki()
    {
        if (!PushFlag)
        {
            PushFlag = true;

            OtetsukiCount = OtetsukiCount + 1;

            if (OtetsukiCount == 2)
            {
                OtetsukiCount = 0;

                //����t����������悤��
                Otetsuki.SetActive(true);

                //����t���̌��ʉ�
                audioSource.PlayOneShot(sounds[5]);

                await Task.Delay(2000);

                //GameOver��ʂɈڍs
                SceneManager.LoadScene("GameOver");
            }
            else
            {
                //����t����������悤��
                Otetsuki.SetActive(true);

                //����t���̌��ʉ�
                audioSource.PlayOneShot(sounds[5]);

                //1000ms=1s
                await Task.Delay(2000);

                //���̃V�[�����ēǂݍ���
                SceneManager.LoadScene(SceneManager.GetActiveScene().name);
            }
        }
    }

    public void ChangeDummy(string name)
    {
        Vector2 tmp; //�ꏊ�����ւ���Ƃ��̔�

        if (!PushFlag) //�����}�E�X��push����Ă��Ȃ����
        {
            //�}�E�X���N���b�N�����t���O��true
            PushFlag = true;

            //�@�G�ƃv���C���[�̏����擾
            GameObject target = GameObject.Find(name);

            // �ꏊ�̓���ւ�
            tmp = target.transform.position;
            target.transform.position = Dummy.transform.position;
            Dummy.transform.position = tmp;

            //����ւ�莞�̌��ʉ�
            audioSource.PlayOneShot(sounds[3]);

            //���̃Q�[���I�u�W�F�N�g������
            Destroy(Dummy);

            //Dummy���؂�ꂽ�I�u�W�F�N�g���擾������
            GameObject splitDummy = Instantiate(SplitDummyPrefab);
            splitDummy.transform.position = new Vector2(0, 2);

            // splitDummy��rigidbody���擾
            Rigidbody2D rb = splitDummy.GetComponent<Rigidbody2D>(); 

            if (name == "Samurai")
            {
                Vector2 force = new Vector2(5.0f, 2.0f);  // �͂�ݒ�
                rb.AddForce(force, ForceMode2D.Impulse);
            }
            else
            {
                Vector2 force = new Vector2(-5.0f, 2.0f);  // �͂�ݒ�
                rb.AddForce(force, ForceMode2D.Impulse);
            }
        }
    }

    public void ChangeFlag()
    {
        EnemyWinFlag = true;
    }
}
