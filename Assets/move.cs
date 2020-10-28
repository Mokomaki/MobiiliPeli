using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class move : MonoBehaviour
{
    Camera cam;
    float min = -5;
    float max = 5;
    Rigidbody m_rb;

    public float m_speed = 1f;

    void Start()
    {
        cam = Camera.main;
        m_rb = GetComponent<Rigidbody>();
    }

    void Update()
    {
        if (Input.touchCount > 0)
        {
            Touch touch = Input.GetTouch(0);
            m_rb.velocity = (new Vector3(Mathf.Clamp(-(transform.position - cam.ScreenToWorldPoint(new Vector3(touch.position.x, touch.position.y, cam.transform.position.z))).x,min,max), m_rb.velocity.y,0));
        }
   
    }
}
