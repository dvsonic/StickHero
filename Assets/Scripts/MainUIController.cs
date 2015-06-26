using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System.Collections.Generic;
public class MainUIController : MonoBehaviour {


    public Text TFScore;
    public Text TFBanaer;
    void Awake()
    {
        
    }
	// Use this for initialization
	void Start () {
        GameData.score = 0;
        GameData.banana = 0;
        Camera.main.GetComponent<AudioSource>().clip = Resources.Load("music/bgm" + GameData.BGIndex) as AudioClip;
        Camera.main.GetComponent<AudioSource>().Play();
	}
	
	// Update is called once per frame
	void Update () {
        TFScore.text = GameData.score.ToString();
        if (TFBanaer)
            TFBanaer.text = GameData.banana.ToString();

        if (Input.GetKeyDown(KeyCode.Escape))
            Application.Quit();
	}
}
