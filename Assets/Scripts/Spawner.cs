using UnityEngine;
using System.Collections;

public class Spawner : MonoBehaviour
{
    //Groups
    public GameObject holded = null;
    public GameObject tmpHold = null;
    public GameObject currentGroup;
    public GameObject[] groups;
    public GameObject[] nexts;
    public char[] order;
    public bool lockHold = false;
	public GameObject GarbageGroup;

	public bool MobileUpdateHold = false;

    private bool firstTime = true;
    private int i = 0;
    private int betweenIGroup = 5;
	private string numberBag = "";

    //private bool firstHold = false;

    private char lastGroup = '\0';
    private int maxLastGroup = 0;

    private AudioSource[] audios;

    // Use this for initialization
	public bool CustomStart ()
    {
		for (int j = 0; j < groups.Length; j++)
		{
			groups[j] = Resources.Load<GameObject>("Prefabs/Blocks/Group" + order[j].ToString() + PlayerPrefs.GetInt ("TetrominoesStyle").ToString("D2"));
		}
		for (int j = 0; j < nexts.Length; j++)
		{
			nexts[j] = Resources.Load<GameObject>("Prefabs/Blocks/Next" + order[j].ToString() + PlayerPrefs.GetInt ("TetrominoesStyle").ToString("D2"));
		}
		GarbageGroup = Resources.Load<GameObject>("Prefabs/Blocks/GarbageGroup" + PlayerPrefs.GetInt ("TetrominoesStyle").ToString("D2"));
		if (PlayerPrefs.GetInt ("GameType") == 3)
			StartCoroutine (GridBehavior.SetCleaningArea (GarbageGroup));
        audios = GetComponents<AudioSource>();
        audios[0].volume = audios[0].volume * ((float)PlayerPrefs.GetInt("EffectsVolume") / 10);
		if (PlayerPrefs.GetInt ("SaveGame") == 1)
			LoadSaveGameGroups ();
		else
		{
			i = NewRandomNumber();
			while (order [i] == 'S' || order [i] == 'Z')
				i = NewRandomNumber();
			GameObject Next01 = Instantiate(nexts[i], transform.Find("Next01").position, Quaternion.identity) as GameObject;
			Next01.transform.parent = transform.Find("Next01");
			Next01.transform.localScale = new Vector3(1, 1, 1);
			i = NewRandomNumber();
			GameObject Next02 = Instantiate(nexts[i], transform.Find("Next02").position, Quaternion.identity) as GameObject;
			Next02.transform.parent = transform.Find("Next02");
			i = NewRandomNumber();
			GameObject Next03 = Instantiate(nexts[i], transform.Find("Next03").position, Quaternion.identity) as GameObject;
			Next03.transform.parent = transform.Find("Next03");
			i = NewRandomNumber();
			GameObject Next04 = Instantiate(nexts[i], transform.Find("Next04").position, Quaternion.identity) as GameObject;
			Next04.transform.parent = transform.Find("Next04");
			i = NewRandomNumber();
			GameObject Next05 = Instantiate(nexts[i], transform.Find("Next05").position, Quaternion.identity) as GameObject;
			Next05.transform.parent = transform.Find("Next05");
			SpawnNext();
		}
		return true;
	}

	private void LoadSaveGameGroups()
	{
		string tmp = PlayerPrefs.GetString("SaveGameCurrentGroup");
		for (int counter = 0; counter < groups.Length; ++counter)
		{
			if (groups [counter].GetComponent<Group> ().tetromino == tmp)
				i = counter;
		}
		currentGroup = Instantiate(groups[i], transform.position, Quaternion.identity) as GameObject;
		firstTime = false;
		GameObject Next01 = Instantiate(nexts[PlayerPrefs.GetInt("SaveGameNext01")], transform.Find("Next01").position, Quaternion.identity) as GameObject;
		Next01.transform.parent = transform.Find("Next01");
		Next01.transform.localScale = new Vector3(1, 1, 1);
		GameObject Next02 = Instantiate(nexts[PlayerPrefs.GetInt("SaveGameNext02")], transform.Find("Next02").position, Quaternion.identity) as GameObject;
		Next02.transform.parent = transform.Find("Next02");
		GameObject Next03 = Instantiate(nexts[PlayerPrefs.GetInt("SaveGameNext03")], transform.Find("Next03").position, Quaternion.identity) as GameObject;
		Next03.transform.parent = transform.Find("Next03");
		GameObject Next04 = Instantiate(nexts[PlayerPrefs.GetInt("SaveGameNext04")], transform.Find("Next04").position, Quaternion.identity) as GameObject;
		Next04.transform.parent = transform.Find("Next04");
		GameObject Next05 = Instantiate(nexts[PlayerPrefs.GetInt("SaveGameNext05")], transform.Find("Next05").position, Quaternion.identity) as GameObject;
		Next05.transform.parent = transform.Find("Next05");
		if (PlayerPrefs.GetInt ("SaveGameHolded") != -1)
		{
			holded = Instantiate(nexts[PlayerPrefs.GetInt ("SaveGameHolded")], new Vector3(-4f, 17f, 0), Quaternion.identity) as GameObject;
			holded.transform.localScale = new Vector3(1, 1, 1);
		}
	}

	// Update is called once per frame
	void Update ()
    {
		if ((Input.GetButtonDown("Hold") || MobileUpdateHold) && lockHold == false) // KEY HOLD
        {
			MobileUpdateHold = false;
			Hold();
        }
    }

	public void Hold()
	{
		char tmp;
		int tmp2;

		audios[0].Play();
		lockHold = true;
		tmp = currentGroup.GetComponent<Group>().tetromino[0];
		Destroy(GameObject.FindGameObjectWithTag("CurrentShadow"));
		currentGroup.tag = "DeadGroup";
		Destroy(currentGroup);
		tmp2 = 0;
		while (tmp2 < order.Length)
		{
			if (order[tmp2] == tmp)
				break;
			++tmp2;
		}
		if (tmp2 < order.Length)
		{
			tmpHold = holded;
			holded = Instantiate(nexts[tmp2], new Vector3(-4f, 17f, 0), Quaternion.identity) as GameObject;
			holded.transform.localScale = new Vector3(1, 1, 1);
			if (tmpHold == null)
			{
				//firstHold = true;
				SpawnNext();
			}
			else
			{
				i = tmpHold.GetComponent<NextBehaviour>().type;
				currentGroup = Instantiate(groups[i], transform.position + new Vector3(0.0f, 1.0f, 0.0f), Quaternion.identity) as GameObject;
				Destroy(tmpHold);
			}
		}
	}

    public void SpawnNext()
    {
        i = transform.Find("Next01").GetChild(0).GetComponent<NextBehaviour>().type;
        if (firstTime)
        {
            currentGroup = Instantiate(groups[i], transform.position, Quaternion.identity) as GameObject;
            firstTime = false;
        }
        else
        	currentGroup = Instantiate(groups[i], transform.position + new Vector3(0.0f, 1.0f, 0.0f), Quaternion.identity) as GameObject;
        Destroy(transform.Find("Next01").GetChild(0).gameObject);
        transform.Find("Next02").GetChild(0).transform.localScale = new Vector3(1, 1, 1);
        transform.Find("Next02").GetChild(0).transform.position = transform.Find("Next01").transform.position;
        transform.Find("Next03").GetChild(0).transform.position = transform.Find("Next02").transform.position;
        transform.Find("Next04").GetChild(0).transform.position = transform.Find("Next03").transform.position;
        transform.Find("Next05").GetChild(0).transform.position = transform.Find("Next04").transform.position;
        transform.Find("Next02").GetChild(0).transform.parent = transform.Find("Next01");
        transform.Find("Next03").GetChild(0).transform.parent = transform.Find("Next02");
        transform.Find("Next04").GetChild(0).transform.parent = transform.Find("Next03");
        transform.Find("Next05").GetChild(0).transform.parent = transform.Find("Next04");
		i = NewRandomNumber();
        ++betweenIGroup;
        if (betweenIGroup >= 12) // If not enough 'I'
        {
            i = 0;
            while (order[i] != 'I')
                ++i;
        }
        if (order[i] == 'I')
            betweenIGroup = 0;
        if (lastGroup == order[i]) // Check if same as before
            ++maxLastGroup;
        else
        {
            lastGroup = order[i];
            maxLastGroup = 0;
        }
        if (maxLastGroup >= 3) // If more than 2 already
        {
			while (lastGroup == order[i])
				i = NewRandomNumber();
            maxLastGroup = 0;
        }
        //TEST//
        //i = 0;
        //TEST//
        GameObject Next05 = Instantiate(nexts[i], transform.Find("Next05").position, Quaternion.identity) as GameObject;
        Next05.transform.parent = transform.Find("Next05");
    }

	private int NewRandomNumber()
	{
		int randomNumber;

		if (numberBag == "")
			numberBag = "0123456";
		int randomCharacterId = Random.Range(0, numberBag.Length);
		int.TryParse(numberBag[randomCharacterId].ToString(), out randomNumber);
		numberBag = numberBag.Remove(randomCharacterId, 1);
		return randomNumber;
	}
}
