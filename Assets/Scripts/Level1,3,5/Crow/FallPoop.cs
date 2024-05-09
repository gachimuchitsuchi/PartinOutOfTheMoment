using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FallPoop : MonoBehaviour
{
    [SerializeField] GameObject J; //Judgeスクリプトから値を取得するための箱

    public GameObject CreatePrefab; //生成するプレファブの箱

    //SEを鳴らすもの
    public AudioClip CrowSound;
    AudioSource audioSource;

    public int GameLevel; //level3かlevel5の区別用


    // Start is called before the first frame update
    void Start()
    {
        audioSource = GetComponent<AudioSource>();

        if (GameLevel == 3) //level3のステージだったら
        {
            Invoke("Create", 5); //5秒後に一回poopを生成
        }
        if(GameLevel == 5) //level5のステージだったら
        {
            InvokeRepeating("Create", 5, 2); //５秒後に２秒毎でpoopを生成
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Create()
    {
        GameObject poop; //prefabを入れる箱

        if (!J.GetComponent<Judge>().ClickFlag) //プレイヤーがクリックしてはダメな時
        {
            //カラスの鳴き声
            audioSource.PlayOneShot(CrowSound);

            //プレイヤーを探す
            GameObject player = GameObject.Find("Player");

            //親オブジェクトのCrowを探す
            GameObject crow = GameObject.Find("Crow");

            //プレイヤーの真上にpoopを生成
            poop = Instantiate(CreatePrefab) as GameObject;
            poop.transform.parent = crow.transform;
            poop.transform.position = new Vector3(player.transform.position.x, 5, 0);
        }
    }
}
