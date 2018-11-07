using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PopupBehavior : MonoBehaviour
{
	public AudioSource PopupSound;

	public void PlayPopupSound()
	{
		PopupSound.pitch = 1;
		var tmpPitch = (float)(Random.Range(-50, 51)/100.0f); //51 is exclusive, so : -50 >= value <= 50
		PopupSound.pitch += tmpPitch;
		PopupSound.Play();
	}
}
