using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MemorialFortyLinesBehavior : MonoBehaviour
{
    public GameObject[] minutesNumbers;
    public GameObject[] secondsNumbers;
    public GameObject[] millisecondsNumbers;
    public GameObject[] numberLines;
    public Sprite[] spriteNumbers;
    public GameObject scoreNumberSample;

    // Use this for initialization
    void Start()
    {
        int i = 0;
        while (i < 3)
        {
            millisecondsNumbers[i] = Instantiate(scoreNumberSample, new Vector3(9.85f - (i + 1) * 1.2f, 16.5f, 0), transform.rotation) as GameObject;
            ++i;
        }
        i = 0;
        while (i < 2)
        {
            secondsNumbers[i] = Instantiate(scoreNumberSample, new Vector3(5.7f - (i + 1) * 1.2f, 16.5f, 0), transform.rotation) as GameObject;
            ++i;
        }
        i = 0;
        while (i < 2)
        {
            minutesNumbers[i] = Instantiate(scoreNumberSample, new Vector3(2.8f - (i + 1) * 1.2f, 16.5f, 0), transform.rotation) as GameObject;
            ++i;
        }
        i = 0;
        while (i < 2)
        {
            numberLines[i] = Instantiate(scoreNumberSample, new Vector3(6.2f - (i + 1) * 1.2f, 11.5f, 0), transform.rotation) as GameObject;
            ++i;
        }
        if (PlayerPrefs.GetInt("FortyLinesRecord") == 0)
            return;
        AddTo(numberLines, PlayerPrefs.GetInt("FortyLinesBestLines").ToString());
        Debug.Log("Best Time: "+ PlayerPrefs.GetFloat("FortyLinesBestTime"));
        int minutes = (int)(PlayerPrefs.GetFloat("FortyLinesBestTime") / 60);
        int seconds = (int)(PlayerPrefs.GetFloat("FortyLinesBestTime") % 60);
        int milliseconds = (int)((PlayerPrefs.GetFloat("FortyLinesBestTime") % 1) * 1000);
        secondsNumbers[0].GetComponent<SpriteRenderer>().sprite = spriteNumbers[(int)seconds % 10];
        secondsNumbers[1].GetComponent<SpriteRenderer>().sprite = spriteNumbers[(int)seconds / 10];
        minutesNumbers[0].GetComponent<SpriteRenderer>().sprite = spriteNumbers[(int)minutes % 10];
        minutesNumbers[1].GetComponent<SpriteRenderer>().sprite = spriteNumbers[(int)minutes / 10];
        millisecondsNumbers[0].GetComponent<SpriteRenderer>().sprite = spriteNumbers[(int)milliseconds % 10];
        milliseconds /= 10;
        millisecondsNumbers[1].GetComponent<SpriteRenderer>().sprite = spriteNumbers[(int)milliseconds % 10];
        millisecondsNumbers[2].GetComponent<SpriteRenderer>().sprite = spriteNumbers[(int)milliseconds / 10];
    }

	public void Update()
	{
		if (Input.GetKeyDown(KeyCode.Escape))
		{
			BackToMenu();
		}
	}

    public void AddTo(GameObject[] receiver, string tmp)
    {
        int i;
        int j;

        i = tmp.Length - 1;
        j = 0;
        while (j < tmp.Length)
        {
            if (receiver[i].GetComponent<SpriteRenderer>().sprite != spriteNumbers[(int)tmp[j] - '0'])
            {
                receiver[i].GetComponent<SpriteRenderer>().sprite = spriteNumbers[(int)tmp[j] - '0'];
            }
            --i;
            ++j;
        }
    }

    public void BackToMenu()
    {
        SceneManager.LoadScene("Memorial");
    }
}
