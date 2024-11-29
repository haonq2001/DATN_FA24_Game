using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class audioManager : MonoBehaviour
{
    public static audioManager Instance;

    public Sound[] musicSounds, sfxSounds;
    public AudioSource musicSource, sfxSource;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
            musicSource.mute = PlayerPrefs.GetInt("MusicMuted", 0) == 1;
            sfxSource.mute = PlayerPrefs.GetInt("SFXMuted", 0) == 1;
        }
        else
        {
            Destroy(gameObject);
        }
    }
    public void Start()
    {
        PlayMusic("theme");

    }
    public void PlayMusic(string name)
    {
        Sound s = Array.Find(musicSounds, x => x.name == name);
        if (s == null)
        {
            Debug.Log("Sound not found");
        }
        else
        {
            musicSource.clip = s.clip;
            musicSource.Play();
        }
    }

    public void PlaySFX(string name)
    {
        Sound s = Array.Find(sfxSounds, x => x.name == name);
        if (s == null)
        {
            Debug.Log("Sound not found");
        }
        else
        {
            sfxSource.PlayOneShot(s.clip);
        }
    }
    public void ToggleMusic()
    {
        if (musicSource != null)
        {
            musicSource.mute = !musicSource.mute;
            PlayerPrefs.SetInt("MusicMuted", musicSource.mute ? 1 : 0);
            PlayerPrefs.Save();
        }
        else
        {
            Debug.LogError("Music source is missing!");
        }
    }

    public void ToggleSFX()
    {
        if (sfxSource != null)
        {
            sfxSource.mute = !sfxSource.mute;
            PlayerPrefs.SetInt("SFXMuted", sfxSource.mute ? 1 : 0);
            PlayerPrefs.Save();
        }
        else
        {
            Debug.LogError("SFX source is missing!");
        }
    }
}
