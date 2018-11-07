using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimerUltraBehavior : MonoBehaviour
{
    public GameObject[] secondNumbers;
    public GameObject[] minuteNumbers;
    public Sprite[] spriteNumbers;
    public GameObject scoreNumberSample;
    public bool isEnd = false;
    public bool isPause = false;
    public float t = 0.0F;

    private float pauseTime = -1.0F;
    private float timeToRemove = 0.0F;
    private float cumulatedPauseTime;

    // Use this for initialization
    void Start()
    {
        int i = 0;

        while (i < 2)
        {
            secondNumbers[i] = Instantiate(scoreNumberSample, new Vector3(22F - (i + 1) * 1.2f, 7.5F, 0), transform.rotation) as GameObject;
            ++i;
        }
        i = 0;
        while (i < 2)
        {
            minuteNumbers[i] = Instantiate(scoreNumberSample, new Vector3(19F - (i + 1) * 1.2f, 7.5F, 0), transform.rotation) as GameObject;
            ++i;
        }
		if (PlayerPrefs.GetInt ("SaveGame") == 1)
			t = PlayerPrefs.GetFloat("SaveGameTimerUltra");
		else
			t = 180.0F;
        GameObject.Find("Score").GetComponent<ScoreBehaviour>().isFortyLines = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (isPause == true)
        {
            if (pauseTime == -1.0F)
            {
                pauseTime = Time.time;
            }
            cumulatedPauseTime = Time.time - pauseTime;
            return;
        }
        else
        {
            if (pauseTime != -1.0F)
            {
                timeToRemove += cumulatedPauseTime;
                cumulatedPauseTime = 0;
                pauseTime = -1;
            }
        }
        if (isEnd == true)
            return;
        t = t - Time.deltaTime;
        int minutes = (int)(t / 60);
        int seconds = (int)(t % 60);
        //Debug.Log("Timer: "+minutes+":"+seconds);
        secondNumbers[0].GetComponent<SpriteRenderer>().sprite = spriteNumbers[(int)seconds % 10];
        secondNumbers[1].GetComponent<SpriteRenderer>().sprite = spriteNumbers[(int)seconds / 10];
        minuteNumbers[0].GetComponent<SpriteRenderer>().sprite = spriteNumbers[(int)minutes % 10];
        minuteNumbers[1].GetComponent<SpriteRenderer>().sprite = spriteNumbers[(int)minutes / 10];

        if (t <= 0)
        {
            GameObject.Find("$GameManager").GetComponent<GameManagerBehaviour>().EndUltra();
            isEnd = true;
        }
    }
}
