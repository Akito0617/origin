using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NCMB;

public class test : MonoBehaviour {

	public void OnClick(){
        //naichilab.UnityRoomTweet.Tweet ("ゲームID", "人望を"+(スコア).ToString()+"獲得しました", "unityroom");
        naichilab.UnityRoomTweet.Tweet("KGJ20181104", ScoreManager.rank + "人望を" + MainManager.score.ToString() + "獲得しました(" + MainManager.selectGameMode + ")" , "unityroom");
	}
}
