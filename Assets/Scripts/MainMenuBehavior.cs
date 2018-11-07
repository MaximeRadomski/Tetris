using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuBehavior : MonoBehaviour
{
    public void Start()
    {
        /*if (PlayerPrefs.GetFloat("MusicVolume") == 0.0F)
        {*/
            /*PlayerPrefs.SetFloat("MusicVolume", 1.0F);
            PlayerPrefs.SetFloat("EffectsVolume", 1.0F);*/
        /*}*/
        /*PlayerPrefs.SetFloat("FortyLinesBestTime", 999999999.999999F);*/
    }

	public void Update()
	{
		if (Input.GetKeyDown(KeyCode.Escape))
		{
			Application.Quit();
		}
	}

    public void Play()
    {
        SceneManager.LoadScene("PlayMenu");
    }

    public void Memorial()
    {
        SceneManager.LoadScene("Memorial");
    }

    public void Settings()
    {
        SceneManager.LoadScene("Settings");
    }

    public void quitGame()
    {
        Application.Quit();
    }

    public void Credits()
    {
        SceneManager.LoadScene("Credits");
    }
}
