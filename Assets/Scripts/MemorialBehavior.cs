using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MemorialBehavior : MonoBehaviour
{
	public void Update()
	{
		if (Input.GetKeyDown(KeyCode.Escape))
		{
			Back();
		}
	}

    public void Marathon()
    {
        SceneManager.LoadScene("MemorialMarathon");
    }

    public void FortyLines()
    {
        SceneManager.LoadScene("MemorialFortyLines");
    }

    public void Ultra()
    {
        SceneManager.LoadScene("MemorialUltra");
    }

	public void Cleaning()
	{
		SceneManager.LoadScene("MemorialCleaning");
	}

    public void Back()
    {
        SceneManager.LoadScene("mainMenu");
    }
}
