using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using System.Threading.Tasks; //指定した時間を待つ機能

public class MoveScene : MonoBehaviour
{
    public string SceneName; //移動先のシーン

    public AudioClip sound;
    AudioSource audioSource;


    // Start is called before the first frame update
    void Start()
    {
        audioSource = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
       
    }
    public async void OnClick()
    {
        //移動のボタンを押すとそれに応じたSEを鳴らす
        audioSource.PlayOneShot(sound);

        await Task.Delay(2000);

        SceneManager.LoadScene(SceneName);
    }
}
