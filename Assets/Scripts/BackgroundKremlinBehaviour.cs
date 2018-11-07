using UnityEngine;
using System.Collections;

public class BackgroundKremlinBehaviour : MonoBehaviour
{
	private static int decreaseCount = 0;
    private int i = 0;
    private bool decreasing = false;
    private bool increasing = false;

    public void Decrease()
    {
        decreasing = true;
        if (i < 25)
        {
            Vector3 positionTmp = transform.position;
            if (positionTmp.y <= -10.5)
            {
                decreasing = false;
                i = 0;
            }
            positionTmp.y -= 0.2f;
            transform.position = positionTmp;
            ++i;
        }
        else
        {
            decreasing = false;
            i = 0;
			++decreaseCount;
			if (decreaseCount == 4)
			{
				ThongSuccess ();
			}
        }
    }

    public void Increase()
    {
        increasing = true;
        Vector3 positionTmp = transform.position;
        if (positionTmp.y < 9.5f)
        {
            positionTmp.y += 0.2f;
            transform.position = positionTmp;
        }
        else
        {
            increasing = false;
			decreaseCount = 0;
        }
    }

    void Update()
    {
        if (decreasing == true)
        {
            Decrease();
        }
        if (increasing == true)
        {
            Increase();
        }
    }

	private static void ThongSuccess()
	{
		if (PlayerPrefs.GetString ("UnlockedCurtainsStyles", "10000000000000000000") [2] == '1')
		{
			GameObject.Find("$GameManager").GetComponent<GameManagerBehaviour>().DisplayPopup("Baguette Deja Vu");
		}
		else
		{
			string tmpUnlockedCurtainsStyles = "";
			tmpUnlockedCurtainsStyles += PlayerPrefs.GetString ("UnlockedCurtainsStyles", "10000000000000000000");
			tmpUnlockedCurtainsStyles = tmpUnlockedCurtainsStyles.Remove(2,1);
			tmpUnlockedCurtainsStyles = tmpUnlockedCurtainsStyles.Insert(2,"1");
			PlayerPrefs.SetString ("UnlockedCurtainsStyles", tmpUnlockedCurtainsStyles);
			GameObject.Find("$GameManager").GetComponent<GameManagerBehaviour>().DisplayPopup("Unlocked:\nBaguette Deja Vu");	
		}
	}
}
