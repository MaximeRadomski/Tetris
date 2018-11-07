using UnityEngine;
using System.Collections;

public class Group : MonoBehaviour
{
    public GameObject score;
    public GameObject particles = null;
    public string tetromino = "";
    public float width = 1.0f;
    public float height = 1.0f;
    public float leftRightScale = 0.0f;
    public bool isDown = false;
    public GameObject shadow;
	public bool canMoveDown = true;
	public bool LastMovementIsRotate = false;

	private GhostLineBehaviour ghostLine;
    // Time since last gravity tick
    float lastFall = 0.1f;
    //bool canMoveSide = true;
    //bool firstTimeSide = true;
    private float tmp;
	#if UNITY_IOS || UNITY_ANDROID || UNITY_WP8 || UNITY_IPHONE
	private Vector2 touchOrigin = -Vector2.one;
	#endif
    int rotationStep = 0;
    float mainSpeed;
    float lockDelay;
    AudioSource[] audios;

    private GameObject gameManager;
    private float timeStamp;
    public float timeBeforeHoldKey = 0.15f;
	private bool keepBeforeHardDrop = false;

    // Use this for initialization
    void Start ()
    {
        score = GameObject.Find("Score");
		ghostLine = GameObject.Find("GhostLine").GetComponent<GhostLineBehaviour>();
        mainSpeed = score.GetComponent<ScoreBehaviour>().speed;
        lockDelay = mainSpeed;
        audios = GetComponents<AudioSource>();
        audios[0].volume = audios[0].volume * ((float)PlayerPrefs.GetInt("EffectsVolume") / 10);
        audios[1].volume = audios[1].volume * ((float)PlayerPrefs.GetInt("EffectsVolume") / 10);
        audios[2].volume = audios[2].volume * ((float)PlayerPrefs.GetInt("EffectsVolume") / 10);
        if (tetromino == "I" || tetromino == "O")
        {
            ++rotationStep;
            leftRightScale = 0.5f;
        }
        transform.gameObject.tag = "AliveGroup";
        // Default position not valid? Then it's game over
        /*if (!IsValidGridPos())
        {
            Debug.Log("GAME OVER");
            Destroy(gameObject);
        }*/
        Instantiate(shadow, transform.position + new Vector3(0.0f, 0.0f, 0.0f), transform.rotation);
        gameManager = GameObject.Find("$GameManager");
    }
	
	// Update is called once per frame
	void Update ()
    {
        if (transform.childCount <= 0) // If no more squares -> Destroy Object
            Destroy(gameObject);
        if (isDown == true || gameManager.GetComponent<GameManagerBehaviour>().pauseEnable == true ||
                              gameManager.GetComponent<GameManagerBehaviour>().gameOver == true) // If Down or Pause -> no Update
            return;
        if (Input.GetButtonDown("MoveLeft")) // KEY LEFT
        {
			MoveLeft();
        }
        if (Input.GetButton("MoveLeft") && Time.time >= timeStamp) // KEY LEFT HOLDED
        {
			MoveLeftHolded();
        }
        if (Input.GetButtonDown("MoveRight")) // KEY RIGHT
        {
			MoveRight();
        }
        if (Input.GetButton("MoveRight") && Time.time >= timeStamp) // KEY RIGHT HOLDED
        {
			MoveRightHolded();
        }
        // Rotate Right
		if (Input.GetButtonDown("RotateRight")&& tetromino != "O")
        {
			RotateRight();
        }
        // Rotate Left
        if (Input.GetButtonDown("RotateLeft") && tetromino != "O")
        {
			RotateLeft();
        }
        // Move Downwards and Fall
        if ((Input.GetButton("SoftDrop") && canMoveDown == true) || Time.time - lastFall >= lockDelay)
        {
			SoftDrop();
        }
        //Drop Down
        if (Input.GetButtonDown("HardDrop"))
        {
			HardDrop();
        }

		//Touch Screen Handling
		#if UNITY_IOS || UNITY_ANDROID || UNITY_WP8 || UNITY_IPHONE

		int horizontal = 0;
		int vertical = 0;

		if (Input.touchCount > 0)
		{
			Touch myTouch = Input.touches[0];
			if (myTouch.phase == TouchPhase.Began)
			{
				touchOrigin = myTouch.position;
			}
			else if (myTouch.phase == TouchPhase.Ended && touchOrigin.x >= 0)
			{
				Vector2 touchEnd = myTouch.position;
				float y = touchEnd.y - touchOrigin.y;
				touchOrigin.x = -1;

				if (Mathf.Abs(y) > 150)
					vertical = y > 0 ? 1 : -1;

				if (vertical > 0)
					HardDrop();
			}
			else if (myTouch.phase == TouchPhase.Moved)
			{
				if (touchOrigin.x == -1)
					touchOrigin = myTouch.position;
				Vector2 touchMoved = myTouch.position;
				float x = touchMoved.x - touchOrigin.x;
				float y = touchMoved.y - touchOrigin.y;

				if (Mathf.Abs(x) > Mathf.Abs(y) && Mathf.Abs(x) > 50)
					horizontal = x > 0 ? 1 : -1;
				else if (Mathf.Abs(x) < Mathf.Abs(y) && Mathf.Abs(y) > 25)
					vertical = y > 0 ? 1 : -1;

				if (horizontal > 0)
					MoveRight();
				else if (horizontal < 0)
					MoveLeft();
				else if (vertical < 0 && keepBeforeHardDrop == false)
					SoftDrop();
				else
					return;
				touchOrigin.x = -1;
			}
		}

		#endif
    }

    void OnDropDown()
    {
        score.GetComponent<ScoreBehaviour>().AddToTetromiones(1);
		// Clear filled horizontal lines
        GridBehavior.DeleteFullRows();
        // Spawn next Group
        foreach (Transform child in transform)
        {
            child.transform.gameObject.layer = LayerMask.NameToLayer("LockedBlock");
        }
        Destroy(GameObject.FindGameObjectWithTag("CurrentShadow"));
        FindObjectOfType<Spawner>().SpawnNext();
        FindObjectOfType<Spawner>().lockHold = false;
    }

	void CheckTSpin()
	{
		int moveRequirement = 0;
		if (tetromino == "T")
		{
			if (LastMovementIsRotate == true)
			{
				//Check LEFT
				transform.position += new Vector3(-1, 0, 0);
				if (IsValidGridPos ())
				{
					transform.position += new Vector3(1, 0, 0);
				}
				else
				{
					transform.position += new Vector3(1, 0, 0);
					++moveRequirement;
				}

				//Check RIGHT
				transform.position += new Vector3(1, 0, 0);
				if (IsValidGridPos ())
				{
					transform.position += new Vector3(-1, 0, 0);
				}
				else
				{
					transform.position += new Vector3(-1, 0, 0);
					++moveRequirement;
				}


				//Check UP
				transform.position += new Vector3(0, 1, 0);
				if (IsValidGridPos ())
				{
					transform.position += new Vector3(0, -1, 0);
				}
				else
				{
					transform.position += new Vector3(0, -1, 0);
					++moveRequirement;
				}

				if (moveRequirement == 3)
					GridBehavior.TSpinOccured();
			}
		}
	}

    void CheckLastStep()
    {
        //Check if next step is last
        transform.position += new Vector3(0, -1, 0);
		if (IsValidGridPos ())
			lockDelay = mainSpeed;
		else 
            lockDelay = 1.75f;
        transform.position += new Vector3(0, 1, 0);
    }

    void RotationAccepted(int i)
    {
        tmp = height;
        height = width;
        width = tmp;
        rotationStep += i;
        if (rotationStep == -1)
            rotationStep = 3;
        else if (rotationStep == 4)
            rotationStep = 0;
        if (rotationStep == 0)
            leftRightScale = 0.0f;
        else if (rotationStep == 1)
            leftRightScale = 0.5f;
        else if (rotationStep == 2)
            leftRightScale = 0.0f;
        else if (rotationStep == 3)
            leftRightScale = -0.5f;
        updateGrid();
		LastMovementIsRotate = true;
    }

    void SpawnNextAfterDelay()
    {
        foreach (Transform child in transform)
        {
            child.transform.gameObject.layer = LayerMask.NameToLayer("LockedBlock");
        }
        Destroy(GameObject.FindGameObjectWithTag("CurrentShadow"));
        FindObjectOfType<Spawner>().SpawnNext();
        FindObjectOfType<Spawner>().lockHold = false;
    }

    void DelayMoveDown()
    {
        canMoveDown = true;
    }

	void DelayMoveDownBeforeHardDrop()
	{
		canMoveDown = true;
		if (keepBeforeHardDrop == true)
			SoftDrop ();
	}

    bool IsValidGridPos()
    {
        foreach(Transform child in transform)
        {
			Vector2 v = GridBehavior.RoundVec2(child.position);
            //not inside border ?
			if (!GridBehavior.InsideBorder(v))
                return false;
            //Block in grid cell (and not part of the same group) ?
			if (GridBehavior.grid[(int)v.x, (int)v.y] != null &&
				GridBehavior.grid[(int)v.x, (int)v.y].parent != transform)
                return false;
        }
        return true;
    }

    void updateGrid()
    {
        // Remove old children from grid
		for (int y = 0; y < GridBehavior.h; ++y)
        {
			for (int x = 0; x < GridBehavior.w; ++x)
            {
				if (GridBehavior.grid[x, y] != null)
                {
					if (GridBehavior.grid[x, y].parent == transform)
						GridBehavior.grid[x, y] = null;
                }
            }
        }

        // Add new children to grid
        foreach (Transform child in transform)
        {
			Vector2 v = GridBehavior.RoundVec2(child.position);
			GridBehavior.grid[(int)v.x, (int)v.y] = child;
        }
    }

	public void RotateRight()
	{
		audios[1].Play();
		transform.Rotate(0, 0, -90);
		RotationSequence(1);
		CheckLastStep();
	}

	public void RotateLeft()
	{
		audios[1].Play();
		transform.Rotate(0, 0, 90);
		RotationSequence(-1);
		CheckLastStep();
	}

	private void RotationSequence(int rotationOrientation)
	{
		if (IsValidGridPos())
			RotationAccepted(rotationOrientation);
		else
		{
			transform.position += new Vector3(-1, 0, 0); // Test LEFT
			if (IsValidGridPos())
				RotationAccepted(rotationOrientation);
			else
			{
				transform.position += new Vector3(2, 0, 0);  // Test RIGHT
				if (IsValidGridPos())
					RotationAccepted(rotationOrientation);
				else
				{
					transform.position += new Vector3(-1, 1, 0); // Test UP
					if (IsValidGridPos())
						RotationAccepted(rotationOrientation);
					else
					{
						transform.position += new Vector3(0, -2, 0); // Test DOWN
						if (IsValidGridPos())
							RotationAccepted(rotationOrientation);
						else
						{
							transform.position += new Vector3(-1, 0, 0); // Test DOWN LEFT
							if (IsValidGridPos())
								RotationAccepted(rotationOrientation);
							else
							{
								transform.position += new Vector3(2, 0, 0); // Test DOWN RIGHT
								if (IsValidGridPos())
									RotationAccepted(rotationOrientation);
								else
								{
									transform.position += new Vector3(-2, 2, 0); // Test UP LEFT
									if (IsValidGridPos ())
										RotationAccepted (rotationOrientation);
									else 
									{
										transform.position += new Vector3(2, 0, 0); // Test UP RIGHT
										if (IsValidGridPos ())
											RotationAccepted (rotationOrientation);
										else 
										{

											if (tetromino == "I")
											{
												transform.position += new Vector3 (-1, 1, 0); // UP UP
												if (IsValidGridPos ())
													RotationAccepted (rotationOrientation);
												else 
												{
													transform.position += new Vector3 (-2, -2, 0); // LEFT LEFT
													if (IsValidGridPos ())
														RotationAccepted (rotationOrientation);
													else 
													{
														transform.position += new Vector3 (4, 0, 0); // RIGHT RIGHT
														if (IsValidGridPos ())
															RotationAccepted (rotationOrientation);
														else 
														{
															transform.position += new Vector3 (-2, 0, 0); // RESET
															transform.Rotate (0, 0, 90 * rotationOrientation);
														}
													}
												}
											}
											else 
											{
												transform.position += new Vector3 (-1, -1, 0); // RESET
												transform.Rotate (0, 0, 90 * rotationOrientation);
											}

										}
									}
								}
							}
						}
					}

				}
			}
		}
	}

	public void MoveLeft()
	{
		timeStamp = Time.time + timeBeforeHoldKey;
		// Modify position
		transform.position += new Vector3(-1, 0, 0);

		// See if valid
		if (IsValidGridPos ())
		{
			audios[0].Play();
			// It's valid. Update grid.
			LastMovementIsRotate = false;
			updateGrid();
		}
			// Its valid. Update grid.
		else
			// Its not valid. revert.
			transform.position += new Vector3(1, 0, 0);
		CheckLastStep();
	}

	public void MoveRight()
	{
		timeStamp = Time.time + timeBeforeHoldKey;
		// Modify position
		transform.position += new Vector3(1, 0, 0);

		// See if valid
		if (IsValidGridPos())
		{
			audios[0].Play();
			// It's valid. Update grid.
			LastMovementIsRotate = false;
			updateGrid();
		}
		else
			// It's not valid. revert.
			transform.position += new Vector3(-1, 0, 0);
		CheckLastStep();
	}

	public void SoftDrop()
	{
		// Modify position
		transform.position += new Vector3(0, -1, 0);

		// See if valid
		if (IsValidGridPos())
		{
			// It's valid. Update grid.
			score.GetComponent<ScoreBehaviour>().AddToScore(1);
			updateGrid();
			canMoveDown = false;
			Invoke("DelayMoveDown", mainSpeed * 0.078f);
			CheckLastStep();
			keepBeforeHardDrop = false;
		}
		else if (keepBeforeHardDrop == false)
		{
			keepBeforeHardDrop = true;
			canMoveDown = false;
			Invoke("DelayMoveDownBeforeHardDrop", 0.75f);
			// It's not valid. revert.
			transform.position += new Vector3(0, 1, 0);
		}
		else
		{
			keepBeforeHardDrop = false;
			lockDelay = mainSpeed;
			transform.gameObject.tag = "DeadGroup";
			// It's not valid. revert.
			transform.position += new Vector3(0, 1, 0);
			CheckTSpin();
			if (GridBehavior.CheckIfFullRows() > 0)
				Invoke("OnDropDown", 0.0f);
			else
				Invoke("OnDropDown", 0.0f);

			// Disable script
			isDown = true;
			if (particles != null)
			{
				Instantiate(particles, transform.position, particles.transform.rotation);
			}
			audios[2].Play();
		}
		lastFall = Time.time;
	}

	public void HardDrop()
	{
		bool end = false;
		keepBeforeHardDrop = false;
		ghostLine.HardDrop ();
		while (end == false)
		{
			// Modify position
			transform.position += new Vector3(0, -1, 0);

			// See if valid
			if (IsValidGridPos())
			{
				// It's valid. Update grid.
				score.GetComponent<ScoreBehaviour>().AddToScore(2);
				updateGrid();
			}
			else
			{
				lockDelay = mainSpeed;
				// It's not valid. revert.
				transform.position += new Vector3(0, 1, 0);
				CheckTSpin();
				if (GridBehavior.CheckIfFullRows() > 0)
					Invoke("OnDropDown", 0.0f);
				else
					Invoke("OnDropDown", 0.0f);

				// Disable script
				isDown = true;
				transform.gameObject.tag = "DeadGroup";
				end = true;
				if (particles != null)
				{
					Instantiate(particles, transform.position, particles.transform.rotation);
				}
				audios[2].Play();
				score.GetComponent<ScoreBehaviour>().AddToScore(2);
			}
		}
	}

	public void MoveLeftHolded()
	{
		// Modify position
		transform.position += new Vector3(-1, 0, 0);

		// See if valid
		if (IsValidGridPos())
			// Its valid. Update grid.
			updateGrid();
		else
			// Its not valid. revert.
			transform.position += new Vector3(1, 0, 0);
		CheckLastStep();
	}

	public void MoveRightHolded()
	{
		// Modify position
		transform.position += new Vector3(1, 0, 0);

		// See if valid
		if (IsValidGridPos())
			// It's valid. Update grid.
			updateGrid();
		else
			// It's not valid. revert.
			transform.position += new Vector3(-1, 0, 0);
		CheckLastStep();
	}
}
