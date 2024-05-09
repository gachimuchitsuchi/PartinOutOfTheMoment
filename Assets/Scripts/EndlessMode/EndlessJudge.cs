using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using System.Threading.Tasks; //�w�肵�����Ԃ�҂@�\

public class EndlessJudge : MonoBehaviour
{
    public GameObject Exclamation; //���b��ɕ\��������̔�
    public GameObject Otetsuki;
    public GameObject Ready;
    public GameObject Go;
    public GameObject Win;
    public GameObject You;

    public AudioClip[] sounds;
    AudioSource audioSource;

    public static int WinCount = 0; //��������
    private static int OtetsukiCount = 0; //����t���̉�

    private float WaitTime; //�����擾�p
    public static float EnemyReactionRate = 1.0f; //�G�̔������x

    private bool ClickFlag = false; //�v���C���[���N���b�N�ł���悤�ɂ���Flag
    private bool PushFlag = false; //�v���C���[���N���b�N�����������𔻒f
    private bool EnemyWinFlag = false;
    private bool StartFlag = false;


    // Start is called before the first frame update
    void Start()
    {
        audioSource = GetComponent<AudioSource>();

        WaitTime = Random.Range(5, 16); //5�ȏ�16�����̗����𐶐�
        StartCoroutine("ReadyGo");
        StartCoroutine("ChangeExclamationShow");

    }

    // Update is called once per frame
    void Update()
    {
        if (StartFlag)
        {
            if (!ClickFlag)
            {
                if (Input.GetMouseButtonDown(0))
                {
                    PlayOtetsuki();
                }

            }

            if (ClickFlag)
            {
                if (Input.GetMouseButtonDown(0))
                {
                    OtetsukiCount = 0;

                    WinCount = WinCount + 1;

                    ClickFlag = false;


                    //�G�̔������x���ǂ�ǂ񑁂�����
                    if (EnemyReactionRate <= 1.0f && EnemyReactionRate > 0.3f)
                    {
                        EnemyReactionRate = EnemyReactionRate - 0.1f;
                    }

                    if(EnemyReactionRate <= 0.3f)
                    {
                        EnemyReactionRate = EnemyReactionRate - 0.02f;
                    }

                    //Debug.Log(EnemyReactionRate);

                    //!!������
                    Exclamation.SetActive(false);
                    PlayerWin();
                }

                Invoke("ChangeFlag", EnemyReactionRate);

                if (EnemyWinFlag)
                {
                    //!!������
                    Exclamation.SetActive(false);
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

    IEnumerator ChangeExclamationShow() //�҂����Ԍ�Ɏ��s����֐�
    {
        yield return new WaitForSeconds(WaitTime);

        if (!PushFlag)
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
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);

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
            SceneManager.LoadScene("EndlessResult");

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
                //����t����������悤��
                Otetsuki.SetActive(true);

                //����t���̌��ʉ�
                audioSource.PlayOneShot(sounds[5]);

                await Task.Delay(2000);
                //GameOver��ʂɈڍs
                SceneManager.LoadScene("EndlessResult");
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
