using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Settings : MonoBehaviour
{
    public bool isOn = false;
    public bool bgmOn = true;
    public bool sfxOn = true;
    public GameObject pauseMenuBg;
    public GameObject bgmDisable, sfxDisable;

    public void Start()
    {
        pauseMenuBg = GameObject.Find("/GUI/Menu");
        bgmDisable = GameObject.Find("/GUI/Menu/BGM/bgmDisable");
        sfxDisable = GameObject.Find("/GUI/Menu/SFX/sfxDisable");
    }

    public void Update()
    {
        pauseMenuBg.SetActive(isOn);
        bgmDisable.GetComponent<Image>().enabled = bgmOn;
        sfxDisable.GetComponent<Image>().enabled = sfxOn;
    }

    public void SettingsToggle()
    {
        isOn = !isOn;
    }

    public void BgmToggle()
    {
        bgmOn = !bgmOn;
        SoundManager.Instance.ToggleMusic();
    }

    public void SfxToggle()
    {
        sfxOn = !sfxOn;
        SoundManager.Instance.ToggleEffects();
    }
}
