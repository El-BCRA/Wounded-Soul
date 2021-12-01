using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeartGameRun : MonoBehaviour
{
    [SerializeField] private HeartGameActivation m_Activator;
    [SerializeField] private MovePlayer m_MovePlayer;
    [SerializeField] private AudioSource m_Beat;
    [SerializeField] private AudioSource m_Success;
    [SerializeField] private AudioSource m_Fail;
    [SerializeField] private float m_GoalScale;
    [SerializeField] private float m_InitialScale;
    private AudioSource m_BGM;
    private float m_Timer = 0.0f;
    private float m_InputDelay = 0.0f;
    private bool m_Beating = false;
    private bool m_Increasing = false;
    private bool m_LevelSet = false;
    private int m_HeartLevel;
    // private int m_Successes = 0;


    private void Awake()
    {
        // m_BGM = GameObject.Find("HeartBGM").GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
    }

    private void FixedUpdate()
    {
        if (!m_LevelSet)
        {
            m_HeartLevel = (int)(m_Activator.m_HeartLevel / 60.0f + 1.0f);
            if (m_HeartLevel > 3)
            {
                m_HeartLevel = 3;
            }
        }
        if (m_Activator.m_InHeartGame)
        {
            // Increment timer
            m_Timer += Time.deltaTime;
            
            // Check if, based on the current m__HeartLevel, the heart should beat now
            if (m_Timer > (2.030f))
            {
                AudioManager.instance.Play("HeartBeat");
                m_Timer = 0;
                m_Beating = true;
                m_Increasing = true;
            }

            if (m_InputDelay > 0)
            {
                m_InputDelay -= Time.deltaTime;
            }

            if (m_Beating && m_Increasing)
            {
                Vector3 newScale = transform.localScale;
                newScale += new Vector3(20.0f * Time.deltaTime, 20.0f * Time.deltaTime, 20.0f * Time.deltaTime);
                transform.localScale = newScale;
            } else if (m_Beating)
            {
                Vector3 newScale = transform.localScale;
                newScale -= new Vector3(4.0f * Time.deltaTime, 4.0f * Time.deltaTime, 4.0f * Time.deltaTime);
                transform.localScale = newScale;
            }

            if (m_Beating && transform.localScale.x >= m_GoalScale)
            {
                m_Increasing = false;
            } else if (m_Beating && transform.localScale.x <= m_InitialScale)
            {
                transform.localScale = new Vector3(m_InitialScale, m_InitialScale, m_InitialScale);
                m_Beating = false;
            }

            /*
            // Check if succeeded
            if (Input.GetKeyDown(KeyCode.Space) && m_InputDelay <= 0)
            {
                if (m_Beating && m_Increasing)
                {
                    m_Fail.Play();
                } else if (m_Beating)
                {
                    m_Success.Play();
                    m_Successes++;
                    if (m_Successes == 3)
                    {
                        m_HeartLevel--;
                        m_Timer = m_HeartLevel * 60.0f;
                        m_Successes = 0;
                        if (m_HeartLevel < 1)
                        {
                            m_HeartLevel = 1;
                            m_Activator.m_InHeartGame = false;
                            m_Timer = 0.0f;
                        } 
                    }
                } else
                {
                    m_Fail.Play();
                }
                m_InputDelay = 0.1f;
            }

            */
            // Fade out BGM (if not already done)
            AudioManager.instance.LowerVolume();
        } else
        {
            // Reset all relevant values
            // m_Successes = 0;

            // Continue resetting heart if not yet set
            if (transform.localScale.x > m_InitialScale)
            {
                Vector3 newScale = transform.localScale;
                newScale -= new Vector3(4.0f * Time.deltaTime, 4.0f * Time.deltaTime, 4.0f * Time.deltaTime);
                transform.localScale = newScale;
            }
            if (transform.localScale.x < m_InitialScale)
            {
                transform.localScale = new Vector3(m_InitialScale, m_InitialScale, m_InitialScale);
            }
            // Fade in BGM (if not already done)
            AudioManager.instance.RaiseVolume();
        }
    }
}
