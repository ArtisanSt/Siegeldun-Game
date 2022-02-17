using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class VolumeSlider : MonoBehaviour
{
    [SerializeField] public bool bgmSlider, sfxSlider;
    [SerializeField] public Slider slider;
    // Start is called before the first frame update
    void Start()
    {
        if(bgmSlider)
            slider.onValueChanged.AddListener(val => SoundManager.Instance.ChangeMusicVolume(val));
        else if(sfxSlider)
            slider.onValueChanged.AddListener(val => SoundManager.Instance.ChangeEffectsVolume(val));
    }
}
