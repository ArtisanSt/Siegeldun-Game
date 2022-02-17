using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayBGM : MonoBehaviour
{
    [SerializeField] private AudioClip bgm;

    void Start()
    {
        SoundManager.Instance.PlayBGM(bgm);
    }
}
