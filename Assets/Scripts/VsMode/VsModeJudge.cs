using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using System.Threading.Tasks; //�w�肵�����Ԃ�҂@�\

public class VsModeJudge : MonoBehaviour
{
    //���b��ɕ\��������̔�
    public GameObject Exclamation;
    public GameObject P1Otetsuki;
    public GameObject P2Otetsuki;
    public GameObject Ready;
    public GameObject Go;
    public GameObject P1; //Player1
    public GameObject P2; //Player2
    public GameObject P1Win;
    public GameObject P2Win;

    public AudioClip[] sounds;
    AudioSource audioSource;

    private static int P1OtetsukiCount = 0; //P1�̂���t���̉�
    private static int P2OtetsukiCount = 0; //P2�̂���t���̉�

    private float waitTime; //�����擾�p
    public static float EnemyReactionRate = 3.0f; //�G�̔������x

    private bool ClickFlag = false; //�v���C���[���N���b�N�����珟�Ă�܂ł�Flag
    private bool PushFlag = false; //�v���C���[���N���b�N�����������𔻒f
    private bool StartFlag = false; //�Q�[�����n�܂�t���O


    // Start is called before the first frame update
    void Start()
    {
        audioSource = GetComponent<AudioSource>();

        waitTime = Random.Range(5, 16); //5�ȏ�16�����̗����𐶐�
        StartCoroutine("ReadyGo");
        StartCoroutine("ChangeExclamationShow");

    }

    // Update is called once per frame
    void Update()
    {
        if (StartFlag)�@//�J�n�O�̓N���b�N�ł��Ȃ�
        {
            if (!ClickFlag)
            {
                if (Input.GetKey("a"))
                {
                    PlayOtetsuki("P1");
                }
                if (Input.GetKey("l"))
                {
                    PlayOtetsuki("P2");
                }

            }

            if (ClickFlag)
            {
                if (Input.GetKey("a"))
                {
                    //OtetsukiCount�����Z�b�g
                    P1OtetsukiCount = 0;
                    P2OtetsukiCount = 0;

                    //!!������
                    Exclamation.SetActive(false);
                    Player1Win();
                }

                if (Input.GetKey("l"))
                {
                    //OtetsukiCount�����Z�b�g
                    P1OtetsukiCount = 0;
                    P2OtetsukiCount = 0;

                    //!!������
                    Exclamation.SetActive(false);
                    Player2Win();
                }
            }
        }
    }

    private IEnumerator ReadyGo()
    {
        //�悧���̌��ʉ�
        audioSource.PlayOneShot(sounds[0]);
        yield return new WaitForSeconds(1.5f);

        //�悧����P1,P2�������J�n��������悤��
        Ready.SetActive(false);
        P1.SetActive(false);
        P2.SetActive(false);
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
        yield return new WaitForSeconds(waitTime);

        if (!PushFlag)
        {
            Exclamation.SetActive(true);

            //!!���o�����̌��ʉ�
            audioSource.PlayOneShot(sounds[2]);

            ClickFlag = true;
        }
    }

    public async void Player1Win()
    {
        Vector2 tmp; //�ꏊ�����ւ���Ƃ��̔�

        if (!PushFlag)
        {
            //�}�E�X��push�����t���O��true
            PushFlag = true;

            //�@�G�ƃv���C���[�̏����擾
            GameObject player1 = GameObject.Find("Player1");
            GameObject player2 = GameObject.Find("Player2");

            // �ꏊ�̓���ւ�
            tmp = player1.transform.position;
            player1.transform.position = player2.transform.position;
            player2.transform.position = tmp;

            //����ւ�莞�̌��ʉ�
            audioSource.PlayOneShot(sounds[3]);

            //P2���|���
            player2.transform.Rotate(0, 0, -90.0f);

            //Debug.Log("������");

            await Task.Delay(1000);

            //P1�̏���\��
            P1Win.SetActive(true);
            //P1�����������̌��ʉ�
            audioSource.PlayOneShot(sounds[4]);

            await Task.Delay(3000);

            //���̃��x���Ɉڍs(level5�Ȃ�N���A���)
            SceneManager.LoadScene("Start");

        }
    }

    public async void Player2Win()
    {
        Vector2 tmp; //�ꏊ�����ւ���Ƃ��̔�

        if (!PushFlag) //�����}�E�X��push����Ă��Ȃ����
        {
            //�}�E�X���N���b�N�����t���O��true
            PushFlag = true;

            //�@�G�ƃv���C���[�̏����擾
            GameObject player1 = GameObject.Find("Player1");
            GameObject player2 = GameObject.Find("Player2");

            // �ꏊ�̓���ւ�
            tmp = player1.transform.position;
            player1.transform.position = player2.transform.position;
            player2.transform.position = tmp;

            //����ւ�莞�̌��ʉ�
            audioSource.PlayOneShot(sounds[3]);

            //P1���|���
            player1.transform.Rotate(0, 0, 90.0f);

            await Task.Delay(1000); //1000ms=1s

            //P2�̏���\��
            P2Win.SetActive(true);
            //P2�����������̌��ʉ�
            audioSource.PlayOneShot(sounds[5]);

            await Task.Delay(3000);

            //�Q�[���I�[�o�[��ʂɈڍs
            SceneManager.LoadScene("Start");

        }
    }

    //���ʂ��������t���O ����t���֐��Ŏg��
    private static bool P1PassOnceFlag = false;
    private static bool P2PassOnceFlag = false;

    public async void PlayOtetsuki(string Player) //����t���֐�
    {
        if (!PushFlag)
        {
            //�����������ture
            PushFlag = true;

            if (Player == "P1")
            {
                P1OtetsukiCount = P1OtetsukiCount + 1;
            }
            else
            {
                P2OtetsukiCount = P2OtetsukiCount + 1;
            }

            if (P1OtetsukiCount == 1 || P2OtetsukiCount == 1)
            {
                if (P1OtetsukiCount == 1)
                {
                    if (!P1PassOnceFlag)
                    {
                        P1PassOnceFlag = true;

                        //����t����������悤��
                        P1Otetsuki.SetActive(true);

                        //����t���̌��ʉ�
                        audioSource.PlayOneShot(sounds[6]);
                    }
                }

                if (P2OtetsukiCount == 1)
                {
                    if (!P2PassOnceFlag)
                    {
                        P2PassOnceFlag = true;

                        //����t����������悤��
                        P2Otetsuki.SetActive(true);

                        //����t���̌��ʉ�
                        audioSource.PlayOneShot(sounds[6]);
                    }
                }

                await Task.Delay(2000);

                //���̃V�[�����ēǂݍ���
                SceneManager.LoadScene(SceneManager.GetActiveScene().name);
            }

            if (P1OtetsukiCount == 2 || P2OtetsukiCount == 2) //�Q��ŃQ�[���I�[�o�[
            {
                //static�Ȃ̂ŏ�����
                P1PassOnceFlag = false;
                P2PassOnceFlag = false;

                if (P1OtetsukiCount == 2)
                {
                    //����t����������悤��
                    P1Otetsuki.SetActive(true);

                    //����t���̌��ʉ�
                    audioSource.PlayOneShot(sounds[6]);

                    await Task.Delay(2000);

                    P1Otetsuki.SetActive(false);

                    //P2�̏���\��
                    P2Win.SetActive(true);
                    //P2�����������̌��ʉ�
                    audioSource.PlayOneShot(sounds[5]);
                }

                if(P2OtetsukiCount == 2)
                {
                    //����t����������悤��
                    P2Otetsuki.SetActive(true);

                    //����t���̌��ʉ�
                    audioSource.PlayOneShot(sounds[6]);

                    await Task.Delay(2000);

                    P2Otetsuki.SetActive(false);

                    //P1�̏���\��
                    P1Win.SetActive(true);
                    //P1�����������̌��ʉ�
                    audioSource.PlayOneShot(sounds[4]);
                }

                //OtetsukiCount�����Z�b�g
                P1OtetsukiCount = 0;
                P2OtetsukiCount = 0;

                await Task.Delay(3000);
                //Start��ʂɈڍs
                SceneManager.LoadScene("Start");
            }
        }
    }
}