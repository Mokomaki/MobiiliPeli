using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class move : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI tmp;
    short apples = 5;
    Camera cam;
    public float Speed;
    Rigidbody m_rb;
    bool m_faceRight = true;
    [SerializeField] Transform m_KettuMalli;
    [SerializeField] Animator m_animator;
    float beginTouch = 0;

    [SerializeField] GameObject panel;
    [SerializeField] GameObject score;

    bool playing = true;
   void Start()
    {
        cam = Camera.main;
        m_rb = GetComponent<Rigidbody>();

        InvokeRepeating("LoseApple", 6f, 2f);
    }

    void LoseApple()
    {
        if(apples>0)
        {
            apples--;
            tmp.text = "x" + apples;
        }else
        {
            panel.SetActive(true);
            score.SetActive(false);
            playing = false;
        }

    }

    void Update()
    {
        if (!playing) return;

        if(m_rb.velocity == Vector3.zero)
        {
            m_animator.SetBool("Walking", false);
        }
        else
        {
            m_animator.SetBool("Walking", true);
        }

        if (Input.touchCount > 0)
        {
            if(beginTouch==0) beginTouch = Time.time;

            Touch touch = Input.GetTouch(0);

            float xMove = -(transform.position - cam.ScreenToWorldPoint(new Vector3(touch.position.x, touch.position.y, cam.transform.position.z))).x;
            if (xMove != 0) xMove = (xMove < 0) ? -Speed : Speed;

            m_rb.velocity = new Vector3(xMove, m_rb.velocity.y, 0);

            if (m_faceRight)
            {
                if (xMove > 0)
                {
                    m_KettuMalli.Rotate(0, 180, 0);
                    m_faceRight = false;
                }
            }
            else if (!m_faceRight)
            {
                if (xMove < 0)
                {
                    m_KettuMalli.Rotate(0, 180, 0);
                    m_faceRight = true;
                }
            }
        }
        else
        {
            beginTouch = 0;
        }
   
    }
    private void OnCollisionStay(Collision collision)
    {
        
        ContactPoint contact = collision.contacts[0];
        if (contact.otherCollider.gameObject.CompareTag("bounds")) return;
        Quaternion rotation = Quaternion.FromToRotation(transform.up, contact.normal);
        transform.rotation *= rotation;
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("apple"))
        {
            apples++;
            Destroy(other.gameObject);
            tmp.text = "x" + apples;
        }
    }
}