using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class DisappearingText : MonoBehaviour
{
    [SerializeField] private MovePlayer m_MovePlayer;
    [SerializeField] private RelativeText m_RelativeText;
    [SerializeField] private string m_Message;

    private TextMeshPro m_Text;
    private Color m_TextColor;
    private bool m_TextOnScreen = false;

    // Update is called once per frame
    void Update()
    {

    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        // Player enters activation collider
        if (collision.gameObject.CompareTag("Player"))
        {
            if (!m_TextOnScreen)
            {
                m_Text = m_RelativeText.Create(m_Message);
                m_Text.fontSize = 2;
                m_TextColor = m_Text.color;
                m_TextColor.a = 0.0f;
                m_Text.color = m_TextColor;
                m_TextOnScreen = true;
            }
            StartCoroutine(FadeTextIn(m_Text));
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        // Player leaves activation collider
        if (other.gameObject.CompareTag("Player"))
        {
            StartCoroutine(FadeTextOut(m_Text));
        }
    }

    public IEnumerator FadeTextIn(TextMeshPro text)
    {
        while (text.color.a < 1)
        {
            m_TextColor.a += Time.deltaTime / 2;
            m_Text.color = m_TextColor;
            if (text.color.a >= 1)
            {
                StopCoroutine(FadeTextIn(m_Text));
            }
            yield return null;
        }
    }

    public IEnumerator FadeTextOut(TextMeshPro text)
    {
        while (text.color.a > 0)
        {
            m_TextColor.a -= Time.deltaTime / 2;
            m_Text.color = m_TextColor;
            if (text.color.a <= 0)
            {
                m_TextOnScreen = false;
                Destroy(m_Text);
            }
            yield return null;
        }
    }
}
