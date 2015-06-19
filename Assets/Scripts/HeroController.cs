using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

public class HeroController : MonoBehaviour {

    public Transform bgFar;
    public Transform bgNear;
    public Transform bgFront;
    public GameObject initBlock;
    private GameObject targetBlock;
    public GameObject stick;
    public float speed = 3.0f;
    private BlockFactory bf;
    public AudioSource cross;
    public AudioSource bonous;
    public Text GameTip;
    public enum HERO_STATE { IDLE, STAMP, PUSH, RUN };//用于控制动画
    void Awake()
    {
        GameTip.text = GameData.getLanguage().SearchForChildByTag("guide").Text;
    }
	// Use this for initialization
	void Start () {
        initBlock.transform.Find("BlockBG").gameObject.GetComponent<SpriteRenderer>().sprite = Resources.Load("images/bg" + GameData.BGIndex + "/Block1", typeof(Sprite)) as Sprite;
        EventManager.getInstance().addEventListener(Event_Name.STICK_FALL_END, toCross);
        bf = GameObject.Find("BlockFactory").GetComponent<BlockFactory>();
        hitedList = new List<GameObject>();
	}
	
	// Update is called once per frame
	void Update () {
#if UNITY_EDITOR
        if (Input.GetMouseButton(0))
        {
            if (GameTip)
                GameTip.enabled = false;
            stick.SendMessage("SetGrowing", true);
            SetHeroState(HERO_STATE.STAMP);
        }
        else if (Input.GetMouseButtonUp(0))
        {
            stick.SendMessage("SetGrowing", false);
            SetHeroState(HERO_STATE.PUSH);
        }
#else
        foreach (Touch t in Input.touches)
        {
            if (t.phase == TouchPhase.Began)
            {
                if (GameTip)
                    GameTip.enabled = false;
                stick.SendMessage("SetGrowing", true);
                SetHeroState(HERO_STATE.STAMP);
                break;
            }
            else if (t.phase == TouchPhase.Ended)
            {
                stick.SendMessage("SetGrowing", false);
                SetHeroState(HERO_STATE.PUSH);
                break;
            }
        }
#endif
    }

    public void SetHeroState(HERO_STATE state)
    {
        Animator amt = GetComponent<Animator>();
        switch (state)
        {
            case HERO_STATE.IDLE:
                amt.SetBool("isStamp", false);
                amt.SetBool("isPush", false);
                amt.SetBool("isRun", false);
                break;
            case HERO_STATE.STAMP:
                amt.SetBool("isStamp", true);
                break;
            case HERO_STATE.PUSH:
                amt.SetBool("isPush", true);
                break;
            case HERO_STATE.RUN:
                if (cross)
                    cross.Play();
                amt.SetBool("isRun", true);
                break;

        }
    }

    void FixedUpdate()
    {
        if (_isCrossing)
        {
            transform.position += new Vector3(speed * Time.deltaTime, 0, 0);
             updateBG();
        }
        else if (_isMove2Target)
        {
            if (_endX > transform.position.x)
            {
                transform.position += new Vector3(speed * Time.deltaTime, 0, 0);
                updateBG();
            }
            else
            {
                SetHeroState(HERO_STATE.IDLE);
                transform.position = new Vector3(_endX, transform.position.y);
                float cameraTargetPos = _endX + _initOffset;
                float followDistant = cameraTargetPos - Camera.main.transform.position.x;
                Camera.main.SendMessage("Follow", followDistant);
                _isMove2Target = false;
                CreateNextBlock();
            }
        }
    }

    private void updateBG()
    {
        if(bgFar)
            bgFar.position -= new Vector3(0.1f*speed * Time.deltaTime, 0, 0);
        if(bgNear)
            bgNear.position -= new Vector3(0.2f*speed*Time.deltaTime, 0, 0);
        if (bgFront)
            bgFront.position -= new Vector3(0.6f*speed * Time.deltaTime, 0);
    }

    private void CreateNextBlock()
    {
        GameData.score++;
        stick.SendMessage("Reset",targetBlock );
        targetBlock = bf.CreateBlock(targetBlock.transform);
        stick.SendMessage("SetTargetBlock", targetBlock);
        _endX = GetEndPosOnBlock(targetBlock);
        if (cross)
            cross.Stop();

    }

    private bool _isCrossing;
    private bool _isMove2Target;//下了棍子,还没走到终点
    private bool _isOnBlock;
    private float _startX;
    private float _endX;
    private float _initOffset;
    private void toCross()
    {
        if (stick)
        {
            SetHeroState(HERO_STATE.RUN);
            _isCrossing = true;
            _startX = transform.position.x;
            _endX = GetEndPosOnBlock(targetBlock);
        }
    }
    private List<GameObject> hitedList;
    void OnCollisionEnter2D(Collision2D coll)
    {
        if (hitedList.IndexOf(coll.gameObject) >= 0)
            return;
        hitedList.Add(coll.gameObject);
        if (initBlock && coll.gameObject == initBlock.transform.Find("BlockBG").gameObject)
        {
            _startX = GetEndPosOnBlock(initBlock);
            transform.localPosition = new Vector3(_startX, transform.position.y, transform.position.z);
            targetBlock = bf.CreateBlock(initBlock.transform);
            _endX = GetEndPosOnBlock(targetBlock);
            stick.SendMessage("SetTargetBlock", targetBlock);
            stick.SendMessage("Reset", initBlock);
            _initOffset = -transform.position.x;
            SetHeroState(HERO_STATE.IDLE);
        }
        else if (coll.gameObject.tag == "block")//需要离开棍子
        {
            _isOnBlock = true;
            if (!_isCrossing)
                _isMove2Target = true;
        }
        else if (coll.gameObject.tag == "banana")
        {
            GameData.score++;
            GameData.banana++;
            Destroy(coll.gameObject);
            if (bonous)
                bonous.Play();
        }
    }

    float GetEndPosOnBlock(GameObject block)
    {
        GameObject blockBG = block.transform.Find("BlockBG").gameObject;
        SpriteRenderer sr = blockBG.GetComponent<SpriteRenderer>();
        return block.transform.position.x + sr.bounds.extents.x - GameData.stickSize-GetComponent<SpriteRenderer>().bounds.extents.x;
    }

    void OnCollisionExit2D(Collision2D coll)
    {
        if (coll.gameObject == stick)
        {
            _isCrossing = false;
            if (_isOnBlock)
                _isMove2Target = true;
            else
            {
                stick.SendMessage("Fall");
                if (cross)
                    cross.Stop();
                StartCoroutine(Fall());
            }
        }
        else if (coll.gameObject.tag == "block")
            _isOnBlock = false;
    }

    private IEnumerator Fall()
    {
        transform.Find("Shadow").gameObject.SetActive(false);
        GetComponent<Animator>().speed = 0;
        rigidbody2D.gravityScale = 0;
        yield return new  WaitForSeconds(0.3f);
        GetComponent<Animator>().speed = 1;
        rigidbody2D.gravityScale = 10;
    }
}
