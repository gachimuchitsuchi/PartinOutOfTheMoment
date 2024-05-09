using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using System.Threading.Tasks; //指定した時間を待つ機能

public class VsModeJudge : MonoBehaviour
{
    //数秒後に表示させるの箱
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

    private static int P1OtetsukiCount = 0; //P1のお手付きの回数
    private static int P2OtetsukiCount = 0; //P2のお手付きの回数

    private float waitTime; //乱数取得用
    public static float EnemyReactionRate = 3.0f; //敵の反応速度

    private bool ClickFlag = false; //プレイヤーがクリックしたら勝てるまでのFlag
    private bool PushFlag = false; //プレイヤーがクリックを押したかを判断
    private bool StartFlag = false; //ゲームが始まるフラグ


    // Start is called before the first frame update
    void Start()
    {
        audioSource = GetComponent<AudioSource>();

        waitTime = Random.Range(5, 16); //5以上16未満の乱数を生成
        StartCoroutine("ReadyGo");
        StartCoroutine("ChangeExclamationShow");

    }

    // Update is called once per frame
    void Update()
    {
        if (StartFlag)　//開始前はクリックできない
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
                    //OtetsukiCountをリセット
                    P1OtetsukiCount = 0;
                    P2OtetsukiCount = 0;

                    //!!を消す
                    Exclamation.SetActive(false);
                    Player1Win();
                }

                if (Input.GetKey("l"))
                {
                    //OtetsukiCountをリセット
                    P1OtetsukiCount = 0;
                    P2OtetsukiCount = 0;

                    //!!を消す
                    Exclamation.SetActive(false);
                    Player2Win();
                }
            }
        }
    }

    private IEnumerator ReadyGo()
    {
        //よぉいの効果音
        audioSource.PlayOneShot(sounds[0]);
        yield return new WaitForSeconds(1.5f);

        //よぉいとP1,P2を消し開始を見えるように
        Ready.SetActive(false);
        P1.SetActive(false);
        P2.SetActive(false);
        Go.SetActive(true);

        //始めの効果音
        audioSource.PlayOneShot(sounds[1]);

        //update関数をスタート
        StartFlag = true;

        yield return new WaitForSeconds(1.0f);
        //開始を消す
        Go.SetActive(false);
    }

    IEnumerator ChangeExclamationShow() //待ち時間後に実行する関数
    {
        yield return new WaitForSeconds(waitTime);

        if (!PushFlag)
        {
            Exclamation.SetActive(true);

            //!!が出た時の効果音
            audioSource.PlayOneShot(sounds[2]);

            ClickFlag = true;
        }
    }

    public async void Player1Win()
    {
        Vector2 tmp; //場所を入れ替えるときの箱

        if (!PushFlag)
        {
            //マウスをpushしたフラグをtrue
            PushFlag = true;

            //　敵とプレイヤーの情報を取得
            GameObject player1 = GameObject.Find("Player1");
            GameObject player2 = GameObject.Find("Player2");

            // 場所の入れ替え
            tmp = player1.transform.position;
            player1.transform.position = player2.transform.position;
            player2.transform.position = tmp;

            //入れ替わり時の効果音
            audioSource.PlayOneShot(sounds[3]);

            //P2が倒れる
            player2.transform.Rotate(0, 0, -90.0f);

            //Debug.Log("押した");

            await Task.Delay(1000);

            //P1の勝を表示
            P1Win.SetActive(true);
            //P1が勝った時の効果音
            audioSource.PlayOneShot(sounds[4]);

            await Task.Delay(3000);

            //次のレベルに移行(level5ならクリア画面)
            SceneManager.LoadScene("Start");

        }
    }

    public async void Player2Win()
    {
        Vector2 tmp; //場所を入れ替えるときの箱

        if (!PushFlag) //もしマウスがpushされていなければ
        {
            //マウスをクリックしたフラグをtrue
            PushFlag = true;

            //　敵とプレイヤーの情報を取得
            GameObject player1 = GameObject.Find("Player1");
            GameObject player2 = GameObject.Find("Player2");

            // 場所の入れ替え
            tmp = player1.transform.position;
            player1.transform.position = player2.transform.position;
            player2.transform.position = tmp;

            //入れ替わり時の効果音
            audioSource.PlayOneShot(sounds[3]);

            //P1が倒れる
            player1.transform.Rotate(0, 0, 90.0f);

            await Task.Delay(1000); //1000ms=1s

            //P2の勝を表示
            P2Win.SetActive(true);
            //P2が勝った時の効果音
            audioSource.PlayOneShot(sounds[5]);

            await Task.Delay(3000);

            //ゲームオーバー画面に移行
            SceneManager.LoadScene("Start");

        }
    }

    //一回通った後閉じるフラグ お手付き関数で使う
    private static bool P1PassOnceFlag = false;
    private static bool P2PassOnceFlag = false;

    public async void PlayOtetsuki(string Player) //お手付き関数
    {
        if (!PushFlag)
        {
            //押した判定をture
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

                        //お手付きを見えるように
                        P1Otetsuki.SetActive(true);

                        //お手付きの効果音
                        audioSource.PlayOneShot(sounds[6]);
                    }
                }

                if (P2OtetsukiCount == 1)
                {
                    if (!P2PassOnceFlag)
                    {
                        P2PassOnceFlag = true;

                        //お手付きを見えるように
                        P2Otetsuki.SetActive(true);

                        //お手付きの効果音
                        audioSource.PlayOneShot(sounds[6]);
                    }
                }

                await Task.Delay(2000);

                //今のシーンを再読み込み
                SceneManager.LoadScene(SceneManager.GetActiveScene().name);
            }

            if (P1OtetsukiCount == 2 || P2OtetsukiCount == 2) //２回でゲームオーバー
            {
                //staticなので初期化
                P1PassOnceFlag = false;
                P2PassOnceFlag = false;

                if (P1OtetsukiCount == 2)
                {
                    //お手付きを見えるように
                    P1Otetsuki.SetActive(true);

                    //お手付きの効果音
                    audioSource.PlayOneShot(sounds[6]);

                    await Task.Delay(2000);

                    P1Otetsuki.SetActive(false);

                    //P2の勝を表示
                    P2Win.SetActive(true);
                    //P2が勝った時の効果音
                    audioSource.PlayOneShot(sounds[5]);
                }

                if(P2OtetsukiCount == 2)
                {
                    //お手付きを見えるように
                    P2Otetsuki.SetActive(true);

                    //お手付きの効果音
                    audioSource.PlayOneShot(sounds[6]);

                    await Task.Delay(2000);

                    P2Otetsuki.SetActive(false);

                    //P1の勝を表示
                    P1Win.SetActive(true);
                    //P1が勝った時の効果音
                    audioSource.PlayOneShot(sounds[4]);
                }

                //OtetsukiCountをリセット
                P1OtetsukiCount = 0;
                P2OtetsukiCount = 0;

                await Task.Delay(3000);
                //Start画面に移行
                SceneManager.LoadScene("Start");
            }
        }
    }
}