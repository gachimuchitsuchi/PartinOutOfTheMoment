using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EndlessResult : MonoBehaviour
{
    public Text WinCountText;
    public Text ReactionRateText;
    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
       WinCountText.text = "Ÿ—˜‰ñ”F" + EndlessJudge.WinCount.ToString();
       ReactionRateText.text = "”½‰‘¬“xF" + EndlessJudge.EnemyReactionRate.ToString();
    }
}
