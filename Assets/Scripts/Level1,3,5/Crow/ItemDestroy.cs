using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using System.Threading.Tasks; //�w�肵�����Ԃ�҂@�\

public class ItemDestroy : MonoBehaviour
{
    //gameObject�������鑊��
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
        //�Փ˂��ꂽ�̂��v���C���[��������
        if (collision.gameObject.name == Player)
        {
            //���̃Q�[���I�u�W�F�N�g������
            Destroy(this.gameObject);

            //1000ms=1s
            await Task.Delay(1000);
            //�Q�[���I�[�o�[��ʂɈڍs
            SceneManager.LoadScene("GameOver");
        }
        else
        {
            //���̃Q�[���I�u�W�F�N�g������
            Destroy(this.gameObject);
        }
    }
}
