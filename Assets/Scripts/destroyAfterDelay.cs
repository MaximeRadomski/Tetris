using UnityEngine;
using System.Collections;

public class destroyAfterDelay : MonoBehaviour
{
    public float delay = 2.0f;

	// Use this for initialization
	void Start ()
    {
        Invoke("DestroyAfterDelay", delay);
	}
	
	void DestroyAfterDelay()
    {
        Destroy(gameObject);
    }
}
