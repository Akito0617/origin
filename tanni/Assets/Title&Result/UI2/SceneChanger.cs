using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SceneChanger : MonoBehaviour {

	[SerializeField] GameObject enmacho;
	[SerializeField] GameObject title;

	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
	}

	public void OnClick(int mode){
        if (mode == 2)
            MainManager.selectGameMode = GAMEMODE.HARD;
        else if (mode == 1)
            MainManager.selectGameMode = GAMEMODE.NORMAL;
        else if (mode == 0)
            MainManager.selectGameMode = GAMEMODE.EASY;
       StartCoroutine(SceneLoad());
	}

	private IEnumerator SceneLoad(){
		enmacho.GetComponent<Rigidbody2D>().isKinematic = false;
		yield return new WaitForSeconds(1.5f);
		SceneManager.LoadScene("Main");
	}
}
