using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    [SerializeField] private GameObject creditsScreen;
    [SerializeField] private GameObject creditsUI;
    [SerializeField] private GameObject mainMenuScreen;
    [SerializeField] private GameObject mainMenuUI;

    private void Awake()
    {
        Cursor.visible = true;
    }

    public void PlayGame()
	{
        AudioManager.instance.Stop();
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
	}

    public void Credits()
    {
        mainMenuScreen.SetActive(false);
        mainMenuUI.SetActive(false);
        creditsScreen.SetActive(true);
        creditsUI.SetActive(true);
    }

    public void Back()
    {
        creditsScreen.SetActive(false);
        creditsUI.SetActive(false);
        mainMenuScreen.SetActive(true);
        mainMenuUI.SetActive(true);
    }

	public void Quit()
    {
		Application.Quit();
    }
}
