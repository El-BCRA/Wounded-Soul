using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PopBubble : MonoBehaviour
{
    [SerializeField] private float m_PopTimer;

    private float m_InitalScale;
    private float m_GoalScale;
    public bool m_Popping = false;
    private Color m_Color;

    private void Awake()
    {
        m_InitalScale = transform.localScale.x;
        m_GoalScale = m_InitalScale * 1.25f;
        m_Color = transform.GetComponent<SpriteRenderer>().color;
    }

    // Update is called once per frame
    void Update()
    {
        if (m_Popping)
        {
            if (m_PopTimer <= 0)
            {
                AudioManager.instance.Pop(transform.position);
                Destroy(transform.gameObject);
            } else
            {
                float newScale = (m_PopTimer * m_PopTimer) * Time.deltaTime;
                newScale += transform.localScale.x;
                m_Color.a = 0.5f + (0.5f * m_PopTimer/2.0f);
                transform.GetComponent<SpriteRenderer>().color = m_Color;
                m_PopTimer -= Time.deltaTime;
                transform.localScale = new Vector3(newScale, newScale, newScale);
            }
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Ceiling") || collision.gameObject.CompareTag("Bubble") && collision.gameObject != transform.gameObject)
        {
            m_Popping = true;
        }
    }

    private void OnCollisionStay2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player") && collision.gameObject.transform.position.y > gameObject.transform.position.y + 1)
        {
            m_Popping = true;
        }
    }
}
