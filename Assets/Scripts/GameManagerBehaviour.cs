using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using UnityEngine.SceneManagement;

public class GameManagerBehaviour : MonoBehaviour
{
    //A float array that stores the audio samples  
    public float[] samples = new float[64];
    public Sprite[] Singer;
    public Sprite sexyBaground;
    public Sprite sexyBagroundDude;
    public Sprite kremlinBackground;
    public bool sexyEnable = false;
    public bool pauseEnable = false;
    public GameObject ScoreManager;
    public bool gameOver = false;
    public GameObject TetrisDancers;

	public bool IsOnMobile = false;

    private AudioSource[] aSources;
    private AudioSource currentAudioSource;
    private float currentAudioSourceOldVolume;
    private GameObject tmp1;
    private GameObject tmp2;
    private bool volumeDown = false;
    private bool musicVisualizerDelay = true;
    private bool refreshing;
    private int sexyVoiceLine = 0;
	private Camera mainCamera;
	private GameObject popup;
	private int nbFadingPopups = 0;

    // Use this for initialization
    void Start ()
    {
        // ----------------------- CAREFUL ----------------------- //
        // ----------------------- CAREFUL ----------------------- //
        // ----------------------- CAREFUL ----------------------- //
        //PlayerPrefs.DeleteAll();
        // ----------------------- CAREFUL ----------------------- //
        // ----------------------- CAREFUL ----------------------- //
        // ----------------------- CAREFUL ----------------------- //
        /*currentScene = SceneManager.GetActiveScene();*/
        Cursor.visible = false;
        aSources = GetComponents<AudioSource>();
		/* AudioSources
		 * 0 - Main Music
		 * 1 - GameOverMusic
		 * 2 - Pause Menu
		 * 3 - MCB Reporting In
		 * 4 - Zangief For Mother Russia
		 * 5 - MCB As You Wish
		 * 6 - Zangief Is That All You Have
		*/
        /*Debug.Log("Music: " + (float)PlayerPrefs.GetInt("MusicVolume") / 10);
        Debug.Log("Effects: " + (float)PlayerPrefs.GetInt("EffectsVolume") / 10);*/
        aSources[0].volume = aSources[0].volume * ((float)PlayerPrefs.GetInt("MusicVolume") / 10);
        aSources[1].volume = aSources[1].volume * ((float)PlayerPrefs.GetInt("MusicVolume") / 10);
        aSources[2].volume = aSources[2].volume * ((float)PlayerPrefs.GetInt("EffectsVolume") / 10);
        aSources[3].volume = aSources[3].volume * ((float)PlayerPrefs.GetInt("EffectsVolume") / 10);
        aSources[4].volume = aSources[4].volume * ((float)PlayerPrefs.GetInt("EffectsVolume") / 10);
        aSources[5].volume = aSources[5].volume * ((float)PlayerPrefs.GetInt("EffectsVolume") / 10);
        aSources[6].volume = aSources[6].volume * ((float)PlayerPrefs.GetInt("EffectsVolume") / 10);
        currentAudioSource = aSources[0];
        currentAudioSourceOldVolume = currentAudioSource.volume;
        currentAudioSource.Play();
        if (PlayerPrefs.GetInt("HighScore") == 0)
        {
            PlayerPrefs.SetInt("HighScore", 0);
            PlayerPrefs.SetInt("HighScoreLevel", 0);
            PlayerPrefs.SetInt("HighScoreLines", 0);
        }


		// HANDLE SEXY //
		if (PlayerPrefs.GetInt ("SexyGenre") == 0)
		{
			PlayerPrefs.SetInt("SexyGenre", 2); //1 for male, 2 for female. Female first.
		}
		if (PlayerPrefs.GetInt ("SexyGenre") == 1)
		{
			sexyVoiceLine = 1;
			GameObject.Find ("BackgroundSexy").GetComponent<SpriteRenderer> ().sprite = sexyBagroundDude;
		}
		else if (PlayerPrefs.GetInt ("SexyGenre") == 2)
		{
			sexyVoiceLine = 0;
			GameObject.Find("BackgroundSexy").GetComponent<SpriteRenderer>().sprite = sexyBaground;
		}
		// HANDLE SEXY //


		if (Application.platform == RuntimePlatform.Android || Application.platform == RuntimePlatform.IPhonePlayer) {
			IsOnMobile = true;
		}
		GameObject.Find("MobileKeys").SetActive(IsOnMobile);
		TetrisDancers = GameObject.Find ("TetrisDance");
		mainCamera = GameObject.Find("Main Camera").GetComponent<Camera>();
		popup = GameObject.Find ("Popup");

		var scoreIsReady = ScoreManager.GetComponent<ScoreBehaviour> ().CustomStart ();
		var spawnerIsReady = GameObject.Find ("Spawner").GetComponent<Spawner> ().CustomStart ();
		if (PlayerPrefs.GetInt("SaveGame") == 1 && scoreIsReady == true && spawnerIsReady == true)
			LoadSaveGame();
    }

	// Update is called once per frame
	void Update ()
    {
        if (gameOver == true)
        {
            if (currentAudioSource.time >= 16.0f)
                currentAudioSource.Stop();
        }
        if (Input.GetButtonDown("Secret")) //
        {
            toggleSexyHotKey();
        }
        if (Input.GetButtonDown("Mute")) //MUTE
        {
            if (volumeDown == false)
            {
                currentAudioSource.volume = 0;
                Invoke("LoweringVolume", 0.2f);
            }
            else
            {
                currentAudioSource.volume = currentAudioSourceOldVolume;
                volumeDown = false;
            }
        }
        if (Input.GetButtonDown("Pause")) //PAUSE
        {
			Pause();
        }
        if (volumeDown == false)
        {
            if (musicVisualizerDelay == true)
            {
                MusicVisualizer();
                musicVisualizerDelay = false;
            }
            else if (refreshing == false)
            {
                refreshing = true;
                Invoke("RefreshMusicVisualizer", 0.05f);
            }
        }
    }

    void MusicVisualizer()
    {
        float iterator = 0;
        float test;
        //Obtain the samples from the frequency bands of the attached AudioSource  
        currentAudioSource.GetSpectrumData(this.samples, 0, FFTWindow.BlackmanHarris);

        //For each sample  
        for (int i = 0; i < samples.Length; i++)
        {
            if ((i+1) % 4 == 0)
            {
                test = Mathf.Clamp(samples[i] * (10 + i * i), 0, 10);
                tmp1 = GameObject.Find("Singer (" + i/4 + ")");
                tmp2 = GameObject.Find("Singer (" + i/4 + "bis)");
                if (tmp1 && tmp2)
                {
                    /*if (i / 4 >= 13)
                        iterator = 0.4f;*/
                    /*Vector3 positionTmp = tmp.transform.position;
                    positionTmp.y = 1.0f + (test);
                    tmp.transform.position += positionTmp;*/
                    if (test > 0.2 - iterator)
                    {
                        tmp1.GetComponent<SpriteRenderer>().sprite = Singer[1];
                        tmp2.GetComponent<SpriteRenderer>().sprite = Singer[1];
                    }
                    else
                    {
                        tmp1.GetComponent<SpriteRenderer>().sprite = Singer[0];
                        tmp2.GetComponent<SpriteRenderer>().sprite = Singer[0];
                    }
                }
                tmp1 = GameObject.Find("SmallCurtain (" + i / 4 + ")");
                tmp2 = GameObject.Find("SmallCurtain (" + i / 4 + "bis)");
                if (tmp1 && tmp2)
                {
                    Vector3 positionTmp1 = tmp1.transform.position;
                    Vector3 positionTmp2 = tmp2.transform.position;
                    positionTmp1.y = 18.4f - (test / 10);
                    positionTmp2.y = 18.4f - (test / 10);
                    tmp1.transform.position = positionTmp1;
                    tmp2.transform.position = positionTmp2;
                }
            }
        }
    }

	public void Pause()
	{
		if (pauseEnable == false)
		{
			pauseGame();
			if (GameObject.Find("Timer"))
				GameObject.Find("Timer").GetComponent<TimerBehavior>().isPause = true;
			if (GameObject.Find("TimerUltra"))
				GameObject.Find("TimerUltra").GetComponent<TimerUltraBehavior>().isPause = true;
			if (GameObject.Find("TimerCleaning"))
				GameObject.Find("TimerCleaning").GetComponent<TimerCleaningBehavior>().isPause = true;
		}
		else
		{
			if (GameObject.Find("Timer"))
				GameObject.Find("Timer").GetComponent<TimerBehavior>().isPause = false;
			if (GameObject.Find("TimerUltra"))
				GameObject.Find("TimerUltra").GetComponent<TimerUltraBehavior>().isPause = false;
			if (GameObject.Find("TimerCleaning"))
				GameObject.Find("TimerCleaning").GetComponent<TimerCleaningBehavior>().isPause = false;
			unPauseGame();
		}
	}

    void pauseGame()
    {
        Cursor.visible = true;
        aSources[2].Play();
        GameObject.Find("PauseScreen").GetComponent<Animator>().SetBool("isPaused", true);
        currentAudioSource.Pause();
        //Time.timeScale = 0;
        pauseEnable = true;
    }

    public void unPauseGame()
    {
        Cursor.visible = false;
        aSources[2].Play();
        GameObject.Find("PauseScreen").GetComponent<Animator>().SetBool("isPaused", false);
        currentAudioSource.Play();
        //Time.timeScale = 1;
        pauseEnable = false;
        if (sexyEnable == false)
        {
            tmp1 = GameObject.Find("BackgroundKremlin");
            tmp1.GetComponent<BackgroundKremlinBehaviour>().Increase();
        }

    }

    public void toggleSexy()
    {
        if (sexyEnable == false)
        {
            sexyEnable = true;
            aSources[3 + sexyVoiceLine].Play();
        }
        else
        {
            sexyEnable = false;
			aSources[5 + sexyVoiceLine].Play();
        }
    }

    public void toggleSexyHotKey()
    {
        Toggle tugle = GameObject.FindGameObjectWithTag("SexyToggle").GetComponent<Toggle>();
        if (sexyEnable == false)
        {
            tugle.isOn = true;
        }
        else
        {
            tugle.isOn = false;
        }
        if (sexyEnable == false)
        {
            tmp1 = GameObject.Find("BackgroundKremlin");
            tmp1.GetComponent<BackgroundKremlinBehaviour>().Increase();
        }
    }

    public void reloadScene()
    {
		PlayerPrefs.SetInt("SaveGame", 0);
        SceneManager.LoadScene("GetReady");
    }

    public void quitGame()
    {
		SaveGame();
        Application.Quit();
    }

    void RefreshMusicVisualizer()
    {
        musicVisualizerDelay = true;
        refreshing = false;
    }

    void LoweringVolume()
    {
        volumeDown = true;
    }

    public void GameOver()
    {
        if (GameObject.Find("Timer"))
            GameObject.Find("Timer").GetComponent<TimerBehavior>().isEnd = true;
        if (GameObject.Find("TimerUltra"))
            GameObject.Find("TimerUltra").GetComponent<TimerUltraBehavior>().isEnd = true;
		if (GameObject.Find("TimerCleaning"))
			GameObject.Find("TimerCleaning").GetComponent<TimerCleaningBehavior>().isEnd = true;
        bool volumeDownGameOver = false;
        gameOver = true;
        currentAudioSource.Stop();
        if (currentAudioSource.volume == 0)
            volumeDownGameOver = true;
        currentAudioSource = aSources[1];
        currentAudioSourceOldVolume = currentAudioSource.volume;
        if (volumeDownGameOver == true)
            currentAudioSource.volume = 0;
        currentAudioSource.Play();
        GameObject.Find("NewRecord").GetComponent<SpriteRenderer>().enabled = false;
        /*Debug.Log("Score: "+ScoreManager.GetComponent<ScoreBehaviour>().score);*/
        if (ScoreManager.GetComponent<ScoreBehaviour>().score >= PlayerPrefs.GetInt("HighScore"))
        {
            GameObject.Find("NewRecord").GetComponent<SpriteRenderer>().enabled = true;
            GameObject.Find("GameOverText").GetComponent<SpriteRenderer>().enabled = false;
            PlayerPrefs.SetInt("HighScore", ScoreManager.GetComponent<ScoreBehaviour>().score);
            PlayerPrefs.SetInt("HighScoreLevel", ScoreManager.GetComponent<ScoreBehaviour>().level);
            PlayerPrefs.SetInt("HighScoreLines", ScoreManager.GetComponent<ScoreBehaviour>().lines);
        }
        GameObject.Find("GameOverScreen").GetComponent<Animator>().SetBool("GameOver", true);
        GameObject.Find("BackgroundKremlinGameOver").GetComponent<Animator>().SetBool("GameOver", true);
		if (ScoreManager.GetComponent<ScoreBehaviour>().lines >= 200)
		{
			if (PlayerPrefs.GetString ("UnlockedTetrominoesStyles", "10000000000000000000") [3] == '1')
			{
				DisplayPopup ("Dark Communism");
				return;
			}
			string tmpUnlockedTetrominoesStyles = "";
			tmpUnlockedTetrominoesStyles += PlayerPrefs.GetString ("UnlockedTetrominoesStyles", "10000000000000000000");
			tmpUnlockedTetrominoesStyles = tmpUnlockedTetrominoesStyles.Remove(3,1);
			tmpUnlockedTetrominoesStyles = tmpUnlockedTetrominoesStyles.Insert(3,"1");
			PlayerPrefs.SetString ("UnlockedTetrominoesStyles", tmpUnlockedTetrominoesStyles);
			DisplayPopup ("Unlocked:\nDark Communism");
		}
    }

    public void EndFortyLines()
    {
        if (GameObject.Find("Timer"))
            GameObject.Find("Timer").GetComponent<TimerBehavior>().isEnd = true;
        bool volumeDownGameOver = false;
        gameOver = true;
        currentAudioSource.Stop();
        if (currentAudioSource.volume == 0)
            volumeDownGameOver = true;
        currentAudioSource = aSources[1];
        currentAudioSourceOldVolume = currentAudioSource.volume;
        if (volumeDownGameOver == true)
            currentAudioSource.volume = 0;
        currentAudioSource.Play();
        GameObject.Find("NewRecord").GetComponent<SpriteRenderer>().enabled = false;
		if (PlayerPrefs.GetInt("FortyLinesRecord") == 0
			|| (GameObject.Find("Timer").GetComponent<TimerBehavior>().t < PlayerPrefs.GetFloat("FortyLinesBestTime") && ScoreManager.GetComponent<ScoreBehaviour>().lines >= 40)
			|| (PlayerPrefs.GetInt("FortyLinesBestLines") < 40 && ScoreManager.GetComponent<ScoreBehaviour>().lines > PlayerPrefs.GetInt("FortyLinesBestLines")))
        {
            GameObject.Find("NewRecord").GetComponent<SpriteRenderer>().enabled = true;
            GameObject.Find("GameOverText").GetComponent<SpriteRenderer>().enabled = false;
            PlayerPrefs.SetInt("FortyLinesRecord", 1);
            PlayerPrefs.SetFloat("FortyLinesBestTime", GameObject.Find("Timer").GetComponent<TimerBehavior>().t);
            PlayerPrefs.SetInt("FortyLinesBestLines", ScoreManager.GetComponent<ScoreBehaviour>().lines);
        }
        GameObject.Find("GameOverScreen").GetComponent<Animator>().SetBool("GameOver", true);
        GameObject.Find("BackgroundKremlinGameOver").GetComponent<Animator>().SetBool("GameOver", true);
		//Succes under 2m00s
		if (GameObject.Find ("Timer").GetComponent<TimerBehavior> ().t < 120.0f && ScoreManager.GetComponent<ScoreBehaviour>().lines >= 40)
		{
			if (PlayerPrefs.GetString ("UnlockedTetrominoesStyles", "10000000000000000000") [1] == '1')
			{
				DisplayPopup ("Smooth Vodka");
			}
			else
			{
				string tmpUnlockedTetrominoesStyles = "";
				tmpUnlockedTetrominoesStyles += PlayerPrefs.GetString ("UnlockedTetrominoesStyles", "10000000000000000000");
				tmpUnlockedTetrominoesStyles = tmpUnlockedTetrominoesStyles.Remove(1,1);
				tmpUnlockedTetrominoesStyles = tmpUnlockedTetrominoesStyles.Insert(1,"1");
				PlayerPrefs.SetString ("UnlockedTetrominoesStyles", tmpUnlockedTetrominoesStyles);
				DisplayPopup ("Unlocked:\nSmooth Vodka");	
			}
		}
    }

    public void EndUltra()
    {
        if (GameObject.Find("TimerUltra"))
            GameObject.Find("TimerUltra").GetComponent<TimerUltraBehavior>().isEnd = true;
        bool volumeDownGameOver = false;
        gameOver = true;
        currentAudioSource.Stop();
        if (currentAudioSource.volume == 0)
            volumeDownGameOver = true;
        currentAudioSource = aSources[1];
        currentAudioSourceOldVolume = currentAudioSource.volume;
        if (volumeDownGameOver == true)
            currentAudioSource.volume = 0;
        currentAudioSource.Play();
        GameObject.Find("NewRecord").GetComponent<SpriteRenderer>().enabled = false;
        if (PlayerPrefs.GetInt("UltraRecord") == 0 || ScoreManager.GetComponent<ScoreBehaviour>().score >= PlayerPrefs.GetInt("UltraScore"))
        {
            GameObject.Find("NewRecord").GetComponent<SpriteRenderer>().enabled = true;
            GameObject.Find("GameOverText").GetComponent<SpriteRenderer>().enabled = false;
            PlayerPrefs.SetInt("UltraRecord", 1);
            PlayerPrefs.SetInt("UltraScore", ScoreManager.GetComponent<ScoreBehaviour>().score);
            PlayerPrefs.SetInt("UltraLevel", ScoreManager.GetComponent<ScoreBehaviour>().level);
            PlayerPrefs.SetInt("UltraLines", ScoreManager.GetComponent<ScoreBehaviour>().lines);
        }
        GameObject.Find("GameOverScreen").GetComponent<Animator>().SetBool("GameOver", true);
        GameObject.Find("BackgroundKremlinGameOver").GetComponent<Animator>().SetBool("GameOver", true);
		if (ScoreManager.GetComponent<ScoreBehaviour>().score >= 50000)
		{
			if (PlayerPrefs.GetString ("UnlockedTetrominoesStyles", "10000000000000000000") [4] == '1')
			{
				DisplayPopup ("Mein Kampf");
				return;
			}
			string tmpUnlockedTetrominoesStyles = "";
			tmpUnlockedTetrominoesStyles += PlayerPrefs.GetString ("UnlockedTetrominoesStyles", "10000000000000000000");
			tmpUnlockedTetrominoesStyles = tmpUnlockedTetrominoesStyles.Remove(4,1);
			tmpUnlockedTetrominoesStyles = tmpUnlockedTetrominoesStyles.Insert(4,"1");
			PlayerPrefs.SetString ("UnlockedTetrominoesStyles", tmpUnlockedTetrominoesStyles);
			DisplayPopup ("Unlocked:\nMein Kampf");
		}
    }

	public void EndCleaning()
	{
		if (GameObject.Find("TimerCleaning"))
			GameObject.Find("TimerCleaning").GetComponent<TimerCleaningBehavior>().isEnd = true;
		bool volumeDownGameOver = false;
		gameOver = true;
		currentAudioSource.Stop();
		if (currentAudioSource.volume == 0)
			volumeDownGameOver = true;
		currentAudioSource = aSources[1];
		currentAudioSourceOldVolume = currentAudioSource.volume;
		if (volumeDownGameOver == true)
			currentAudioSource.volume = 0;
		currentAudioSource.Play();
		GameObject.Find("NewRecord").GetComponent<SpriteRenderer>().enabled = false;
		if ((PlayerPrefs.GetInt("CleaningRecord") == 0 && GridBehavior.NbGarbage == 0)
			|| (GameObject.Find("TimerCleaning").GetComponent<TimerCleaningBehavior>().t < PlayerPrefs.GetFloat("CleaningBestTime") && GridBehavior.NbGarbage == 0))
		{
			GameObject.Find("NewRecord").GetComponent<SpriteRenderer>().enabled = true;
			GameObject.Find("GameOverText").GetComponent<SpriteRenderer>().enabled = false;
			PlayerPrefs.SetInt("CleaningRecord", 1);
			PlayerPrefs.SetFloat("CleaningBestTime", GameObject.Find("TimerCleaning").GetComponent<TimerCleaningBehavior>().t);
			PlayerPrefs.SetInt("CleaningBestLines", ScoreManager.GetComponent<ScoreBehaviour>().lines);
		}
		GameObject.Find("GameOverScreen").GetComponent<Animator>().SetBool("GameOver", true);
		GameObject.Find("BackgroundKremlinGameOver").GetComponent<Animator>().SetBool("GameOver", true);
		if (GameObject.Find ("TimerCleaning").GetComponent<TimerCleaningBehavior> ().t < 60.0f * 8.0f && GridBehavior.NbGarbage == 0)
		{
			if (PlayerPrefs.GetString ("UnlockedTetrominoesStyles", "10000000000000000000") [2] == '1')
			{
				DisplayPopup ("Trump's Wall");
				return;
			}
			string tmpUnlockedTetrominoesStyles = "";
			tmpUnlockedTetrominoesStyles += PlayerPrefs.GetString ("UnlockedTetrominoesStyles", "10000000000000000000");
			tmpUnlockedTetrominoesStyles = tmpUnlockedTetrominoesStyles.Remove(2,1);
			tmpUnlockedTetrominoesStyles = tmpUnlockedTetrominoesStyles.Insert(2,"1");
			PlayerPrefs.SetString ("UnlockedTetrominoesStyles", tmpUnlockedTetrominoesStyles);
			DisplayPopup ("Unlocked:\nTrump's Wall");
		}
	}

	public void OneLine()
	{
		StartCoroutine(CameraZoom(0.10f));
	}

	public void TwoLines()
	{
		StartCoroutine(CameraZoom(0.10f));
	}

	public void ThreeLines()
	{
		StartCoroutine(CameraZoom(0.10f));
	}

	public int BoysAreDancing = 0;

	public void TetrisDance()
	{
		++BoysAreDancing;
		StartCoroutine(DanceMyBoys());
		StartCoroutine(CameraZoom(0.20f));
	}

    IEnumerator DanceMyBoys()
    {
		foreach (Transform dancer in TetrisDancers.transform)
		{
			dancer.GetComponent<SpriteRenderer>().enabled = true;
		}
        yield return new WaitForSeconds(2.0f);
		--BoysAreDancing;
		if (BoysAreDancing == 0)
		{
			foreach (Transform dancer in TetrisDancers.transform)
			{
				dancer.GetComponent<SpriteRenderer>().enabled = false;
			}
		}
    }

	IEnumerator CameraZoom(float zoomDuration)
	{
		mainCamera.orthographicSize = 9.9f;
		yield return new WaitForSeconds(zoomDuration);
		mainCamera.orthographicSize = 10.0f;
	}

    public void ReturnToMenu()
    {
		SaveGame();
        SceneManager.LoadScene("mainMenu");
    }

	private void SaveGame()
	{
		if (gameOver == true)
		{
			PlayerPrefs.SetInt("SaveGame", 0);
			return;
		}
		int GameType = PlayerPrefs.GetInt("GameType");
		GameObject spawner = GameObject.Find ("Spawner");

		PlayerPrefs.SetInt("SaveGame", 1);
		PlayerPrefs.SetInt("SaveGameGameType", GameType);
		if (GameType == 1)
			PlayerPrefs.SetFloat ("SaveGameTimer", GameObject.Find ("Timer").GetComponent<TimerBehavior> ().t);
		else if (GameType == 2)
			PlayerPrefs.SetFloat ("SaveGameTimerUltra", GameObject.Find ("TimerUltra").GetComponent<TimerUltraBehavior> ().t);
		else if (GameType == 3)
			PlayerPrefs.SetFloat("SaveGameTimerCleaning", GameObject.Find("TimerCleaning").GetComponent<TimerCleaningBehavior>().t);
					
		PlayerPrefs.SetInt("SaveGameScore", ScoreManager.GetComponent<ScoreBehaviour>().score);
		PlayerPrefs.SetInt("SaveGameLevel", ScoreManager.GetComponent<ScoreBehaviour>().level);
		PlayerPrefs.SetInt("SaveGameLines", ScoreManager.GetComponent<ScoreBehaviour>().lines);
		PlayerPrefs.SetInt("SaveGameLinesNeeded", ScoreManager.GetComponent<ScoreBehaviour>().linesNeeded);
		PlayerPrefs.SetInt("SaveGameLastLinesNeeded", ScoreManager.GetComponent<ScoreBehaviour>().lastLinesNeeded);
		PlayerPrefs.SetInt("SaveGameTetrominoes", ScoreManager.GetComponent<ScoreBehaviour>().tetrominoes);
		PlayerPrefs.SetFloat("SaveGameSpeed", ScoreManager.GetComponent<ScoreBehaviour>().speed);
		string gridContainer = "";
		string tmpTetromino = null;
		for (int y = 0; y < GridBehavior.h; ++y)
		{
			for (int x = 0; x < GridBehavior.w; ++x)
			{
				if (GridBehavior.grid [x, y] != null && GridBehavior.grid [x, y].gameObject.layer == 10) //10 = LockedBlock
				{
					tmpTetromino = GridBehavior.grid [x, y].gameObject.GetComponent<SingleBlockIdentity>().Tetromino;
					switch (tmpTetromino)
					{
					case "I":
						gridContainer += "0";
						break;
					case "J":
						gridContainer += "1";
						break;
					case "L":
						gridContainer += "2";
						break;
					case "O":
						gridContainer += "3";
						break;
					case "S":
						gridContainer += "4";
						break;
					case "T":
						gridContainer += "5";
						break;
					case "Z":
						gridContainer += "6";
						break;
					}
					if (tmpTetromino == "G")
						gridContainer += "G00";
					else
					{
						gridContainer += GridBehavior.grid [x, y].gameObject.GetComponent<SingleBlockIdentity>().Id;
						if (GridBehavior.grid [x, y].parent && GridBehavior.grid [x, y].parent.GetComponent<Group>())
							gridContainer += ZRotationToSaveGame(GridBehavior.grid [x, y].parent.transform.eulerAngles.z); // Si le block vient d'un Group
						else
							gridContainer += ZRotationToSaveGame(GridBehavior.grid [x, y].eulerAngles.z); // Si le block ne vient pas d'un Group (load game par exemple)
					}
				}
				else
					gridContainer += "N00";
			}
		}
		//Debug.Log ("    [DEBUG]    Grid Container = " + gridContainer);
		PlayerPrefs.SetString("SaveGameGrid", gridContainer);
		PlayerPrefs.SetString("SaveGameCurrentGroup", GameObject.FindGameObjectWithTag("AliveGroup").GetComponent<Group>().tetromino);
		PlayerPrefs.SetInt("SaveGameNext01", spawner.transform.Find("Next01").GetChild(0).GetComponent<NextBehaviour>().type);
		PlayerPrefs.SetInt("SaveGameNext02", spawner.transform.Find("Next02").GetChild(0).GetComponent<NextBehaviour>().type);
		PlayerPrefs.SetInt("SaveGameNext03", spawner.transform.Find("Next03").GetChild(0).GetComponent<NextBehaviour>().type);
		PlayerPrefs.SetInt("SaveGameNext04", spawner.transform.Find("Next04").GetChild(0).GetComponent<NextBehaviour>().type);
		PlayerPrefs.SetInt("SaveGameNext05", spawner.transform.Find("Next05").GetChild(0).GetComponent<NextBehaviour>().type);
		if (spawner.GetComponent<Spawner>().holded != null)
			PlayerPrefs.SetInt("SaveGameHolded", spawner.GetComponent<Spawner>().holded.GetComponent<NextBehaviour>().type);
		else
			PlayerPrefs.SetInt("SaveGameHolded", -1);
	}

	private string ZRotationToSaveGame(float z)
	{
		if (z > 89.0F && z < 91.0F)
			return "1";
		else if (z > 179.0F && z < 181.0F)
			return "2";
		else if (z > 269.0F && z < 271.0F)
			return "3";
		else
			return "0";
	}

	private float ZRotationToLoadGame(string z)
	{
		if (z == "1")
			return 90.0F;
		else if (z == "2")
			return 180.0F;
		else if (z == "3")
			return 270.0F;
		else
			return 0.0F;
	}

	private void LoadSaveGame()
	{
		Spawner spawner = GameObject.Find ("Spawner").GetComponent<Spawner> ();

		ScoreManager.GetComponent<ScoreBehaviour>().score = PlayerPrefs.GetInt("SaveGameScore");
		ScoreManager.GetComponent<ScoreBehaviour>().level = PlayerPrefs.GetInt("SaveGameLevel");
		ScoreManager.GetComponent<ScoreBehaviour>().lines = PlayerPrefs.GetInt("SaveGameLines");
		ScoreManager.GetComponent<ScoreBehaviour>().linesNeeded = PlayerPrefs.GetInt("SaveGameLinesNeeded");
		ScoreManager.GetComponent<ScoreBehaviour>().lastLinesNeeded = PlayerPrefs.GetInt("SaveGameLastLinesNeeded");
		ScoreManager.GetComponent<ScoreBehaviour>().tetrominoes = PlayerPrefs.GetInt("SaveGameTetrominoes");
		ScoreManager.GetComponent<ScoreBehaviour>().speed = PlayerPrefs.GetFloat("SaveGameSpeed");
		ScoreManager.GetComponent<ScoreBehaviour>().AddToScore(0);
		ScoreManager.GetComponent<ScoreBehaviour>().AddToLines(0);
		ScoreManager.GetComponent<ScoreBehaviour>().AddToTetromiones(0);
		ScoreManager.GetComponent<ScoreBehaviour>().RemoveFromNeeded(0);
		ScoreManager.GetComponent<ScoreBehaviour>().UpdateLevel();
		string gridContainer = PlayerPrefs.GetString("SaveGameGrid");
		int yi = 0;
		int tmpNbGarbage = 0;
		for (int y = 0; y < GridBehavior.h; ++y)
		{
			int xi = 0;
			for (int x = 0; x < GridBehavior.w; ++x)
			{
				if (gridContainer [yi + xi] == 'N')
					GridBehavior.grid [x, y] = null;
				else if (gridContainer [yi + xi] == 'G')
				{
					var tmp = Instantiate (spawner.GarbageGroup, new Vector3 (x, y, 0), spawner.GarbageGroup.transform.rotation);
					GridBehavior.grid [x, y] = tmp.transform;
					++tmpNbGarbage;
				}	
				else
				{
					int tmpIdContainer;
					int.TryParse (gridContainer [yi + xi].ToString(), out tmpIdContainer);
					int tmpChildIdContainer;
					int.TryParse (gridContainer [yi + xi + 1].ToString(), out tmpChildIdContainer);
					var tmpChild = spawner.groups [tmpIdContainer].transform.GetChild (tmpChildIdContainer);
					var tmp = Instantiate (tmpChild, new Vector3 (x, y, 0), tmpChild.transform.rotation);
					tmp.gameObject.layer = 10; //10 = LockedBlock
					tmp.Rotate(0,0,ZRotationToLoadGame(gridContainer [yi + xi + 2].ToString()));
					GridBehavior.grid [x, y] = tmp.transform;
				}
				xi += 3;
			}
			yi += GridBehavior.w * 3;
		}
		if (PlayerPrefs.GetInt("SaveGameGameType") == 3)
			GridBehavior.NbGarbage = tmpNbGarbage;
	}

	public void DisplayPopup(string text)
	{
		popup.GetComponent<PopupBehavior> ().PlayPopupSound ();
		popup.transform.GetChild (1).GetComponent<UnityEngine.UI.Text> ().text = text;
		popup.transform.GetChild (0).GetComponent<SpriteRenderer> ().color = new Color (1.0f, 1.0f, 1.0f, 1.0f);
		popup.transform.GetChild (1).GetComponent<UnityEngine.UI.Text> ().color = new Color (204.0f/255.0f, 190.0f/255.0f, 155.0f/255.0f, 1.0f);
		++nbFadingPopups;
		Invoke ("CoolDownBeforeFadingPopup", 4.0f);
	}

	private void CoolDownBeforeFadingPopup()
	{
		FadePopup();
	}

	void FadePopup()
	{
		float tmpAlpha = 0.0f;
		if (nbFadingPopups <= 1)
		{
			tmpAlpha = popup.transform.GetChild (0).GetComponent<SpriteRenderer> ().color.a;
			tmpAlpha -= 0.05f;
			popup.transform.GetChild (0).GetComponent<SpriteRenderer> ().color = new Color(1.0f, 1.0f, 1.0f, tmpAlpha);
			popup.transform.GetChild (1).GetComponent<UnityEngine.UI.Text> ().color = new Color(204.0f/255.0f, 190.0f/255.0f, 155.0f/255.0f, tmpAlpha);
		}
		if (tmpAlpha > 0 && nbFadingPopups <= 1)
			Invoke ("FadePopup", 0.05f);
		else
			--nbFadingPopups;
	}
}