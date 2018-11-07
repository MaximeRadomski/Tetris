using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuMusic : MonoBehaviour
{
    public AudioSource currentAudioSource;

    private float currentAudioSourceOldVolume;
    static bool AudioBegin = false;
    private bool volumeDown = false;
    private float basicAudioVolume = 0.0F;

    public void Awake()
    {
        if (PlayerPrefs.GetInt("VolumeSet") == 0)
        {
            PlayerPrefs.SetInt("VolumeSet", 1);
            PlayerPrefs.SetInt("MusicVolume", 10);
            PlayerPrefs.SetInt("EffectsVolume", 10);
        }
        currentAudioSource = GetComponent<AudioSource>();
        basicAudioVolume = currentAudioSource.volume;
        currentAudioSource.volume = basicAudioVolume * ((float)PlayerPrefs.GetInt("MusicVolume") / 10);
        currentAudioSourceOldVolume = currentAudioSource.volume;
        if (!AudioBegin)
        {
            currentAudioSource.Play();
            DontDestroyOnLoad(gameObject);
            AudioBegin = true;
        }
    }

    public void AlterMusicVolume()
    {
        currentAudioSource.volume = basicAudioVolume * ((float)PlayerPrefs.GetInt("MusicVolume") / 10);
        currentAudioSource.volume = basicAudioVolume * ((float)PlayerPrefs.GetInt("MusicVolume") / 10);
    }

    public void Update()
    {
        if (Input.GetKeyDown(KeyCode.M)) //MUTE
        {
            if (volumeDown == false)
            {
                currentAudioSource.volume = 0;
                volumeDown = true;
            }
            else
            {
                currentAudioSource.volume = currentAudioSourceOldVolume;
                volumeDown = false;
            }
        }
        if (SceneManager.GetActiveScene().name == "GetReady" || SceneManager.GetActiveScene().name == "Marathon" || SceneManager.GetActiveScene().name == "FortyLines" || SceneManager.GetActiveScene().name == "Ultra")
        {
            currentAudioSource.Stop();
            AudioBegin = false;
            Destroy(gameObject);
        }
    }
}
