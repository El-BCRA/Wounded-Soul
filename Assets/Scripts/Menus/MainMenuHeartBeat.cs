using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainMenuHeartBeat : MonoBehaviour
{
    [SerializeField] private float m_GoalScale;
    [SerializeField] private float m_InitialScale;

    private float m_Timer = 0;
    private float m_HangTimer = 0;
    private bool m_Increasing = false;
    private bool m_Decreasing = false;
    private bool m_Beating = true;

    void Update()
    {
        // Increment timer
        m_Timer += Time.deltaTime;

        // Check if, based on the current m__HeartLevel, the heart should beat now
        if (m_Timer > 3.0f)
        {
            AudioManager.instance.Play("HeartBeat");
            m_Timer = 0;
            m_Beating = true;
            m_Increasing = true;
        }

        if (m_Beating && m_Increasing)
        {
            Vector3 newScale = transform.localScale;
            float value = Easing() * Time.deltaTime;
            newScale += new Vector3(value, value, value);
            transform.localScale = newScale;
        }
        else if (m_Beating && m_Decreasing)
        {
            Vector3 newScale = transform.localScale;
            float value = Easing() * Time.deltaTime;
            newScale -= new Vector3(value, value, value);
            transform.localScale = newScale;
        }
        else if (m_Beating)
        {
            m_HangTimer += Time.deltaTime;
            if (m_HangTimer >= 0.1f)
            {
                m_HangTimer = 0;
                m_Decreasing = true;
            }
        }

        if (m_Beating && transform.localScale.x >= m_GoalScale)
        {
            m_Increasing = false;
        }
        else if (m_Beating && transform.localScale.x <= m_InitialScale)
        {
            transform.localScale = new Vector3(m_InitialScale, m_InitialScale, m_InitialScale);
            m_Beating = false;
            m_Decreasing = false;
        }
    }

    private float Easing()
    {
        float x = transform.localScale.x / 1.2f;
        if (x < 0.5)
        {
            return 2.0f * (16 * x * x * x * x * x);
        }
        else
        {
            return 2.0f * (1 - Mathf.Pow(-2 * x + 2, 5) / 2);
        }
    }
}
