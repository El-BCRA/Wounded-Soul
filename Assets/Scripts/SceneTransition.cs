using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SceneTransition : MonoBehaviour
{
    [SerializeField] private GameObject blackOutSquare;
    [SerializeField] private GameObject m_Warp;
    [SerializeField] private MovePlayer m_PlayerScript;
    [SerializeField] private GameObject m_Player;

    [SerializeField] private float m_FadeSpeed;
    [SerializeField] private CameraStateMachine instance;
    [SerializeField] private CameraStateMachine.Zone m_WarpZone;

    private bool m_Faded = false;

    private void Awake()
    {
        Color objectColor = blackOutSquare.GetComponent<Image>().color;
        objectColor = new Color(objectColor.r, objectColor.g, objectColor.b, 1.0f);
        blackOutSquare.GetComponent<Image>().color = objectColor;
        StartCoroutine("FadeInLoad");
    }

    private void Update()
    {
        if (m_Faded)
        {
            m_Player.transform.position = m_Warp.transform.position;
            instance.m_Zone = m_WarpZone;
            StartCoroutine("FadeIn");
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            if (m_WarpZone == CameraStateMachine.Zone.Stomach)
            {
                AudioManager.instance.Stop();
                AudioManager.instance.Play("StomachBGM");
            }
            else if (m_WarpZone != CameraStateMachine.Zone.Stomach && instance.m_Zone == CameraStateMachine.Zone.Stomach)
            {
                AudioManager.instance.Stop();
                AudioManager.instance.Play("HeartBGM");
            } else if (m_WarpZone == CameraStateMachine.Zone.Brain)
            {
                AudioManager.instance.Stop();
                AudioManager.instance.Play("BrainBGM");
            }
            AudioManager.instance.spawnPoint = m_Warp.transform.position;
            AudioManager.instance.warpZone = m_WarpZone;
            StartCoroutine("FadeOut");
        }
    }

    public IEnumerator FadeOut()
    {
        m_PlayerScript.m_InCutscene = true;
        Color objectColor = blackOutSquare.GetComponent<Image>().color;
        float fadeAmount;
        while (blackOutSquare.GetComponent<Image>().color.a < 1)
        {
            fadeAmount = objectColor.a + (m_FadeSpeed * Time.deltaTime);

            if (fadeAmount > 1)
            {
                fadeAmount = 1;
                m_Faded = true;
                StopCoroutine("FadeOut");
            }

            objectColor = new Color(objectColor.r, objectColor.g, objectColor.b, fadeAmount);
            blackOutSquare.GetComponent<Image>().color = objectColor;
            yield return null;
        }
    }

    public IEnumerator FadeIn()
    {
        yield return new WaitForSeconds(0.5f);

        Color objectColor = blackOutSquare.GetComponent<Image>().color;
        float fadeAmount;
        while (blackOutSquare.GetComponent<Image>().color.a > 0)
        {
            fadeAmount = objectColor.a - (m_FadeSpeed * Time.deltaTime);

            if (fadeAmount < 0)
            {
                fadeAmount = 0;
                m_Faded = false;
                m_PlayerScript.m_InCutscene = false;
                StopCoroutine("FadeIn");
            }

            objectColor = new Color(objectColor.r, objectColor.g, objectColor.b, fadeAmount);
            blackOutSquare.GetComponent<Image>().color = objectColor;
            yield return null;
        }
    }

    public IEnumerator FadeInLoad()
    {
        yield return new WaitForSeconds(1.0f);

        Color objectColor = blackOutSquare.GetComponent<Image>().color;
        float fadeAmount;
        while (blackOutSquare.GetComponent<Image>().color.a > 0)
        {
            fadeAmount = objectColor.a - (m_FadeSpeed * Time.deltaTime);

            if (fadeAmount < 0)
            {
                fadeAmount = 0;
                m_Faded = false;
                m_PlayerScript.m_InCutscene = false;
                StopCoroutine("FadeIn");
            }

            objectColor = new Color(objectColor.r, objectColor.g, objectColor.b, fadeAmount);
            blackOutSquare.GetComponent<Image>().color = objectColor;
            yield return null;
        }
    }
}
