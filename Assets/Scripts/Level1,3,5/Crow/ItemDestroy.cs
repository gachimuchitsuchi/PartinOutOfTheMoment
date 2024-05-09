using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using System.Threading.Tasks; //指定した時間を待つ機能

public class ItemDestroy : MonoBehaviour
{
    //gameObjectがあたる相手
    public string Player;

    //public string Samurai;
    //public string Ground;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    private async void OnCollisionEnter2D(Collision2D collision)
    {
        //衝突されたのがプレイヤーだったら
        if (collision.gameObject.name == Player)
        {
            //このゲームオブジェクトを消す
            Destroy(this.gameObject);

            //1000ms=1s
            await Task.Delay(1000);
            //ゲームオーバー画面に移行
            SceneManager.LoadScene("GameOver");
        }
        else
        {
            //このゲームオブジェクトを消す
            Destroy(this.gameObject);
        }
    }
}
