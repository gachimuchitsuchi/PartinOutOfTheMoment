using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using System.Threading.Tasks; //指定した時間を待つ機能

public class Judge : MonoBehaviour
{
    //数秒後に表示させたり消したりする箱
    public GameObject Exclamation;
    public GameObject Otetsuki;
    public GameObject Ready;
    public GameObject Go;
    public GameObject Win;
    public GameObject You;
    private GameObject Crow;

    public AudioClip[] sounds;
    AudioSource audioSource;

    public string SceneName; //次のシーンの移動先の箱
    public static string NowSceneName; //リトライ用,今のゲームシーンを保存

    private static int OtetsukiCount = 0; //お手付きの回数

    private float WaitTime; //乱数取得用
    public float EnemyReactionRate = 3.0f; //敵の反応速度

    public bool ClickFlag = false; //プレイヤーがクリックできるようにするFlag,FallPoopにフラグを渡すためpublic
    public bool PushFlag = false; //プレイヤーがクリックを押したかを判断 poopに当たった時に使うためpublic
    private bool EnemyWinFlag = false;
    private bool StartFlag = false; //よぉい始めの時にUpdateを制限


    // Start is called before the first frame update
    void Start()
    {
        audioSource = GetComponent<AudioSource>();

        Crow = GameObject.Find("Crow");

        WaitTime = Random.Range(5, 11); //5以上11未満の乱数を生成

        StartCoroutine("ReadyGo");
        StartCoroutine("ChangeExclamationShow");

        //リトライ用に今のゲームシーンを取得
        NowSceneName = SceneManager.GetActiveScene().name;
    }

    // Update is called once per frame
    void Update()
    {
        if (StartFlag)
        {
            if (!ClickFlag)　//!!が出る前に
            {
                if (Input.GetMouseButtonDown(0))
                {
                    if (Crow)
                    {
                        Destroy(Crow);
                    }

                    PlayOtetsuki(); //お手付き関数を実行
                }
            }

            if (ClickFlag) //!!が出た後
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

                Invoke("ChangeFlag", EnemyReactionRate); //default3秒後に敵が勝つFlagをtrue

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

    IEnumerator ChangeExclamationShow() //待ち時間後に「!!」を見えるようにする関数
    {
        yield return new WaitForSeconds(1.5f + WaitTime);

        if (!PushFlag) //クリックがまだされていなければ
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
            SceneManager.LoadScene(SceneName);

        }
    }

    public async void EnemyWin()
    {
        Vector2 tmp; //場所を入れ替えるときの箱

        if(!PushFlag) //もしマウスがpushされていなければ
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
            SceneManager.LoadScene("GameOver");

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
                OtetsukiCount = 0;

                //お手付きを見えるように
                Otetsuki.SetActive(true);

                //お手付きの効果音
                audioSource.PlayOneShot(sounds[5]);

                await Task.Delay(2000);
                //GameOver画面に移行
                SceneManager.LoadScene("GameOver");
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
