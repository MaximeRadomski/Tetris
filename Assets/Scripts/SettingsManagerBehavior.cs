using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SettingsManagerBehavior : MonoBehaviour
{
	public Sprite VolumeOn, VolumeOff;

	public Sprite FemaleOn, FemaleOff;
	public Sprite MaleOn, MaleOff;

    private int i;
    int currentMax;
	private GameObject popup;
	private int nbFadingPopups = 0;

    // Use this for initialization
    void Start ()
    {
        i = 1;
        currentMax = PlayerPrefs.GetInt("MusicVolume");
		popup = GameObject.Find ("Popup");
		popup.GetComponent<AudioSource>().volume = 1 * ((float)PlayerPrefs.GetInt("EffectsVolume") / 10);
        while (i <= currentMax)
        {
            GameObject.Find("Music"+i).GetComponent<SpriteRenderer>().sprite = VolumeOn;
            ++i;
        }
        while (i <= 10)
        {
            GameObject.Find("Music" + i).GetComponent<SpriteRenderer>().sprite = VolumeOff;
            ++i;
        }
        i = 1;
        currentMax = PlayerPrefs.GetInt("EffectsVolume");
        while (i <= currentMax)
        {
            GameObject.Find("Effect" + i).GetComponent<SpriteRenderer>().sprite = VolumeOn;
            ++i;
        }
        while (i <= 10)
        {
            GameObject.Find("Effect" + i).GetComponent<SpriteRenderer>().sprite = VolumeOff;
            ++i;
        }
        GameObject.Find("$PauseMenuManager").GetComponent<PauseMenuBehavior>().AlterEffectsVolume();
        GameObject.Find("MenuMusic").GetComponent<MenuMusic>().AlterMusicVolume();
		if (Application.platform == RuntimePlatform.Android || Application.platform == RuntimePlatform.IPhonePlayer) {
			GameObject.Find("Text").GetComponent<UnityEngine.UI.Text>().enabled = false;
			GameObject.Find("HowToPlay").GetComponent<UnityEngine.UI.Text>().enabled = true;
		}

		// HANDLE SEXY //
		if (PlayerPrefs.GetInt ("SexyGenre") == 0)
		{
			PlayerPrefs.SetInt("SexyGenre", 2); //1 for male, 2 for female. Female first.
			GameObject.Find("GenderFemale").GetComponent<SpriteRenderer>().sprite = FemaleOn;
			GameObject.Find("GenderMale").GetComponent<SpriteRenderer>().sprite = MaleOff;
		}
		if (PlayerPrefs.GetInt ("SexyGenre") == 1)
		{
			GameObject.Find("GenderFemale").GetComponent<SpriteRenderer>().sprite = FemaleOff;
			GameObject.Find("GenderMale").GetComponent<SpriteRenderer>().sprite = MaleOn;
		}
		else if (PlayerPrefs.GetInt ("SexyGenre") == 2)
		{
			GameObject.Find("GenderFemale").GetComponent<SpriteRenderer>().sprite = FemaleOn;
			GameObject.Find("GenderMale").GetComponent<SpriteRenderer>().sprite = MaleOff;
		}
		SetActiveStyles();
		ChangeTetrominoesStyle(PlayerPrefs.GetInt("TetrominoesStyle"));
		SetActiveCurtainStyles();
		ChangeCurtainStyle(PlayerPrefs.GetInt("CurtainsStyle"));
    }

	private void SetActiveStyles()
	{
		var tmpUnlockedTetrominoesStyles = PlayerPrefs.GetString ("UnlockedTetrominoesStyles", "10000000000000000000");
		for (int i = 1; i< tmpUnlockedTetrominoesStyles.Length; ++i)
		{
			if (tmpUnlockedTetrominoesStyles [i] == '1')
				GameObject.Find ("Style" + i.ToString ("D2")).GetComponent<SpriteRenderer> ().color = new Color (1.0f, 1.0f, 1.0f, 1.0f);
		}
	}

	private void SetActiveCurtainStyles()
	{
		var tmpUnlockedCurtainsStyles = PlayerPrefs.GetString ("UnlockedCurtainsStyles", "10000000000000000000");
		for (int i = 1; i< tmpUnlockedCurtainsStyles.Length; ++i)
		{
			if (tmpUnlockedCurtainsStyles [i] == '1')
				GameObject.Find ("StyleCurtains" + i.ToString ("D2")).GetComponent<SpriteRenderer> ().color = new Color (1.0f, 1.0f, 1.0f, 1.0f);
		}
	}

	public void DisplayHowToPlay()
	{
		GameObject.Find ("TeturssHowToPlay").GetComponent<SpriteRenderer> ().enabled = true;
		GameObject.Find ("Canvas").GetComponent<Canvas> ().enabled = false;
	}

	public void HideHowToPlay()
	{
		GameObject.Find ("TeturssHowToPlay").GetComponent<SpriteRenderer> ().enabled = false;
		GameObject.Find ("Canvas").GetComponent<Canvas> ().enabled = true;
	}

	public void ChangeSexyGender()
	{
		if (PlayerPrefs.GetInt ("SexyGenre") == 1)
		{
			PlayerPrefs.SetInt("SexyGenre", 2);
			GameObject.Find("GenderFemale").GetComponent<SpriteRenderer>().sprite = FemaleOn;
			GameObject.Find("GenderMale").GetComponent<SpriteRenderer>().sprite = MaleOff;
		}
		else if (PlayerPrefs.GetInt ("SexyGenre") == 2)
		{
			PlayerPrefs.SetInt("SexyGenre", 1);
			GameObject.Find("GenderFemale").GetComponent<SpriteRenderer>().sprite = FemaleOff;
			GameObject.Find("GenderMale").GetComponent<SpriteRenderer>().sprite = MaleOn;
		}
		DisplayPopup("A Secret is hidden in the pause menu");
	}

	void Update()
	{
		if (Input.GetMouseButtonDown (0) && GameObject.Find ("TeturssHowToPlay").GetComponent<SpriteRenderer> ().enabled == true)
		{
			HideHowToPlay ();
		}
		if (Input.GetKeyDown(KeyCode.Escape))
		{
			BackToMenu();
		}
	}

    public void MusicLess()
    {
        currentMax = PlayerPrefs.GetInt("MusicVolume");

        if (currentMax > 0)
        {
            PlayerPrefs.SetInt("MusicVolume", currentMax - 1);
            Start();
        }
    }

    public void MusicMore()
    {
        currentMax = PlayerPrefs.GetInt("MusicVolume");

        if (currentMax < 10)
        {
            PlayerPrefs.SetInt("MusicVolume", currentMax + 1);
            Start();
        }
    }

    public void EffectsLess()
    {
        currentMax = PlayerPrefs.GetInt("EffectsVolume");

        if (currentMax > 0)
        {
            PlayerPrefs.SetInt("EffectsVolume", currentMax - 1);
            Start();
        }
    }

    public void EffectsMore()
    {
        currentMax = PlayerPrefs.GetInt("EffectsVolume");

        if (currentMax < 10)
        {
            PlayerPrefs.SetInt("EffectsVolume", currentMax + 1);
            Start();
        }
    }

    public void BackToMenu()
    {
        SceneManager.LoadScene("mainMenu");
    }

	public void ChangeTetrominoesStyle(int styleId)
	{
		if (IsThisStyleAvailable (styleId) == false)
			return;
		PlayerPrefs.SetInt("TetrominoesStyle", styleId);
		var currentStyle = GameObject.Find ("Style" + styleId.ToString("D2"));
		var selectedStyle = GameObject.Find ("SelectedStyle");
		selectedStyle.transform.position = currentStyle.transform.position;
		var styleTitle = GameObject.Find ("StyleName").GetComponent<UnityEngine.UI.Text>();
		switch (styleId)
		{
		case 0:
			styleTitle.text = "Classic Communism";
			break;
		case 1:
			styleTitle.text = "Smooth Vodka";
			break;
		case 2:
			styleTitle.text = "Trump's wall";
			break;
		case 3:
			styleTitle.text = "Dark Communism";
			break;
		case 4:
			styleTitle.text = "Mein Kampf";
			break;
		case 5:
			styleTitle.text = "Soviet Nostalgia";
			break;
		}
	}

	private bool IsThisStyleAvailable(int styleId)
	{
		var unlockedStyles = PlayerPrefs.GetString ("UnlockedTetrominoesStyles", "10000000000000000000");
		if (unlockedStyles [styleId] == '1')
			return true;
		switch (styleId)
		{
		case 1:
			DisplayPopup("Complete a 40 Lines game in less than 2 minutes.\nUnlocks:\nSmooth Vodka");
			break;
		case 2:
			DisplayPopup("Complete a Cleaning game in less than 8 minutes.\nUnlocks:\nTrump's Wall");
			break;
		case 3:
			DisplayPopup("Do more than 200 lines in a Marathon game.\nUnlocks:\nDark Communism");
			break;
		case 4:
			DisplayPopup("Complete an Ultra game with more than 50k points.\nUnlocks:\nMein Kampf");
			break;
		case 5:
			DisplayPopup("Do a x8 Combo in any game mode.\nUnlocks:\nSoviet Nostalgia");
			break;
		}
		return false;
	}

	public void ChangeCurtainStyle(int styleId)
	{
		if (IsThisCurtainStyleAvailable (styleId) == false)
			return;
		PlayerPrefs.SetInt("CurtainsStyle", styleId);
		var currentStyle = GameObject.Find ("StyleCurtains" + styleId.ToString("D2"));
		var selectedStyle = GameObject.Find ("SelectedStyleCurtains");
		selectedStyle.transform.position = currentStyle.transform.position;
		var styleTitle = GameObject.Find ("StyleCurtainsName").GetComponent<UnityEngine.UI.Text>();
		switch (styleId)
		{
		case 0:
			styleTitle.text = "Classic Curtains";
			break;
		case 1:
			styleTitle.text = "Grandma's Carpet";
			break;
		case 2:
			styleTitle.text = "Baguette Deja Vu";
			break;
		case 3:
			styleTitle.text = "Fifth Reich";
			break;
		}
		GameObject.Find ("Curtains").GetComponent<CurtainsBehavior> ().UpdateSprites();
	}

	private bool IsThisCurtainStyleAvailable(int styleId)
	{
		var unlockedCurtainsStyles = PlayerPrefs.GetString ("UnlockedCurtainsStyles", "10000000000000000000");
		if (unlockedCurtainsStyles [styleId] == '1')
			return true;
		switch (styleId)
		{
		case 1:
			DisplayPopup("Do four Tetrises in a row.\nUnlocks:\nGrandma's Carpet", true);
			break;
		case 2:
			DisplayPopup("Uncover your sexy bolshevik.\nUnlocks:\nBaguette Deja Vu", true);
			break;
		case 3:
			DisplayPopup("Do a triple Tetris combo.\nUnlocks:\nFifth Reich", true);
			break;
		}
		return false;
	}

	private void DisplayPopup(string text, bool isCurtain = false)
	{
		if (isCurtain == false && popup.transform.position.x > 0.0f)
			popup.transform.localPosition = new Vector3 (popup.transform.localPosition.x * -1.0f, popup.transform.localPosition.y, 0.0f); // Met la popup à gauche
		else if (isCurtain == true && popup.transform.position.x < 0.0f)
			popup.transform.localPosition = new Vector3 (popup.transform.localPosition.x * -1.0f, popup.transform.localPosition.y, 0.0f); // Met la popup à droite
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
