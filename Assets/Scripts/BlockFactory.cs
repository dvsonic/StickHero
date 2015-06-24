using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class BlockFactory : MonoBehaviour {

    public BananaSpawn BananaSpawner;
    public GameObject[] blockList;
    public int minDistant;
    public int maxDistant;
    public float minSize;
    public float maxSize;
    public int blockStart;
    public int blockEnd;
	// Use this for initialization
	void Start () {
        
    }
	
	// Update is called once per frame
	void Update () {
	
	}

    /*public GameObject CreateBlock(Transform lastBlock)
    {
        Object prefab = blockList[Random.Range(0, blockList.Length-1)];
        //最小距离不能小于两块石头的大小
        float targetScale = Random.Range(minSize, maxSize);
        float blockLimit = Mathf.Max(minDistant, GetBlockWidth(prefab as GameObject)*targetScale + GetBlockWidth(lastBlock.gameObject) + 0.1f);
        GameObject obj = Instantiate(prefab, lastBlock.position + new Vector3(Random.Range(blockLimit, maxDistant), 0), new Quaternion()) as GameObject;
        Transform blockBG = obj.transform.Find("BlockBG");
        blockBG.localScale = new Vector3(targetScale, blockBG.localScale.y, 1);
        obj.transform.Find("Bonus").gameObject.SetActive(true);

        if (BananaSpawner)
        {
            float spawnMin = GetBlockWidth(lastBlock.gameObject)+lastBlock.transform.position.x+0.5f;
            float spawnMax = +obj.transform.position.x - GetBlockWidth(obj)-0.5f;
            if (spawnMax > spawnMin)
            {
                BananaSpawner.transform.position = new Vector3(Random.Range(spawnMin, spawnMax), BananaSpawner.transform.position.y);
                BananaSpawner.SendMessage("CreateBanana");
            }

        }
        return obj;
    }*/

    public GameObject CreateBlock(Transform lastBlock)
    {
        int index = Random.Range(0, blockList.Length);
        Object prefab = blockList[index];
        GameObject obj = Instantiate(prefab, lastBlock.transform.position, new Quaternion()) as GameObject;
        Transform blockBG = obj.transform.Find("BlockBG");
        blockBG.gameObject.GetComponent<SpriteRenderer>().sprite = Resources.Load("images/bg" + GameData.BGIndex + "/Block" + (index+1), typeof(Sprite)) as Sprite;
        float blockLimit = Mathf.Max(minDistant, GetBlockWidth(obj as GameObject) +GetBlockWidth(lastBlock.gameObject) + 0.1f);
        obj.transform.position += new Vector3(Random.Range(blockLimit, maxDistant), 0);
        obj.transform.Find("Bonus").gameObject.SetActive(true);

        if (BananaSpawner)
        {
            float spawnMin = GetBlockWidth(lastBlock.gameObject) + lastBlock.transform.position.x + 0.5f;
            float spawnMax = +obj.transform.position.x - GetBlockWidth(obj) - 0.5f;
            if (spawnMax > spawnMin)
            {
                BananaSpawner.transform.position = new Vector3(Random.Range(spawnMin, spawnMax), BananaSpawner.transform.position.y);
                BananaSpawner.SendMessage("CreateBanana");
            }

        }
        return obj;
    
    }

    private float GetBlockWidth(GameObject block)
    {
        GameObject blockBG = block.transform.Find("BlockBG").gameObject;
        SpriteRenderer sr = blockBG.GetComponent<SpriteRenderer>();
        return sr.bounds.extents.x;
    }
}
