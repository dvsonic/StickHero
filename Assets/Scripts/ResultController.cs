using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class ResultController : MonoBehaviour {

	// Use this for initialization
    public Text tfScore;
    public Text tfScoreBest;
    public Text tfReplay;
    public Text tfQuit;
    public Text tfShare;
    public Text tfEnd;
    public Text tfRecord;
    public GameObject Record;
    public Button btnHome;
    public Button btnRank;
    public Button btnDiscuss;
    public Button btnRestart;
    public const string BEST_SCORE = "BEST_SCORE";

    void Awake()
    {
        if(tfRecord)
            tfRecord.text = GameData.getLanguage("record");
#if UNITY_ANDROID
        Destroy(btnRank.gameObject);
        Destroy(btnDiscuss.gameObject);
        btnHome.GetComponent<RectTransform>().anchoredPosition3D += new Vector3(120,0,0);
        btnRestart.GetComponent<RectTransform>().anchoredPosition3D -= new Vector3(120,0,0);
#endif
    }
	void Start () {
        tfScore.text = GameData.getLanguage("score")+"\n"+GameData.score.ToString();
        int best = PlayerPrefs.GetInt(BEST_SCORE);
        if (GameData.score >= best)
        {
            PlayerPrefs.SetInt(BEST_SCORE, GameData.score);
            best = GameData.score;
            Record.SetActive(true);
        }
        else
        {
            Record.SetActive(false);
        }
        tfScoreBest.text = GameData.getLanguage("bestscore")+"\n" + best.ToString();

        SocialManager.GetInstance().ReportScore("20001", GameData.score);

	}
	
	// Update is called once per frame
	void Update () {
	
	}

    public void Restart()
    {
        Canvas canvas = GameObject.FindObjectOfType(typeof(Canvas)) as Canvas;
        if (canvas)
            canvas.SendMessage("DestroyAD", SendMessageOptions.DontRequireReceiver);
        GameData.BGIndex = Random.Range(1, GameData.BGMax);
        Application.LoadLevel("Main");
    }

    public void Quit()
    {
        Application.Quit();
    }

    public void Login()
    {
        Canvas canvas = GameObject.FindObjectOfType(typeof(Canvas)) as Canvas;
        if (canvas)
            canvas.SendMessage("DestroyAD", SendMessageOptions.DontRequireReceiver);
        Application.LoadLevel("Start");
    }

    public void Rank()
    {
        SocialManager.GetInstance().ShowLeaderboard();
    }

    public void ToDiscuss()
    {
#if UNITY_IPHONE
        Application.OpenURL("https://itunes.apple.com/app/id1011671335?mt=8");
#endif
    }

    public AudioSource soundClick;
    public void PlaySoundClick()
    {
        if (soundClick)
            soundClick.Play();
    }
}
