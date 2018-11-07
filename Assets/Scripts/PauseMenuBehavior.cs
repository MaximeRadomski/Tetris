using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PauseMenuBehavior : MonoBehaviour
{
    private AudioSource[] aSources;

    // Use this for initialization
    void Start ()
    {
        aSources = GetComponents<AudioSource>();
        aSources[0].volume = 1 * ((float)PlayerPrefs.GetInt("EffectsVolume") / 10);
        aSources[1].volume = 1 * ((float)PlayerPrefs.GetInt("EffectsVolume") / 10);
    }

    public void AlterEffectsVolume()
    {
        aSources[0].volume = 1 * ((float)PlayerPrefs.GetInt("EffectsVolume") / 10);
        aSources[1].volume = 1 * ((float)PlayerPrefs.GetInt("EffectsVolume") / 10);
    }

    public void HoverButton()
    {
        aSources[0].Play();
    }

    public void SelectButton()
    {
        aSources[1].Play();
    }
}
