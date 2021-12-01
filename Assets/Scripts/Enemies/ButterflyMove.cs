using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButterflyMove : MonoBehaviour
{
    [SerializeField] private bool m_FacingRight = false;
    [SerializeField] private float m_FlySpeed;
    [SerializeField] private float m_EnemyJumpBoost;
    [SerializeField] private LayerMask m_WhatIsCeiling;
    [SerializeField] private LayerMask m_WhatIsDeath;
    [SerializeField] private Transform m_CeilingCheck;
    [SerializeField] private MovePlayer m_MovePlayer;

    private bool m_reachingAWall;

    private void Awake()
    {
        if (m_FacingRight)
        {
            // Multiply the sprite's x local scale by -1.
            transform.transform.Rotate(new Vector3(0, 180, 0));
            transform.GetComponent<Rigidbody2D>().velocity = new Vector2(m_FlySpeed, 0.0f);
        }
        else
        {
            transform.GetComponent<Rigidbody2D>().velocity = new Vector2(m_FlySpeed * -1.0f, 0.0f);
        }
    }

    // Update is called once per frame
    void Update()
    {
        float xVelocity = transform.GetComponent<Rigidbody2D>().velocity.x;
        if (m_reachingAWall)
        {
            transform.GetComponent<Rigidbody2D>().velocity = new Vector2(xVelocity * 0.99f, 0.0f);
            if (Mathf.Abs(xVelocity) <= m_FlySpeed * 0.5f)
            {
                transform.transform.Rotate(new Vector3(0, 180, 0));
                m_FacingRight = !m_FacingRight;
                transform.GetComponent<Rigidbody2D>().velocity = new Vector2(m_FlySpeed * (xVelocity < 0 ? 1.0f : -1.0f), 0.0f);
                m_reachingAWall = false;
            }
        }
    }


    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player") && !m_MovePlayer.m_GameOver)
        {
            // If player is below butterfly
            if (collision.gameObject.transform.position.y < transform.position.y)
            {
                MetricManager.instance.AddButterflyDamage();
                m_MovePlayer.TakeDamage();
            } else
            {
                m_MovePlayer.animator.SetTrigger("LandedOnEnemy");
                MetricManager.instance.AddButterflyBoost();
                collision.transform.GetComponent<Rigidbody2D>().velocity = new Vector2(collision.transform.GetComponent<Rigidbody2D>().velocity.x, 0.0f);
                collision.transform.GetComponent<Rigidbody2D>().AddForce(new Vector2(0f, m_EnemyJumpBoost));
            }
        }
        else if (collision.gameObject.CompareTag("Attack"))
        {
            MetricManager.instance.AddConfirmedKill();
            AudioManager.instance.EnemyHit();
            Destroy(collision.gameObject);
            Destroy(gameObject);
        }
    }

    private void FixedUpdate()
    {
        Collider2D[] colliders = Physics2D.OverlapCircleAll(m_CeilingCheck.position, 2.0f, m_WhatIsCeiling);
        for (int i = 0; i < colliders.Length; i++)
        {
            if (colliders[i].gameObject != gameObject)
            {
                m_reachingAWall = true;
            }
        }
        colliders = Physics2D.OverlapCircleAll(m_CeilingCheck.position, 2.0f, m_WhatIsDeath);
        for (int i = 0; i < colliders.Length; i++)
        {
            if (colliders[i].gameObject != gameObject)
            {
                Destroy(gameObject);
            }
        }
    }
}
