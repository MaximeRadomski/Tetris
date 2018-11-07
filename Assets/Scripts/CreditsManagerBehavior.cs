using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CreditsManagerBehavior : MonoBehaviour
{
	public void Update()
	{
		if (Input.GetKeyDown(KeyCode.Escape))
		{
			BackToMenu();
		}
	}

    public void BackToMenu()
    {
        SceneManager.LoadScene("mainMenu");
    }
}
