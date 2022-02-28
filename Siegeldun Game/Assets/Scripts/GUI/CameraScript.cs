using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class CameraScript : MonoBehaviour
{
    [SerializeField] public Transform player;

    void Awake()
    {
        player = GameObject.Find("Player").transform;
        SetCamera(player);
    }

    void Update()
    {
        if (player == null && GameObject.Find("Player") != null)
        {
            player = GameObject.Find("Player").transform;
            SetCamera(player);
        }
    }

    public void SetCamera(Transform player)
    {
        if (this.player == null || player == null) return;

        this.player = player;
        GetComponent<CinemachineVirtualCamera>().Follow = player;
    }
}
