using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    public static SoundManager Instance;

    private float bgmVolume = 1;
    [SerializeField] private AudioSource musicSource, effectSource;
    [SerializeField] private AudioClip interactSFX;

    void Awake()
    {
        if(Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void PlayBGM(AudioClip bgm)
    {
        musicSource.clip = bgm;
        musicSource.loop = true;
        musicSource.volume = bgmVolume;
        musicSource.Play();
    }

    public void PlayInteract()
    {
        effectSource.PlayOneShot(interactSFX);
    }

    public void PlaySFX(AudioClip sfx)
    {
        effectSource.PlayOneShot(sfx);
    }

    public void ChangeMusicVolume(float value)
    {   
        bgmVolume = value;
        musicSource.volume = value;
    }

    public void ChangeEffectsVolume(float value)
    {
        effectSource.volume = value;
    }

    public void ToggleMusic()
    {
        musicSource.mute = !musicSource.mute;
    }

    public void ToggleEffects()
    {
        effectSource.mute = !effectSource.mute;
    }
}
