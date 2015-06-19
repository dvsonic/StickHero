using UnityEngine;
using System.Collections;

public class BananaSpawn : MonoBehaviour {

	// Use this for initialization
    public GameObject bananaPrefab;
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    public void CreateBanana()
    {
        GameObject obj = Instantiate(bananaPrefab, transform.position, transform.rotation) as GameObject;
        obj.transform.localScale = new Vector3(1.2f, 1.2f, 1);
    }
}
