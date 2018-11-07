using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MobileKeyBehavior : MonoBehaviour
{
	public string Action;

	private GameObject gameManager;
	private GameObject tetromino;
	private GameObject spawner;
	private GameObject shadow;
	private float timeStamp;
	private float timeBeforeHoldKey = 0.15f;
	private bool ImFocused = false;
	private Vector3 mousePosition;

	void Update ()
	{
		if ((Action == "MoveLeft" || Action == "MoveRight")
			&& ImFocused
			&& Input.GetMouseButton(0)
			&& Time.time >= timeStamp)
		{
			if (Action == "MoveLeft")
			{
				tetromino.GetComponent<Group>().MoveLeftHolded();
			}
			else
			{
				tetromino.GetComponent<Group>().MoveRightHolded();
			}
			shadow.GetComponent<ShadowBehaviour>().FollowTetromino ();
		}
		if ((Action == "SoftDrop")
		    && ImFocused
		    && Input.GetMouseButton (0)) 
		{
			tetromino = GameObject.FindWithTag("AliveGroup");
			if (tetromino.GetComponent<Group>().canMoveDown == true)
				tetromino.GetComponent<Group>().SoftDrop();
		}
	}


	void Start()
	{
		gameManager = GameObject.Find("$GameManager");
		spawner = GameObject.Find("Spawner");
	}

	void OnMouseDown()
	{
		if ((gameManager.GetComponent<GameManagerBehaviour>().pauseEnable == true ||
			gameManager.GetComponent<GameManagerBehaviour>().gameOver == true) && Action != "Pause") // If Pause or GameOver -> no Update
			return;
		tetromino = GameObject.FindWithTag("AliveGroup");
		if (tetromino == null)
			return;
		shadow = GameObject.FindWithTag("CurrentShadow");
		if (shadow == null)
			return;
		switch (Action)
		{
			case "ClockWise":
				if (tetromino.GetComponent<Group>().tetromino != "O")
					tetromino.GetComponent<Group>().RotateRight();
				shadow.GetComponent<ShadowBehaviour> ().FollowTetromino ();
				break;
			case "AntiClockWise":
				if (tetromino.GetComponent<Group>().tetromino != "O")
					tetromino.GetComponent<Group>().RotateLeft();
				shadow.GetComponent<ShadowBehaviour> ().FollowTetromino ();
				break;
			case "MoveLeft":
				tetromino.GetComponent<Group>().MoveLeft();
				timeStamp = Time.time + timeBeforeHoldKey;
				shadow.GetComponent<ShadowBehaviour> ().FollowTetromino ();
				break;
			case "MoveRight":
				tetromino.GetComponent<Group>().MoveRight();
				timeStamp = Time.time + timeBeforeHoldKey;
				shadow.GetComponent<ShadowBehaviour> ().FollowTetromino ();
				break;
			case "HardDrop":
				tetromino.GetComponent<Group>().HardDrop();
				break;
			case "Hold":
				if (spawner.GetComponent<Spawner>().lockHold == false)
					spawner.GetComponent<Spawner>().MobileUpdateHold = true;
				break;
			case "Pause":
				gameManager.GetComponent<GameManagerBehaviour>().Pause();
				break;
			case "ClockWiseTouch":
				mousePosition = Input.mousePosition;
				break;
			case "AntiClockWiseTouch":
				mousePosition = Input.mousePosition;
				break;
		}
		ImFocused = true;
	}

	void OnMouseUp()
	{
		ImFocused = false;
		if (Input.mousePosition.x > mousePosition.x - 10
			&& Input.mousePosition.x < mousePosition.x + 10
			&& Input.mousePosition.y > mousePosition.y - 10
			&& Input.mousePosition.y < mousePosition.y + 10)
		{
			switch (Action)
			{
				case "ClockWiseTouch":
					if (tetromino.GetComponent<Group> ().tetromino != "O")
						tetromino.GetComponent<Group> ().RotateRight ();
					shadow.GetComponent<ShadowBehaviour> ().FollowTetromino ();
					break;
				case "AntiClockWiseTouch":
					if (tetromino.GetComponent<Group> ().tetromino != "O")
						tetromino.GetComponent<Group> ().RotateLeft ();
					shadow.GetComponent<ShadowBehaviour> ().FollowTetromino ();
					break;
			}
		}
	}
}
