using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SoundManager : MonoBehaviour
{
    public static SoundManager instance;

    [SerializeField] private GameObject imgBGMDisable;
    [SerializeField] private GameObject imgSFXDisable;


    [Header("BGM SETTINGS", order = 0)]
    [SerializeField] public bool hasBGM;
    [SerializeField] public bool isOnBGM = true;
    [SerializeField] private float bgmVolume = 1;
    [SerializeField] public AudioSource bgmSource;
    [SerializeField] public Slider bgmSlider;


    [Header("SFX SETTINGS", order = 1)]
    [SerializeField] public bool hasSFX;
    [SerializeField] public bool isOnSFX = true;
    [SerializeField] private float sfxVolume = 1;
    [SerializeField] public AudioSource sfxSource;
    [SerializeField] public Slider sfxSlider;


    [Header("ADDITIONAL SOUND SETTINGS", order = 2)]
    [SerializeField] public AudioClip interactSFX;
    [SerializeField] public AudioClip daggerSFX;


    void Awake()
    {
        instance = this;
        if (bgmSlider != null) bgmSlider.onValueChanged.AddListener(val => ChangeBGMVolume(val));
        if (sfxSlider != null) sfxSlider.onValueChanged.AddListener(val => ChangeSFXVolume(val));
        hasBGM = bgmSource != null;
        hasSFX = sfxSource != null;
        SetBGM(isOnBGM);
        SetSFX(isOnSFX);
    }

    void Update()
    {
        SFXUpdater();
        BGMUpdater();
    }

    private void BGMUpdater()
    {
        bgmVolume = bgmSlider.value;
        bgmSource.volume = bgmVolume;
        bgmSource.mute = bgmVolume == 0;
    }

    private void SFXUpdater()
    {
        sfxVolume = sfxSlider.value;
        sfxSource.volume = sfxVolume;
        sfxSource.mute = sfxVolume == 0;
    }


    // ==================================================== BGM SETTINGS ====================================================
    public void PlayBGM(AudioClip bgm)
    {
        if (!hasBGM) return;
        bgmSource.clip = bgm;
        bgmSource.loop = true;
        bgmSource.Play();
    }

    public void ToggleBGM()
    {
        SetBGM(!isOnBGM);
    }

    public void SetBGM(bool isOnBGM)
    {
        if (!hasBGM) return;

        if (this.isOnBGM != isOnBGM) bgmSlider.value = (isOnBGM) ? 1 : 0;

        this.isOnBGM = isOnBGM;
        imgBGMDisable.SetActive(!isOnBGM);
    }

    public void ChangeBGMVolume(float value)
    {
        if (!hasBGM) return;
        bgmSlider.value = value;
        SetBGM(value != 0);
    }


    // ==================================================== SFX SETTINGS ====================================================
    public void PlaySFX(AudioClip sfx)
    {
        if (!hasSFX) return;
        sfxSource.PlayOneShot(sfx);
    }

    public void ToggleSFX()
    {
        SetSFX(!isOnSFX);
    }

    public void SetSFX(bool isOnSFX)
    {
        if (!hasSFX) return;

        if (this.isOnSFX != isOnSFX) sfxSlider.value = (isOnSFX) ? 1 : 0;

        this.isOnSFX = isOnSFX;
        imgSFXDisable.SetActive(!isOnSFX);
    }

    public void ChangeSFXVolume(float value)
    {
        if (!hasSFX) return;
        sfxSlider.value = value;
        SetSFX(value != 0);
    }


    // ==================================================== ADDITIONAL SETTINGS ====================================================
    public void PlayInteract()
    {
        sfxSource.PlayOneShot(interactSFX);
    }

    public void PlayDagger()
    {
        sfxSource.PlayOneShot(daggerSFX);
    }
}
