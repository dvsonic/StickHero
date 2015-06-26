using UnityEngine;
using System.Collections;

public class StickController : MonoBehaviour {

	// Use this for initialization
    private Vector3 _initScale;
    public float dropStill = 0.2f;
    public float growSpeed = 15.0f;
    public float dropSpeed = -270.0f;
    public float maxScale = 35;

    public AudioSource grow;
    public AudioSource drop;
    public AudioSource fall;
    public AudioSource hit;
    public AudioSource miss;
	void Start () {
        _initScale = transform.localScale;
	}
	
	// Update is called once per frame
	void Update () {
        switch (_curState)
        {
            case STATE.GROW:
                if(transform.localScale.y < maxScale)
                    transform.localScale += new Vector3(0, growSpeed*Time.deltaTime, 0);
                transform.localRotation = new Quaternion();
                break;
            case STATE.DROP:
                if (Time.time - _dropBegin < dropStill)
                    return;
                transform.Rotate(0, 0, dropSpeed * Time.deltaTime);
                if (transform.eulerAngles.z <270)
                {
                    SetSound(STATE.DROP, true);
                    transform.eulerAngles = new Vector3(0, 0, 270);
                    if (_targetBlock)
                    {
                        _targetBlock.transform.Find("Bonus").gameObject.SetActive(false);
                        float stickEnd = transform.position.x + GetComponent<SpriteRenderer>().bounds.size.x;
                        if (Mathf.Abs(stickEnd - _targetBlock.transform.position.x) < 0.1f)
                        {
                            _targetBlock.transform.Find("BonusTip").gameObject.SetActive(true);
                            GameData.score++;
                            if (hit)
                                hit.Play();
                        }
                        else
                        {
                            if (miss)
                                miss.Play();
                        }
                    }
                    EventManager.getInstance().trigger(Event_Name.STICK_FALL_END);
                    _curState = STATE.CROSS;
                }
                break;
            case STATE.FALL:
                transform.Rotate(0, 0, dropSpeed * Time.deltaTime);
                if (transform.eulerAngles.z < 180)
                {
                    transform.eulerAngles = new Vector3(0, 0, 180);
                    _curState = STATE.IDLE;
                    iTween.ShakePosition(Camera.main.gameObject, new Vector3(0.2f, 0.2f, 0), 0.3f);
                }
                break;
        }
	}

    public void SetSound(STATE state, bool enabled)
    {
        AudioSource sound;
        switch (state)
        {
            case STATE.GROW:
                sound = grow;
                break;
            case STATE.DROP:
                sound = drop;
                break;
            case STATE.FALL:
                sound = fall;
                break;
            default:
                sound = null;
                break;
        }
        if (sound != null)
        {
            if (enabled)
                sound.Play();
            else
                sound.Stop();
        }
    }
    private GameObject _targetBlock;
    void SetTargetBlock(GameObject block)
    {
        _targetBlock = block;
    }

    private float _dropBegin;
    void SetGrowing(bool value)
    {
        if (value && _curState == STATE.IDLE)
        {
            _curState = STATE.GROW;
            SetSound(_curState, true);
        }
        else if (!value && _curState == STATE.GROW)
        {
            _dropBegin = Time.time;
            _curState = STATE.DROP;
            SetSound(STATE.GROW, false);
        }
    }

    void Fall()
    {
        _curState = STATE.FALL;
        SetSound(_curState, true);
    }

    void OnCollisionEnter2D(Collision2D coll)
    {

    }

    public enum STATE { IDLE, GROW, DROP, CROSS,FALL}
    private STATE _curState;
    void Reset(GameObject block)
    {
        _curState = STATE.IDLE;
        transform.localScale = _initScale;
        transform.rotation = new Quaternion();
        SpriteRenderer sr = block.transform.Find("BlockBG").gameObject.GetComponent<SpriteRenderer>();
        Vector3 extents = sr.bounds.extents;
        SpriteRenderer stickSR = GetComponent<SpriteRenderer>();
        Vector3 stickExtends = stickSR.bounds.extents;
        transform.position = block.transform.position + new Vector3(extents.x-stickExtends.x, extents.y-GameData.stickSize/2, 0);

    }
}
