using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CurtainsBehavior : MonoBehaviour {

	// Use this for initialization
	void Start ()
	{
		UpdateSprites ();
	}
	
	// Update is called once per frame
	public void UpdateSprites ()
	{
		gameObject.GetComponent<SpriteRenderer>().sprite = Resources.Load<Sprite>("Sprites/Curtains" + PlayerPrefs.GetInt ("CurtainsStyle").ToString("D2"));
		var tmpSmallCurtains = GameObject.Find ("SmallCurtains Left");
		UpdateSmallCurtains (tmpSmallCurtains);
		tmpSmallCurtains = GameObject.Find ("SmallCurtains Right");
		UpdateSmallCurtains (tmpSmallCurtains);
	}

	void UpdateSmallCurtains (GameObject tmpSmallCurtains)
	{
		for (int i = 0; i < tmpSmallCurtains.transform.childCount; ++i)
		{
			tmpSmallCurtains.transform.GetChild(i).GetComponent<SpriteRenderer>().sprite = Resources.Load<Sprite>("Sprites/CurtainSmall" + PlayerPrefs.GetInt ("CurtainsStyle").ToString("D2"));
		}
	}
}
