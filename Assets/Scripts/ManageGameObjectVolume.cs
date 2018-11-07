using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ManageGameObjectVolume : MonoBehaviour
{
	private AudioSource[] aSources;
	// Use this for initialization
	void Start ()
	{
		aSources = GetComponents<AudioSource>();
		foreach (AudioSource source in aSources)
		{
			source.volume = source.volume * ((float)PlayerPrefs.GetInt("EffectsVolume") / 10);	
		}
	}
}
