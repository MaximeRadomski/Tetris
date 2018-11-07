using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MemorialUltraBehavior : MonoBehaviour
{
    public GameObject[] scoreNumbers;
    public GameObject[] levelNumber;
    public GameObject[] numberLines;
    public Sprite[] spriteNumbers;
    public GameObject scoreNumberSample;

    // Use this for initialization
    void Start()
    {
        int i = 0;
        while (i < 8)
        {
            scoreNumbers[i] = Instantiate(scoreNumberSample, new Vector3(9.85f - (i + 1) * 1.2f, 16.5f, 0), transform.rotation) as GameObject;
            ++i;
        }
        i = 0;
        while (i < 2)
        {
            levelNumber[i] = Instantiate(scoreNumberSample, new Vector3(6.2f - (i + 1) * 1.2f, 11.5f, 0), transform.rotation) as GameObject;
            ++i;
        }
        i = 0;
        while (i < 3)
        {
            numberLines[i] = Instantiate(scoreNumberSample, new Vector3(6.85f - (i + 1) * 1.2f, 6.5f, 0), transform.rotation) as GameObject;
            ++i;
        }
        if (PlayerPrefs.GetInt("UltraRecord") == 0)
            return;
        AddTo(scoreNumbers, PlayerPrefs.GetInt("UltraScore").ToString());
        AddTo(levelNumber, PlayerPrefs.GetInt("UltraLevel").ToString());
        AddTo(numberLines, PlayerPrefs.GetInt("UltraLines").ToString());
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
