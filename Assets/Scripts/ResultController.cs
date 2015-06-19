﻿using UnityEngine;
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
        if (tfEnd)
            tfEnd.text = GameData.getLanguage().SearchForChildByTag("end").Text;
        if (tfShare)
            tfShare.text = GameData.getLanguage().SearchForChildByTag("share").Text;
        if(tfRecord)
            tfRecord.text = GameData.getLanguage().SearchForChildByTag("record").Text;
#if UNITY_ANDROID
        Destroy(btnRank.gameObject);
        Destroy(btnDiscuss.gameObject);
        btnHome.GetComponent<RectTransform>().anchoredPosition3D += new Vector3(120,0,0);
        btnRestart.GetComponent<RectTransform>().anchoredPosition3D -= new Vector3(120,0,0);
#endif
    }
	void Start () {
        tfScore.text = GameData.getLanguage().SearchForChildByTag("score").Text+"\n"+GameData.score.ToString();
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
        tfScoreBest.text = GameData.getLanguage().SearchForChildByTag("bestscore").Text+"\n"+best.ToString();

        SocialManager.GetInstance().ReportScore("10010", GameData.score);

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
        string APP_ID = GameData.getLanguage().Attribute("appid");
        Debug.Log(APP_ID);
        var url = string.Format(
            "itms-apps://ax.itunes.apple.com/WebObjects/MZStore.woa/wa/viewContentsUserReviews?type=Purple+Software&id={0}",
            APP_ID);
        Application.OpenURL("https://itunes.apple.com/app/up!up!/id1002324229?l=zh&ls=1&mt=8");
#endif
    }

    public AudioSource soundClick;
    public void PlaySoundClick()
    {
        if (soundClick)
            soundClick.Play();
    }
}
