using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GetReadyBehavior : MonoBehaviour
{
    public GameObject[] Dancer;
    public Sprite SecondState;
    public AudioSource NationalAnthem;

    private float bip = 0.2f;

	// Use this for initialization
	void Start ()
    {
        Cursor.visible = false;
		NationalAnthem.volume = NationalAnthem.volume * ((float)PlayerPrefs.GetInt("EffectsVolume") / 10);
        NationalAnthem.Play();
        StartCoroutine(DanceMyBoys());
    }

    IEnumerator DanceMyBoys()
    {
        int GameType = PlayerPrefs.GetInt("GameType");

        yield return new WaitForSeconds(bip);
        Dancer[0].GetComponent<SpriteRenderer>().sprite = SecondState;
        yield return new WaitForSeconds(bip);
        Dancer[1].GetComponent<SpriteRenderer>().sprite = SecondState;
        yield return new WaitForSeconds(bip);
        Dancer[2].GetComponent<SpriteRenderer>().sprite = SecondState;
        yield return new WaitForSeconds(bip);
        Dancer[3].GetComponent<SpriteRenderer>().sprite = SecondState;
        yield return new WaitForSeconds(bip);
        if (GameType == 0)
            SceneManager.LoadScene("Marathon");
        else if (GameType == 1)
            SceneManager.LoadScene("FortyLines");
		else if (GameType == 2)
            SceneManager.LoadScene("Ultra");
		else
			SceneManager.LoadScene("Cleaning");
    }
}
