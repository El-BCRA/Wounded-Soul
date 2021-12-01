using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour
{
    public bool m_Paused = false;

    [SerializeField] private GameObject m_PauseMenuUI;

    private void Awake()
    {
        Cursor.visible = false;
        m_PauseMenuUI.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (m_Paused)
            {
                Resume();
            } else
            {
                Pause();
            }
        }
    }

    public void Resume()
    {
        Time.timeScale = 1.0f;
        m_PauseMenuUI.SetActive(false);
        Cursor.visible = false;
        m_Paused = false;
    }

    void Pause()
    {
        Time.timeScale = 0f;
        m_PauseMenuUI.SetActive(true);
        Cursor.visible = true;
        m_Paused = true;
    }

    public void LoadMenu()
    {
        Time.timeScale = 1.0f;
        m_Paused = false;
        m_PauseMenuUI.SetActive(false);
        AudioManager.instance.Stop();
        AudioManager.instance.Play("MainMenuBGM");
        SceneManager.LoadScene("MainMenu");
    }

    public void Quit()
    {
        Application.Quit();
    }
}
