using UnityEngine;
using System.Collections;
using UnityEngine.EventSystems;

public class TouchDelegate : MonoBehaviour, IPointerDownHandler, IPointerUpHandler, IMoveHandler, IPointerExitHandler, IPointerEnterHandler
{
    public GameObject stick;
    private bool _isPress;
    private Vector3 _initScale;
    void Start()
    {
        _isPress = false;
        if (null == stick)
            stick = GameObject.Find("Stick");
#if UNITY_EDITOR
       
#else
         gameObject.SetActive(false);
#endif
    }

    // Update is called once per frame
    void Update()
    {
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        _isPress = true;
        stick.SendMessage("SetGrowing", true);
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        _isPress = false;
        stick.SendMessage("SetGrowing", false);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
    }

    public void OnMove(AxisEventData eventData)
    {
    }

    void OnMouseOver()
    {
    }
}
