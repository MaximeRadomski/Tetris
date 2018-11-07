using UnityEngine;
using System.Collections;

public class ShadowBehaviour : MonoBehaviour
{
    private GameObject currentGroup;
    private bool isPlaced;
    private int y;

    void Start()
    {
        currentGroup = GameObject.FindGameObjectWithTag("AliveGroup");
        FirstGoDown();
    }

    void FirstGoDown()
    {
        isPlaced = false;
        y = 0;
        while (isPlaced == false)
        {
            foreach (Transform child in transform)
            {
                if (Physics2D.OverlapCircle(child.transform.position + new Vector3(0.0f, 0.0f - y, 0.0f),
                                            0.5f, 1 << LayerMask.NameToLayer("LockedBlock")))
                {
                    isPlaced = true;
                    //Debug.Log(child.transform.position.x + " " + child.transform.position.y);
                }
            }
            if (isPlaced == true)
            {
                transform.position += new Vector3(0.0f, 1.0f - y, 0.0f);
                if (y == 0)
                {
                    Debug.Log("GameOver");
                    currentGroup.GetComponent<Group>().isDown = true;
                    int GameType = PlayerPrefs.GetInt("GameType");
                    if (GameType == 0)
                        GameObject.Find("$GameManager").GetComponent<GameManagerBehaviour>().GameOver();
                    else if (GameType == 1)
                        GameObject.Find("$GameManager").GetComponent<GameManagerBehaviour>().EndFortyLines();
					else if (GameType == 2)
                        GameObject.Find("$GameManager").GetComponent<GameManagerBehaviour>().EndUltra();
					else
						GameObject.Find("$GameManager").GetComponent<GameManagerBehaviour>().EndCleaning();
                }
            }
            ++y;
        }
    }

    void GoDown()
    {
        isPlaced = false;
        y = 0;
        while (isPlaced == false)
        {
            foreach (Transform child in transform)
            {
                if (Physics2D.OverlapCircle(child.transform.position + new Vector3(0.0f, 0.0f - y, 0.0f),
                                            0.5f, 1 << LayerMask.NameToLayer("LockedBlock")))
                {
                    isPlaced = true;
                    //Debug.Log(child.transform.position.x + " " + child.transform.position.y);
                }
            }
            if (isPlaced == true)
                transform.position += new Vector3(0.0f, 1.0f - y, 0.0f);
            ++y;
        }
    }

	// Update is called once per frame
   	void Update ()
    {
        if (Input.GetButton("MoveLeft"))
        {
            transform.position = currentGroup.transform.position;
            transform.rotation = currentGroup.transform.rotation;
            GoDown();
        }
        if (Input.GetButton("MoveRight"))
        {
            transform.position = currentGroup.transform.position;
            transform.rotation = currentGroup.transform.rotation;
            GoDown();
        }
        if (Input.GetButtonDown("RotateRight"))
        {
            transform.position = currentGroup.transform.position;
            transform.rotation = currentGroup.transform.rotation;
            GoDown();
        }
        if (Input.GetButtonDown("RotateLeft"))
        {
			transform.position = currentGroup.transform.position;
            transform.rotation = currentGroup.transform.rotation;
            GoDown();
        }

		#if UNITY_IOS || UNITY_ANDROID || UNITY_WP8 || UNITY_IPHONE

		if (Input.touchCount > 0)
		{
			//Store the first touch detected.
			Touch myTouch = Input.touches[0];

			//Check if the phase of that touch equals Began
			if (myTouch.phase == TouchPhase.Began
				|| myTouch.phase == TouchPhase.Moved
				||myTouch.phase == TouchPhase.Ended)
			{
				FollowTetromino();
			}
		}

		#endif
    }

	public void FollowTetromino()
	{
		transform.position = currentGroup.transform.position;
		transform.rotation = currentGroup.transform.rotation;
		GoDown();
	}
}
