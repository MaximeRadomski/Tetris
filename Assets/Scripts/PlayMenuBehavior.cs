using System.Collections;
using System.Collections.Generic;
/*using UnityEditor;*/
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayMenuBehavior : MonoBehaviour
{
    // GameType
    // 0 = Marathon
    // 1 = FortyLines
    // 2 = Ultra
	// 3 = Cleaning
	public GameObject GameInProgress;

    private GameObject tmp;

	public void Start()
	{
		if (PlayerPrefs.GetInt ("SaveGame") == 1)
		{
			GameInProgress.GetComponent<UnityEngine.UI.Image> ().enabled = true;

			int GameType = PlayerPrefs.GetInt ("SaveGameGameType");
			string tmpText = "";

			if (GameType == 0) {
				tmpText += "Game Mode : Marathon\n";
				tmpText += "Score : " + PlayerPrefs.GetInt ("SaveGameScore") + "\n";
				tmpText += "Level : " + PlayerPrefs.GetInt ("SaveGameLevel") + "\n";
				tmpText += "Lines : " + PlayerPrefs.GetInt ("SaveGameLines");
			} else if (GameType == 1) {
				int minutes = (int)(PlayerPrefs.GetFloat ("SaveGameTimer") / 60);
				int seconds = (int)(PlayerPrefs.GetFloat ("SaveGameTimer") % 60);
				int milliseconds = (int)((PlayerPrefs.GetFloat ("SaveGameTimer") % 1) * 1000);
				tmpText += "Game Mode : 40 Lines\n";
				tmpText += "Time : " + minutes.ToString("D2") + ":" + seconds.ToString("D2") + ":" + milliseconds.ToString("D3") + "\n";
				tmpText += "Lines : " + PlayerPrefs.GetInt ("SaveGameLines");
			} else if (GameType == 2) {
				int minutes = (int)(PlayerPrefs.GetFloat ("SaveGameTimerUltra") / 60);
				int seconds = (int)(PlayerPrefs.GetFloat ("SaveGameTimerUltra") % 60);
				int milliseconds = (int)((PlayerPrefs.GetFloat ("SaveGameTimerUltra") % 1) * 1000);
				tmpText += "Game Mode : Ultra\n";
				tmpText += "Time : " + minutes.ToString("D2") + ":" + seconds.ToString("D2") + ":" + milliseconds.ToString("D3") + "\n";
				tmpText += "Score : " + PlayerPrefs.GetInt ("SaveGameScore") + "\n";
				tmpText += "Level : " + PlayerPrefs.GetInt ("SaveGameLevel") + "\n";
				tmpText += "Lines : " + PlayerPrefs.GetInt ("SaveGameLines");
			} else if (GameType == 3) {
				int minutes = (int)(PlayerPrefs.GetFloat ("SaveGameTimerCleaning") / 60);
				int seconds = (int)(PlayerPrefs.GetFloat ("SaveGameTimerCleaning") % 60);
				int milliseconds = (int)((PlayerPrefs.GetFloat ("SaveGameTimerCleaning") % 1) * 1000);
				tmpText += "Game Mode : Cleaning\n";
				tmpText += "Time : " + minutes.ToString("D2") + ":" + seconds.ToString("D2") + ":" + milliseconds.ToString("D3") + "\n";
				tmpText += "Lines : " + PlayerPrefs.GetInt ("SaveGameLines");
			}
			GameObject.Find ("Text").GetComponent<UnityEngine.UI.Text> ().text = tmpText;
		}
		else
		{
			GameInProgress.GetComponent<UnityEngine.UI.Image>().enabled = false;
			GameObject.Find ("Text").GetComponent<UnityEngine.UI.Text> ().text = "";
		}
	}

	public void Update()
	{
		if (Input.GetKeyDown(KeyCode.Escape))
		{
			BackToMenu();
		}
	}

    public void Marathon()
    {
		PlayerPrefs.SetInt ("SaveGame", 0);
		PlayerPrefs.SetInt("GameType", 0);
        SceneManager.LoadScene("GetReady");
        /*SceneManager.LoadScene("Marathon");*/
    }

    public void FortyLines()
    {
		PlayerPrefs.SetInt ("SaveGame", 0);
        PlayerPrefs.SetInt("GameType", 1);
        SceneManager.LoadScene("GetReady");
        /*SceneManager.LoadScene("FortyLines");*/
    }

    public void Ultra()
    {
		PlayerPrefs.SetInt ("SaveGame", 0);
        PlayerPrefs.SetInt("GameType", 2);
        SceneManager.LoadScene("GetReady");
        /*SceneManager.LoadScene("Ultra");*/
    }

	public void Cleaning()
	{
		PlayerPrefs.SetInt ("SaveGame", 0);
		PlayerPrefs.SetInt("GameType", 3);
		SceneManager.LoadScene("GetReady");
		/*SceneManager.LoadScene("Ultra");*/
	}

	public void LoadGame()
	{
		PlayerPrefs.SetInt("GameType", PlayerPrefs.GetInt("SaveGameGameType"));
		SceneManager.LoadScene("GetReady");
	}

    public void BackToMenu()
    {
        SceneManager.LoadScene("mainMenu");
    }
}
