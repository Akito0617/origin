using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using DG.Tweening;
using NCMB;

public enum GAMEMODE { EASY, NORMAL, HARD }

public class MainManager : MonoBehaviour {

    public static GAMEMODE selectGameMode;
    public class Params{
        int attendance;
        int report;
        int midExam;
        int finalExam;
        public int requireGrade; //1,2,3,4,5 = 秀,優,良,可,不可とする
        public Params(GAMEMODE g,int a, int r, int m, int f){
            attendance = a;
            report = r;
            midExam = m;
            finalExam = f;
            int sum = a + r + m + f;
            switch(g){
                case GAMEMODE.EASY:
                    if (sum < 60)
                        requireGrade = 5;
                    else if (sum <= 100)
                        requireGrade = 1;
                    break;
                case GAMEMODE.NORMAL:
                    if (sum < 60)
                        requireGrade = 5;
                    else if (sum < 80)
                        requireGrade = 3;
                    else if (sum <= 100)
                        requireGrade = 1;
                    break;
                case GAMEMODE.HARD:
                    if (sum < 60)
                        requireGrade = 5;
                    else if (sum < 70)
                        requireGrade = 4;
                    else if (sum < 80)
                        requireGrade = 3;
                    else if (sum < 90)
                        requireGrade = 2;
                    else if (sum <= 100)
                        requireGrade = 1;
                    break;
            }
        }
    }
    Params studentParams;

    public static int score;
    int combo;
    int chack;

    public TextMeshProUGUI t_combo;
    public TextMeshProUGUI t_score;
    public Text t_grade;
    public TextMeshProUGUI  t_stuGrade;
    public TextMeshProUGUI t_rule;
    public TextMeshProUGUI t_time;
    public Text textCountdown;
    public GameObject maru;
    public GameObject batu;
    public Image gauge;
    public ParticleSystem syuEffect;
    public ParticleSystem yuuEffect;
    public ParticleSystem ryoEffect;
    public ParticleSystem kaEffect;
    public ParticleSystem fukaEffect;
    public SpriteRenderer[] renderers;
    public Transform[] transforms;
    private Sequence seq;
    //音声ファイル格納用変数
    public AudioClip sound01;
    public AudioClip sound02;


    GameObject original;
    GameObject copied;

    float time;

	// Use this for initialization
	void Start () {
        StartCoroutine(CountdownCoroutine());
        gauge.fillAmount = 0;
        score = 0;
        t_score.text = "人望 : "+ score;
        combo = 0;
        t_combo.text = "コンボ : " + combo;
        int aa = Random.Range(0, 20);
        int ra = Random.Range(0, 20);
        int ma = Random.Range(0, 30);
        int fa = Random.Range(0, 30);
        t_stuGrade.text = string.Format("出席        : <size=80>{0}</size>\nレポート: <size=80>{1}</size>\n" +
                                        "中間        : <size=80>{2}</size>\n期末        : <size=80>{3}</size>", aa, ra, ma, fa);
        if (selectGameMode == GAMEMODE.EASY) t_rule.text = "各項目の合計が\n60~秀優良可\n~59 不可";
        else if (selectGameMode == GAMEMODE.NORMAL) t_rule.text = "各項目の合計が\n80~秀優\n60~良可\n~59 不可";
        else if (selectGameMode == GAMEMODE.HARD) t_rule.text = "各項目の合計が\n90~秀    80~優\n70~良    60~可\n~59 不可";
        studentParams = new Params(selectGameMode, aa, ra, ma, fa);
        time = 60f+2.4f;
        t_time.text = "残り時間 : " + 60;
        maru.SetActive(false);
        batu.SetActive(false);
        chack = 0;
	}
    
    // Update is called once per frame
    void Update()
    {
        
        time -= Time.deltaTime;
        t_time.text = "残り時間 : " + (int)time;
        if(time < 0 && chack == 0){
            naichilab.RankingLoader.Instance.SendScoreAndShowRanking(score);
            chack = 1;
        }else if(time > 60){
            t_time.text = "残り時間 : " + 60;
        }

        int input = 0;
        if (selectGameMode == GAMEMODE.EASY){
            if (Input.GetKeyDown(KeyCode.A)) { input = 1; t_grade.text = "秀"; original = (GameObject)Resources.Load("Prefabs/syuu");}
            else if (Input.GetKeyDown(KeyCode.S)) { input = 1; t_grade.text = "優"; original = (GameObject)Resources.Load("Prefabs/yuu");}
            else if (Input.GetKeyDown(KeyCode.D)) { input = 1; t_grade.text = "良"; original = (GameObject)Resources.Load("Prefabs/ryo");}
            else if (Input.GetKeyDown(KeyCode.F)) { input = 1; t_grade.text = "可"; original = (GameObject)Resources.Load("Prefabs/ka");}
            else if (Input.GetKeyDown(KeyCode.Space)) { input = 5; t_grade.text = "不可"; original = (GameObject)Resources.Load("Prefabs/fuka");}
        }
        if (selectGameMode == GAMEMODE.NORMAL)
        {
            if (Input.GetKeyDown(KeyCode.A)) { input = 1; t_grade.text = "秀"; original = (GameObject)Resources.Load("Prefabs/syuu");}
            else if (Input.GetKeyDown(KeyCode.S)) { input = 1; t_grade.text = "優"; original = (GameObject)Resources.Load("Prefabs/yuu");}
            else if (Input.GetKeyDown(KeyCode.D)) { input = 3; t_grade.text = "良"; original = (GameObject)Resources.Load("Prefabs/ryo");}
            else if (Input.GetKeyDown(KeyCode.F)) { input = 3; t_grade.text = "可"; original = (GameObject)Resources.Load("Prefabs/ka");}
            else if (Input.GetKeyDown(KeyCode.Space)) { input = 5; t_grade.text = "不可"; original = (GameObject)Resources.Load("Prefabs/fuka");}
        }
        if (selectGameMode == GAMEMODE.HARD)
        {
            if (Input.GetKeyDown(KeyCode.A)) { input = 1; t_grade.text = "秀"; original = (GameObject)Resources.Load("Prefabs/syuu");}
            else if (Input.GetKeyDown(KeyCode.S)) { input = 2; t_grade.text = "優"; original = (GameObject)Resources.Load("Prefabs/yuu");}
            else if (Input.GetKeyDown(KeyCode.D)) { input = 3; t_grade.text = "良"; original = (GameObject)Resources.Load("Prefabs/ryo");}
            else if (Input.GetKeyDown(KeyCode.F)) { input = 4; t_grade.text = "可"; original = (GameObject)Resources.Load("Prefabs/ka");}
            else if (Input.GetKeyDown(KeyCode.Space)) { input = 5; t_grade.text = "不可"; original = (GameObject)Resources.Load("Prefabs/fuka");}
        }


        if (time > 60 || time < 0)
        {
            input = 0;
            t_time.text = "残り時間 : " + 0;
        }
        if(input != 0){
            DOTween.KillAll();
            SpriteRenderer tmpRenderer = renderers[0];
            renderers[0].sortingOrder = -renderers.Length;
            for (int i = 0; i < renderers.Length;i++){
                if (i == 0){
                    renderers[0].transform.position = transforms[0].position;
                    renderers[0].transform.localScale = transforms[0].localScale;
                    seq.Join(renderers[0].transform.DOMove(transforms[renderers.Length - 1].position, 0.5f));
                    seq.Join(renderers[0].transform.DOScale(transforms[renderers.Length - 1].localScale, 0.5f));
                } 
                else{
                    renderers[i].transform.position = transforms[i].position;
                    renderers[i].transform.localScale = transforms[i].localScale;
                    renderers[i].sortingOrder = -(i - 1);
                    seq.Join(renderers[i].transform.DOMove(transforms[i - 1].position, 0.5f));
                    seq.Join(renderers[i].transform.DOScale(transforms[i - 1].localScale, 0.5f));
                    renderers[i - 1] = renderers[i];
                }
            }
            renderers[renderers.Length - 1] = tmpRenderer;
            seq.Play();
            //effect
            switch (input)
            {
                case 1:
                    syuEffect.transform.position = new Vector3(0, -3f, 0);
                    //ryoEffect.transform.DOMove(transforms[transforms.Length - 1].position, 0.5f);
                    syuEffect.Emit(4);
                    break;
                case 2:
                    yuuEffect.transform.position = new Vector3(0, -3f, 0);
                    //ryoEffect.transform.DOMove(transforms[transforms.Length - 1].position, 0.5f);
                    yuuEffect.Emit(2);
                    break;
                case 3:
                    ryoEffect.transform.position = new Vector3(0, -3f, 0);
                    //ryoEffect.transform.DOMove(transforms[transforms.Length - 1].position, 0.5f);
                    ryoEffect.Emit(2);
                    break;
                case 4:
                    kaEffect.transform.position = new Vector3(0, -3f, 0);
                    //kaEffect.transform.DOMove(transforms[transforms.Length - 1].position, 0.5f);
                    kaEffect.Emit(8);
                    break;
                case 5:
                    fukaEffect.transform.position = new Vector3(0, -3f, 0);
                    //fukaEffect.transform.DOMove(transforms[transforms.Length - 1].position, 0.5f);
                    fukaEffect.Emit(1);
                    break;
            }
            //成績スタンプ生成処理
            Destroy(GameObject.Find("a"));
            copied = Object.Instantiate(original) as GameObject;
            //オリジナルを変更してしまうとプレハブの座標が変わってしまう
            copied.transform.Translate(0, -3f , 0);
            copied.name = "a";

            copied.transform.DOPunchScale(new Vector3(0.5f, 0.5f, 1), 0.5f);
            copied.GetComponentInChildren<SpriteRenderer>().DOFade(0f, 0.5f).SetDelay(0.5f);

            if (input == studentParams.requireGrade){
                //scoreとcomboを変更
                StartCoroutine(maruCoroutine());
                GetComponent<AudioSource>().PlayOneShot(sound01);
                score += 100+combo*30;
                combo += 1;
            }else{
                //scoreとcomboを変更
                StartCoroutine(batuCoroutine());
                GetComponent<AudioSource>().PlayOneShot(sound02);
                score -= 150;
                combo = 0;
            }
            int aa = Random.Range(0, 10 + 5); if (aa > 10) aa -= 5;
            int ra = Random.Range(0, 10 + 5); if (ra > 10) ra -= 5;
            int ma = Random.Range(0, 30+ 15); if (ma > 30) ma -= 15;
            int fa = Random.Range(0, 50+ 25); if (fa > 50) fa -= 25;
            t_stuGrade.text = string.Format("出席        : <size=80>{0}</size>\nレポート: <size=80>{1}</size>\n" +
                                        "中間        : <size=80>{2}</size>\n期末        : <size=80>{3}</size>", aa, ra, ma, fa);
            studentParams = new Params(selectGameMode, aa, ra, ma, fa);
        }
        t_score.text = "人望 : " + score;
        t_combo.text = "コンボ : " + combo;
    }

    IEnumerator CountdownCoroutine()
    {
        textCountdown.gameObject.SetActive(true);

        textCountdown.text = "3";
        yield return new WaitForSeconds(0.6f);

        textCountdown.text = "2";
        yield return new WaitForSeconds(0.6f);

        textCountdown.text = "1";
        yield return new WaitForSeconds(0.6f);
        Debug.Log("A");
        textCountdown.text = "START!";
        yield return new WaitForSeconds(0.6f);

        textCountdown.text = "";
        textCountdown.gameObject.SetActive(false);
    }

    IEnumerator maruCoroutine(){
        maru.SetActive(true);
        yield return new WaitForSeconds(0.5f);
        maru.SetActive(false);
    }
    IEnumerator batuCoroutine()
    {
        batu.SetActive(true);
        yield return new WaitForSeconds(0.5f);
        batu.SetActive(false);
    }
}
