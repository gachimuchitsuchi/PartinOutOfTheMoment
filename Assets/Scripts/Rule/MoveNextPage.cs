using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveNextPage : MonoBehaviour
{
    public GameObject NextPage;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Onclick()
    {
        GameObject NowPage = this.transform.parent.gameObject;
        NowPage.SetActive(false);
        NextPage.SetActive(true);
    }
}
