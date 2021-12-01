using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WhiteBloodCellPatrol : MonoBehaviour
{
    [SerializeField] private MetricManager manager;
    [SerializeField] private Animator animator;
    [SerializeField] private bool mFacingRight = false;
    [SerializeField] private float mPatrolRadius;
    [SerializeField] private float mPatrolSpeed;
    [SerializeField] private float mChaseSpeed;
    [SerializeField] private MovePlayer mMovePlayer;
    [SerializeField] private GameObject mPlayer;
    bool mChasing = false;

    Vector3 mPatrolMovement;
    Vector3 mInitialPosition;
    Vector3 mPatrolVector;
    Vector3 mToPlayer;
    float mDistanceToPlayer;

    private void Awake()
    {
        mInitialPosition = transform.position;
        mPatrolMovement = new Vector3(-1.0f * mPatrolSpeed, 0.0f, 0.0f);
        if (mFacingRight)
        {
            Flip();
        }
    }

    // Update is called once per frame
    void Update()
    {
        mToPlayer = mPlayer.transform.position - mInitialPosition;
        mDistanceToPlayer = mToPlayer.magnitude;
        mToPlayer = mPlayer.transform.position - transform.position;
        mToPlayer.Normalize();

        // Check if cell should begin chasing the player
        if (mDistanceToPlayer < mPatrolRadius && !mChasing)
        {
            mChasing = true;
            if (mFacingRight && mPlayer.transform.position.x < transform.position.x)
            {
                // Flip if cell is facing right but the player is to its left
                Flip();
            }
            else if (!mFacingRight && mPlayer.transform.position.x > transform.position.x)
            {
                // Flip if cell is facing left but the player is to its right
                Flip();
            }
        }

        // Update animator bool
        animator.SetBool("Chasing", mChasing);

        // Update cell position
        if (mChasing)
        {
            Vector3 rotateLookVector = Quaternion.Euler(0, 0, 90) * mToPlayer;
            Quaternion targetRotation = Quaternion.LookRotation(forward: Vector3.forward, upwards: rotateLookVector);
            transform.rotation = Quaternion.RotateTowards(transform.rotation, targetRotation, 360.0f);
            transform.Translate(mToPlayer * mChaseSpeed * (mFacingRight ? 1 : -1) * Time.deltaTime);
            if (mToPlayer.x < 0)
            {
                mFacingRight = false;
                transform.localScale = new Vector3(transform.localScale.x, -7.5f, transform.localScale.z);
            } else
            {
                mFacingRight = true;
                transform.localScale = new Vector3(transform.localScale.x, 7.5f, transform.localScale.z);
            }
        } else
        {
            transform.Translate(mPatrolMovement * Time.deltaTime);
        }

        // Check if cell has gone beyond it's patrol boundaries
        mPatrolVector = transform.position - mInitialPosition;
        if (mPatrolVector.magnitude > mPatrolRadius)
        {
            Flip();
            mChasing = false;
            transform.localScale = new Vector3(transform.localScale.x, 7.5f, transform.localScale.z);
            transform.rotation = Quaternion.identity;
        } 
        
        // Additionally, check if player has left the patrol zone boundaries
        if (mChasing && mDistanceToPlayer > mPatrolRadius)
        {
            mChasing = false;
            transform.localScale = new Vector3(transform.localScale.x, 7.5f, transform.localScale.z);
            transform.rotation = Quaternion.identity;
        }
    }

    private void Flip()
    {
        mFacingRight = !mFacingRight;
        mPatrolMovement = new Vector3(mPatrolMovement.x * -1, 0.0f, 0.0f);
        transform.localScale = new Vector3(transform.localScale.x * -1, transform.localScale.y, transform.localScale.z);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player") && !mMovePlayer.m_GameOver)
        {
            MetricManager.instance.AddWhiteBloodCellDamage();
            mMovePlayer.TakeDamage();
        } else if (collision.gameObject.CompareTag("Attack"))
        {
            MetricManager.instance.AddConfirmedKill();
            AudioManager.instance.EnemyHit();
            Destroy(collision.gameObject);
            Destroy(gameObject);
        }
    }
}
