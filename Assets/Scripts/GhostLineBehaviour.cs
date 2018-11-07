using UnityEngine;
using System.Collections;

public class GhostLineBehaviour : MonoBehaviour
{
    public GameObject AliveGroup;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update ()
    {
        AliveGroup = GameObject.FindGameObjectWithTag("AliveGroup");
        if (AliveGroup)
        {
            gameObject.transform.position = new Vector3(AliveGroup.transform.position.x + AliveGroup.GetComponent<Group>().leftRightScale, transform.position.y, 0);
            transform.localScale = new Vector3(AliveGroup.GetComponent<Group>().width, 1, 1);
        }
    }

	public void HardDrop()
	{
		this.GetComponent<SpriteRenderer>().color = new Color(255.0f, 255.0f, 0.0f);
		Invoke ("ResetColor", 0.1f);
	}

	private void ResetColor()
	{
		this.GetComponent<SpriteRenderer>().color = new Color(255.0f, 255.0f, 255.0f);
	}
}
