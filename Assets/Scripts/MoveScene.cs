using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using System.Threading.Tasks; //�w�肵�����Ԃ�҂@�\

public class MoveScene : MonoBehaviour
{
    public string SceneName; //�ړ���̃V�[��

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
        //�ړ��̃{�^���������Ƃ���ɉ�����SE��炷
        audioSource.PlayOneShot(sound);

        await Task.Delay(2000);

        SceneManager.LoadScene(SceneName);
    }
}
