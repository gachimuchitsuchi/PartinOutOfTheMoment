using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using System.Threading.Tasks; //�w�肵�����Ԃ�҂@�\

public class Judge : MonoBehaviour
{
    //���b��ɕ\����������������肷�锠
    public GameObject Exclamation;
    public GameObject Otetsuki;
    public GameObject Ready;
    public GameObject Go;
    public GameObject Win;
    public GameObject You;
    private GameObject Crow;

    public AudioClip[] sounds;
    AudioSource audioSource;

    public string SceneName; //���̃V�[���̈ړ���̔�
    public static string NowSceneName; //���g���C�p,���̃Q�[���V�[����ۑ�

    private static int OtetsukiCount = 0; //����t���̉�

    private float WaitTime; //�����擾�p
    public float EnemyReactionRate = 3.0f; //�G�̔������x

    public bool ClickFlag = false; //�v���C���[���N���b�N�ł���悤�ɂ���Flag,FallPoop�Ƀt���O��n������public
    public bool PushFlag = false; //�v���C���[���N���b�N�����������𔻒f poop�ɓ����������Ɏg������public
    private bool EnemyWinFlag = false;
    private bool StartFlag = false; //�悧���n�߂̎���Update�𐧌�


    // Start is called before the first frame update
    void Start()
    {
        audioSource = GetComponent<AudioSource>();

        Crow = GameObject.Find("Crow");

        WaitTime = Random.Range(5, 11); //5�ȏ�11�����̗����𐶐�

        StartCoroutine("ReadyGo");
        StartCoroutine("ChangeExclamationShow");

        //���g���C�p�ɍ��̃Q�[���V�[�����擾
        NowSceneName = SceneManager.GetActiveScene().name;
    }

    // Update is called once per frame
    void Update()
    {
        if (StartFlag)
        {
            if (!ClickFlag)�@//!!���o��O��
            {
                if (Input.GetMouseButtonDown(0))
                {
                    if (Crow)
                    {
                        Destroy(Crow);
                    }

                    PlayOtetsuki(); //����t���֐������s
                }
            }

            if (ClickFlag) //!!���o����
            {
                if (Input.GetMouseButtonDown(0))
                {
                    if (Crow)
                    {
                        Destroy(Crow);
                    }

                    Exclamation.SetActive(false);
                    PlayerWin();
                }

                Invoke("ChangeFlag", EnemyReactionRate); //default3�b��ɓG������Flag��true

                if (EnemyWinFlag)
                {
                    if (Crow)
                    {
                        Destroy(Crow);
                    }

                    OtetsukiCount = 0;

                    EnemyWinFlag = false;
                    Exclamation.SetActive(false);
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

    IEnumerator ChangeExclamationShow() //�҂����Ԍ�Ɂu!!�v��������悤�ɂ���֐�
    {
        yield return new WaitForSeconds(1.5f + WaitTime);

        if (!PushFlag) //�N���b�N���܂�����Ă��Ȃ����
        {
            Exclamation.SetActive(true);

            //!!���o�����̌��ʉ�
            audioSource.PlayOneShot(sounds[2]);

            ClickFlag = true;
        }
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

            //���̃��x���Ɉڍs(level5�Ȃ�N���A���)
            SceneManager.LoadScene(SceneName);

        }
    }

    public async void EnemyWin()
    {
        Vector2 tmp; //�ꏊ�����ւ���Ƃ��̔�

        if(!PushFlag) //�����}�E�X��push����Ă��Ȃ����
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

    public async void PlayOtetsuki() //����t���֐�
    {
        if (!PushFlag)
        {
            PushFlag = true;

            OtetsukiCount = OtetsukiCount + 1;

            if (OtetsukiCount == 2) //�Q��ŃQ�[���I�[�o�[
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

    public void ChangeFlag()
    {
        EnemyWinFlag = true;
    }
}
