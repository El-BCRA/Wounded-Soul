using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DropletFall : MonoBehaviour
{
    [SerializeField] private Animator animator;
    [SerializeField] private MovePlayer m_MovePlayer;

    private bool m_Landed = false;
    private float m_Disappear = 0.2f;
    private float m_Timer = 0;

    void Awake()
    {
        m_MovePlayer = GameObject.Find("Player").GetComponent<MovePlayer>();
    }

    // Update is called once per frame
    void Update()
    {
        if (m_Landed)
        {
            if (m_Timer >= m_Disappear)
            {
                Destroy(gameObject);
            } else
            {
                m_Timer += Time.deltaTime;
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            animator.SetTrigger("Landed");
            GetComponent<Rigidbody2D>().velocity = new Vector2(0.0f, 0.0f);
            transform.GetComponent<Rigidbody2D>().constraints = RigidbodyConstraints2D.FreezePositionX | RigidbodyConstraints2D.FreezePositionY | RigidbodyConstraints2D.FreezeRotation;
            m_Landed = true;
        } else if (collision.gameObject.CompareTag("Player")) 
        {
            animator.SetTrigger("Landed");
            m_Landed = true;
            MetricManager.instance.AddFallingAcid();
            m_MovePlayer.TakeDamage();
        }
    }
}
