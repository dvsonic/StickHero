using UnityEngine;
using System.Collections;

public class BGImage : MonoBehaviour {

	// Use this for initialization
    public GameObject[] BGList;
    public string imageName;
    public int BGHeight = 1280;//用于底对齐
    public bool fixBottom;
	void Start () {
        Vector3 loadBGExtents = new Vector3();
        for (int i = 0; i < BGList.Length; i++)
        {
            loadBGExtents = LoadBG(BGList[i]);
        }
        if (fixBottom)
            gameObject.transform.position = new Vector3(0, (loadBGExtents.y - Camera.main.orthographicSize), 0);
        else
            gameObject.transform.position = new Vector3(0, (loadBGExtents.y - BGHeight / 200.0f), 0);
        if(BGList.Length == 3)
        {
            BGList[0].transform.localPosition = new Vector3(-loadBGExtents.x * 2, 0,0);
            BGList[2].transform.localPosition = new Vector3(loadBGExtents.x * 2, 0, 0);
        }

	}

    private Vector3 LoadBG(GameObject bg)
    {
        SpriteRenderer sr = bg.GetComponent<SpriteRenderer>();
        Sprite image = Resources.Load("images/bg" + GameData.BGIndex+"/"+imageName, typeof(Sprite)) as Sprite;
        if (image)
            sr.sprite = image;
        else
        {
            sr.sprite = null;
            Debug.Log("error load:" + imageName);
        }
        return sr.bounds.extents;
    }

    // Update is called once per frame
    void Update()
    {
	
	}
}
