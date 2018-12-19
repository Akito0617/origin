using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
//akito 3310点
//
public class ScoreManager : MonoBehaviour {
    
	[SerializeField] TextMeshProUGUI shogoLabel;
	private int[] threshold = {2000,0,-2500,-10000};
    public static string rank;

	void Start () {
		//shogoLabel.text = shogo(スコア);
        shogoLabel.text = shogo(MainManager.score);
        rank = shogo(MainManager.score);
	}

	private string shogo(int score){
		if(score >= threshold[0]){
			return "「擊墜王」";
		}
		else if(score >= threshold[1]){
			return "「可もなく不可もなく」";
		}
        else if (score >= threshold[2])
        {
            return "「新任教授」";
        }
        else if (score >= threshold[3])
        {
            return "「狂授」";
        }
		else if(score < threshold[3]){
			return "「大菩薩」";
		}
		else{
			return null;
		}
	}
}
