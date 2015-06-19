using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class StartController : MonoBehaviour {

	// Use this for initialization
    public Text debugText;
    public AudioSource soundClick;
	void Start () {
        SocialManager.GetInstance().Start();
    }
	
	// Update is called once per frame
	void Update () {
	
	}

    public void StartGame()
    {
        Application.LoadLevel("Main");
    }

    public void PlaySoundClick()
    {
        if (soundClick)
            soundClick.Play();
    }
}
