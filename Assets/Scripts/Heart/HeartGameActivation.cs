using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class HeartGameActivation : MonoBehaviour
{
    [SerializeField] private MovePlayer m_MovePlayer;
    [SerializeField] private RelativeText m_RelativeText;
    [SerializeField] private CameraStateMachine instance;

    private TextMeshPro m_Text;
    private Color m_TextColor;
    private bool m_TextOnScreen = false;
    private bool m_InTrigger = false;
    private bool m_FirstHeartGame = false;
    
    public bool m_InHeartGame = false;
    public float m_HeartLevel = 0.0f;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        // Start incrementing heart timer if the player has played the heart game once.
        if (m_FirstHeartGame)
        {
            m_HeartLevel += Time.deltaTime;
        }

        if (m_InTrigger)
        {
            if (m_InHeartGame)
            {
                if (Input.GetKeyDown(KeyCode.E))
                {
                    AudioManager.instance.inCutscene = false;
                    if (!m_TextOnScreen)
                    {
                        m_Text = m_RelativeText.Create("Press 'E'");
                        m_TextColor = m_Text.color;
                        m_TextColor.a = 0.0f;
                        m_Text.color = m_TextColor;
                        m_TextOnScreen = true;
                    }
                    m_InHeartGame = false;
                    instance.m_Zone = CameraStateMachine.Zone.HeartCenter;
                    StartCoroutine(FadeTextIn(m_Text));
                    m_MovePlayer.m_CanJump = true;
                }
            }
            else
            {
                if (Input.GetKeyDown(KeyCode.E))
                {
                    AudioManager.instance.inCutscene = true;
                    m_MovePlayer.m_CanJump = false;
                    m_InHeartGame = true;
                    instance.m_Zone = CameraStateMachine.Zone.HeartGame;
                    StartCoroutine(FadeTextOut(m_Text));
                }
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        // Player enters activation collider
        if (collision.gameObject.CompareTag("Player"))
        {
            m_InTrigger = true;
            if (!m_TextOnScreen)
            {
                m_FirstHeartGame = true;
                m_Text = m_RelativeText.Create("Press 'E'");
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
        AudioManager.instance.inCutscene = false;
        // Player leaves activation collider
        if (other.gameObject.CompareTag("Player"))
        {
            m_InTrigger = false;
            StartCoroutine(FadeTextOut(m_Text));
            m_InHeartGame = false;
            instance.m_Zone = CameraStateMachine.Zone.HeartCenter;
            m_MovePlayer.m_CanJump = true;
        }
    }

    public IEnumerator FadeTextIn(TextMeshPro text)
    {
        while (text.color.a < 1)
        {
            m_TextColor.a += Time.deltaTime;
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
            m_TextColor.a -= Time.deltaTime;
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
