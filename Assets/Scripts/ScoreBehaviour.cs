using UnityEngine;
using System.Collections;

public class ScoreBehaviour : MonoBehaviour
{
    public GameObject[] numberTetrominoes;
    public GameObject[] numberLines;
    public GameObject[] scoreNumbers;
    public GameObject[] levelLines;
    public GameObject[] levelNumber;
    public Sprite[] spriteNumbers;
    public GameObject scoreNumberSample;
	public GameObject LevelUp;
	public GameObject GameFeelLines;
    public int tetrominoes = 0;
    public int lines = 0;
    public int score = 0;
    public int level = 0;
    public int linesNeeded = 5;
    public int lastLinesNeeded;
    public float speed;

    public bool isFortyLines = false;

    private float scoreYPos = 14.0f;
    private int i = 0;
	private AudioSource[] aSources;
	private GameObject canvas;

	public bool CustomStart ()
    {
        speed = 0.9f;
        lastLinesNeeded = linesNeeded;
        while (i < 8)
        {
            scoreNumbers[i] = Instantiate(scoreNumberSample, new Vector3(-2 - (i+1) * 1.2f, scoreYPos, 0), transform.rotation) as GameObject;
            ++i;
        }
        i = 0;
        while (i < 3)
        {
            levelLines[i] = Instantiate(scoreNumberSample, new Vector3(-2 - (i + 1) * 1.2f, 10, 0), transform.rotation) as GameObject;
            ++i;
        }
        levelLines[0].GetComponent<SpriteRenderer>().sprite = spriteNumbers[5];
        i = 0;
        while (i < 2)
        {
            levelNumber[i] = Instantiate(scoreNumberSample, new Vector3(-9.2f - (i + 1) * 1.2f, 10, 0), transform.rotation) as GameObject;
            ++i;
        }
        i = 0;
        while (i < 4)
        {
            numberTetrominoes[i] = Instantiate(scoreNumberSample, new Vector3(22f - (i + 1) * 1.2f, scoreYPos, 0), transform.rotation) as GameObject;
            ++i;
        }
        i = 0;
        while (i < 4)
        {
            numberLines[i] = Instantiate(scoreNumberSample, new Vector3(22f - (i + 1) * 1.2f, 10, 0), transform.rotation) as GameObject;
            ++i;
        }
		aSources = GetComponents<AudioSource>();
		aSources[0].volume = aSources[0].volume * ((float)PlayerPrefs.GetInt("EffectsVolume") / 10);
		canvas = GameObject.Find("Canvas");
		GameFeelLines = GameObject.Find("GameFeelLines");
		return true;
    }
	
    public void AddToScore(int toAdd)
    {
        string tmp;
        int i;
        int j;

        score += toAdd;
        tmp = score.ToString();
        i = tmp.Length - 1;
        j = 0;
        while (j < tmp.Length)
        {
            if (scoreNumbers[i].GetComponent<SpriteRenderer>().sprite != spriteNumbers[(int)tmp[j] - '0'])
            {
                scoreNumbers[i].GetComponent<SpriteRenderer>().sprite = spriteNumbers[(int)tmp[j] - '0'];
                if (scoreNumbers[i].transform.position.y <= scoreYPos)
                {
                    scoreNumbers[i].transform.position += new Vector3(0.0f, 0.3f, 0.0f);
                    StartCoroutine(PutPositionBack(scoreNumbers[i], 0.3f, 0.1f + ((float)i / 10)));
                }
            }
            --i;
            ++j;
        }
    }

    public void AddToTetromiones(int toAdd)
    {
        string tmp;
        int i;
        int j;

        tetrominoes += toAdd;
        tmp = tetrominoes.ToString();
        i = tmp.Length - 1;
        j = 0;
        while (j < tmp.Length)
        {
            numberTetrominoes[i].GetComponent<SpriteRenderer>().sprite = spriteNumbers[(int)tmp[j] - '0'];
            numberTetrominoes[i].transform.position += new Vector3(0.0f, 0.3f, 0.0f);
            StartCoroutine(PutPositionBack(numberTetrominoes[i], 0.3f, 0.1f + ((float)i / 10)));
            --i;
            ++j;
        }
    }

    public void AddToLines(int toAdd)
    {
        string tmp;
        int i;
        int j;

        lines += toAdd;
        tmp = lines.ToString();
        i = tmp.Length - 1;
        j = 0;
        while (j < tmp.Length)
        {
            numberLines[i].GetComponent<SpriteRenderer>().sprite = spriteNumbers[(int)tmp[j] - '0'];
            numberLines[i].transform.position += new Vector3(0.0f, 0.3f, 0.0f);
            StartCoroutine(PutPositionBack(numberLines[i], 0.3f, 0.1f + ((float)i / 10)));
            --i;
            ++j;
        }
		GameFeelLinesStart();
        if (isFortyLines == true && lines >= 40)
        {
            GameObject.Find("$GameManager").GetComponent<GameManagerBehaviour>().EndFortyLines();
            GameObject.Find("Timer").GetComponent<TimerBehavior>().isEnd = true;
        }
    }

	void GameFeelLinesStart()
	{
		GameFeelLines.transform.GetChild (0).gameObject.GetComponent<SpriteRenderer> ().color = new Color(255.0f, 255.0f, 255.0f, 1.0f);
		GameFeelLines.transform.GetChild (1).gameObject.GetComponent<SpriteRenderer> ().color = new Color(255.0f, 255.0f, 255.0f, 1.0f);
		HideGameFeelLines ();
	}

	void HideGameFeelLines()
	{
		float tmpAlpha = GameFeelLines.transform.GetChild (0).gameObject.GetComponent<SpriteRenderer> ().color.a;
		tmpAlpha -= 0.1f;
		GameFeelLines.transform.GetChild (0).gameObject.GetComponent<SpriteRenderer> ().color = new Color(255.0f, 255.0f, 255.0f, tmpAlpha);
		GameFeelLines.transform.GetChild (1).gameObject.GetComponent<SpriteRenderer> ().color = new Color(255.0f, 255.0f, 255.0f, tmpAlpha);
		if (tmpAlpha > 0)
			Invoke ("HideGameFeelLines", 0.05f);
	}

    IEnumerator PutPositionBack(GameObject number, float y, float delay)
    {
        yield return new WaitForSeconds(delay);
        number.transform.position -= new Vector3(0.0f, y, 0.0f);
    }

    public void RemoveFromNeeded(int toRemove)
    {
        string tmp;
        int i;
        int j;

        linesNeeded -= toRemove;
        if (linesNeeded <= 0)
        {
            level += 1;
			aSources[0].Play();
			var tmpLevelUp = Instantiate (LevelUp, new Vector3(0.0f, 0.0f, 0.0f), LevelUp.transform.rotation);
			tmpLevelUp.transform.SetParent(canvas.transform, false);
            if (speed > 0.1f)
                speed -= 0.1f;
            else
                speed -= 0.02f;
            lastLinesNeeded += 5;
            linesNeeded = lastLinesNeeded;
            tmp = level.ToString();
            if (level < 10)
            {
                levelNumber[1].GetComponent<SpriteRenderer>().sprite = spriteNumbers[0];
            }
            i = tmp.Length - 1;
            j = 0;
            while (j < tmp.Length)
            {
                levelNumber[i].GetComponent<SpriteRenderer>().sprite = spriteNumbers[(int)tmp[j] - '0'];
                levelNumber[i].transform.position += new Vector3(0.0f, 0.3f, 0.0f);
                StartCoroutine(PutPositionBack(levelNumber[i], 0.3f, 0.1f + ((float)i / 10)));
                --i;
                ++j;
            }
        }
        tmp = linesNeeded.ToString();
        if (linesNeeded < 10)
        {
            levelLines[1].GetComponent<SpriteRenderer>().sprite = spriteNumbers[0];
        }
        i = tmp.Length - 1;
        j = 0;
        while (j < tmp.Length)
        {
            levelLines[i].GetComponent<SpriteRenderer>().sprite = spriteNumbers[(int)tmp[j] - '0'];
            levelLines[i].transform.position += new Vector3(0.0f, 0.3f, 0.0f);
            StartCoroutine(PutPositionBack(levelLines[i], 0.3f, 0.1f + ((float)i / 10)));
            --i;
            ++j;
        }
    }

	public void UpdateLevel()
	{
		string tmp;
		int i;
		int j;

		tmp = level.ToString();
		if (level < 10)
		{
			levelNumber[1].GetComponent<SpriteRenderer>().sprite = spriteNumbers[0];
		}
		i = tmp.Length - 1;
		j = 0;
		while (j < tmp.Length)
		{
			levelNumber[i].GetComponent<SpriteRenderer>().sprite = spriteNumbers[(int)tmp[j] - '0'];
			levelNumber[i].transform.position += new Vector3(0.0f, 0.3f, 0.0f);
			StartCoroutine(PutPositionBack(levelNumber[i], 0.3f, 0.1f + ((float)i / 10)));
			--i;
			++j;
		}
	}
}

// Level Behaviour
// - Start at 5
// - Add 5 each level
// - 1 -> 1
// - 2 -> 3
// - 3 -> 5
// - 4 -> 8