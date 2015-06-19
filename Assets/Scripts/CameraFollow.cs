using UnityEngine;
using System.Collections;

public class CameraFollow : MonoBehaviour
{
    public Transform target;
    public float smoothing = 5f;
    private GameObject bg;
    // Use this for initialization
    void Start()
    {
        bg = GameObject.Find("BGStill");
        _goalPoint = new Vector3(0, 0, -10);
    }

    // Update is called once per frame
    void LateUpdate()
    {
        if (target != null)
        {
            transform.position = Vector3.Lerp(transform.position, _goalPoint, smoothing * Time.deltaTime);
            if(bg)
                bg.transform.position = new Vector3(transform.position.x, transform.position.y, 0);
        }
    }

    private Vector3 _goalPoint;
    public void Follow(float distant)
    {
        _goalPoint = transform.position + new Vector3(distant, 0, 0);
    }
}
