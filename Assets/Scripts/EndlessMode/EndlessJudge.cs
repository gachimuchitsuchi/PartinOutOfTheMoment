using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using System.Threading.Tasks; //指定した時間を待つ機能

public class EndlessJudge : MonoBehaviour
{
    public GameObject Exclamation; //数秒後に表示させるの箱
    public GameObject Otetsuki;
    public GameObject Ready;
    public GameObject Go;
    public GameObject Win;
    public GameObject You;

    public AudioClip[] sounds;
    AudioSource audioSource;

    public static int WinCount = 0; //勝った回数
    private static int OtetsukiCount = 0; //お手付きの回数

    private float WaitTime; //乱数取得用
    public static float EnemyReactionRate = 1.0f; //敵の反応速度

    private bool ClickFlag = false; //プレイヤーがクリックできるようにするFlag
    private bool PushFlag = false; //プレイヤーがクリックを押したかを判断
    private bool EnemyWinFlag = false;
    private bool StartFlag = false;


    // Start is called before the first frame update
    void Start()
    {
        audioSource = GetComponent<AudioSource>();

        WaitTime = Random.Range(5, 16); //5以上16未満の乱数を生成
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


                    //敵の反応速度をどんどん早くする
                    if (EnemyReactionRate <= 1.0f && EnemyReactionRate > 0.3f)
                    {
                        EnemyReactionRate = EnemyReactionRate - 0.1f;
                    }

                    if(EnemyReactionRate <= 0.3f)
                    {
                        EnemyReactionRate = EnemyReactionRate - 0.02f;
                    }

                    //Debug.Log(EnemyReactionRate);

                    //!!を消す
                    Exclamation.SetActive(false);
                    PlayerWin();
                }

                Invoke("ChangeFlag", EnemyReactionRate);

                if (EnemyWinFlag)
                {
                    //!!を消す
                    Exclamation.SetActive(false);
                    EnemyWinFlag = false;
                    EnemyWin();
                }
            }
        }
    }

    IEnumerator ReadyGo()
    {
        //よぉいの効果音
        audioSource.PlayOneShot(sounds[0]);
        yield return new WaitForSeconds(1.5f);

        //よぉいとあなた消し開始を見えるように
        Ready.SetActive(false);
        You.SetActive(false);
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
        yield return new WaitForSeconds(WaitTime);

        if (!PushFlag)
        {
            Exclamation.SetActive(true);

            //!!が出た時の効果音
            audioSource.PlayOneShot(sounds[2]);

            ClickFlag = true;
        }
    }

    public async void PlayerWin()
    {
        Vector2 tmp; //場所を入れ替えるときの箱

        if (!PushFlag)
        {
            //マウスをpushしたフラグをtrue
            PushFlag = true;

            //　敵とプレイヤーの情報を取得
            GameObject target = GameObject.Find("Samurai");
            GameObject player = GameObject.Find("Player");

            // 場所の入れ替え
            tmp = target.transform.position;
            target.transform.position = player.transform.position;
            player.transform.position = tmp;

            //入れ替わり時の効果音
            audioSource.PlayOneShot(sounds[3]);

            //敵が倒れる
            target.transform.Rotate(0, 0, -90.0f);

            //Debug.Log("押した");

            await Task.Delay(1000);

            //勝を表示
            Win.SetActive(true);
            //勝った時の効果音
            audioSource.PlayOneShot(sounds[4]);

            await Task.Delay(1000);

            //次のレベルに移行(level5ならクリア画面)
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);

        }
    }

    public async void EnemyWin()
    {
        Vector2 tmp; //場所を入れ替えるときの箱

        if (!PushFlag) //もしマウスがpushされていなければ
        {
            //マウスをクリックしたフラグをtrue
            PushFlag = true;

            //　敵とプレイヤーの情報を取得
            GameObject target = GameObject.Find("Samurai");
            GameObject player = GameObject.Find("Player");

            // 場所の入れ替え
            tmp = target.transform.position;
            target.transform.position = player.transform.position;
            player.transform.position = tmp;

            //入れ替わり時の効果音
            audioSource.PlayOneShot(sounds[3]);

            //プレイヤーが倒れる
            player.transform.Rotate(0, 0, 90.0f);

            await Task.Delay(1000); //1000ms=1s

            //ゲームオーバー画面に移行
            SceneManager.LoadScene("EndlessResult");

        }
    }

    public async void PlayOtetsuki() //お手付き関数
    {
        if (!PushFlag)
        {
            PushFlag = true;

            OtetsukiCount = OtetsukiCount + 1;

            if (OtetsukiCount == 2) //２回でゲームオーバー
            {
                //お手付きを見えるように
                Otetsuki.SetActive(true);

                //お手付きの効果音
                audioSource.PlayOneShot(sounds[5]);

                await Task.Delay(2000);
                //GameOver画面に移行
                SceneManager.LoadScene("EndlessResult");
            }
            else
            {
                //お手付きを見えるように
                Otetsuki.SetActive(true);

                //お手付きの効果音
                audioSource.PlayOneShot(sounds[5]);

                //1000ms=1s
                await Task.Delay(2000);

                //今のシーンを再読み込み
                SceneManager.LoadScene(SceneManager.GetActiveScene().name);
            }
        }
    }

    public void ChangeFlag()
    {
        EnemyWinFlag = true;
    }
}
