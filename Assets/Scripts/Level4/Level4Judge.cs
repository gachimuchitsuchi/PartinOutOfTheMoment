using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using System.Threading.Tasks; //指定した時間を待つ機能

public class Level4Judge : MonoBehaviour
{
    /*Dummy用の関数を追加*/



    public GameObject Exclamation; //数秒後に表示させる!!マークの箱
    public GameObject Otetsuki;
    public GameObject Ready;
    public GameObject Go;
    public GameObject Win;
    public GameObject You;
    public GameObject DummyPrefab;
    private GameObject Dummy; //生成したDummyを一時保存する箱
    public GameObject SplitDummyPrefab;

    public AudioClip[] sounds;
    AudioSource audioSource;

    public string SceneName;

    private static int OtetsukiCount = 0;

    public float WaitTime; //乱数取得用,FallDummyに値を渡すためにpublic

    private bool ClickFlag = false; //プレイヤーがクリックできるようにするFlag
    private bool PushFlag = false; //プレイヤーがクリックを押したかを判断
    private bool EnemyWinFlag = false; //敵が勝つFlag
    private bool StartFlag = false;


    // Start is called before the first frame update
    void Start()
    {
        audioSource = GetComponent<AudioSource>();

        WaitTime = Random.Range(5, 16); //5以上16未満の乱数を生成

        StartCoroutine("ReadyGo");
        StartCoroutine("ChangeExclamationShow");
        StartCoroutine("EnemyPosChange");
        StartCoroutine("DummyCreate");

        //リトライ用に今のゲームシーンをNowSceneNameに渡す
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

            if (ClickFlag) //クリック可能になったら
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

    IEnumerator ChangeExclamationShow() //待ち時間後に!!を見えるようにする関数
    {
        yield return new WaitForSeconds(1.5f + WaitTime);

        if (!PushFlag)
        {
            Exclamation.SetActive(true);

            //!!が出た時の効果音
            audioSource.PlayOneShot(sounds[2]);

            ClickFlag = true;
        }
    }

    IEnumerator EnemyPosChange() //敵がダミーに引っかかる
    {
        yield return new WaitForSeconds(WaitTime+4.5f);
        ChangeDummy("Samurai");
        PushFlag = false;
    }

    IEnumerator DummyCreate()
    {
        yield return new WaitForSeconds(WaitTime - 1.0f);
        //画面中央にDummyが落ちてくる
        Dummy = Instantiate(DummyPrefab);
        Dummy.transform.position = new Vector2(0, 5);
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

            //次のレベルに移行
            SceneManager.LoadScene(SceneName);

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

    public void ChangeDummy(string name)
    {
        Vector2 tmp; //場所を入れ替えるときの箱

        if (!PushFlag) //もしマウスがpushされていなければ
        {
            //マウスをクリックしたフラグをtrue
            PushFlag = true;

            //　敵とプレイヤーの情報を取得
            GameObject target = GameObject.Find(name);

            // 場所の入れ替え
            tmp = target.transform.position;
            target.transform.position = Dummy.transform.position;
            Dummy.transform.position = tmp;

            //入れ替わり時の効果音
            audioSource.PlayOneShot(sounds[3]);

            //このゲームオブジェクトを消す
            Destroy(Dummy);

            //Dummyが切られたオブジェクトを取得し生成
            GameObject splitDummy = Instantiate(SplitDummyPrefab);
            splitDummy.transform.position = new Vector2(0, 2);

            // splitDummyのrigidbodyを取得
            Rigidbody2D rb = splitDummy.GetComponent<Rigidbody2D>(); 

            if (name == "Samurai")
            {
                Vector2 force = new Vector2(5.0f, 2.0f);  // 力を設定
                rb.AddForce(force, ForceMode2D.Impulse);
            }
            else
            {
                Vector2 force = new Vector2(-5.0f, 2.0f);  // 力を設定
                rb.AddForce(force, ForceMode2D.Impulse);
            }
        }
    }

    public void ChangeFlag()
    {
        EnemyWinFlag = true;
    }
}
