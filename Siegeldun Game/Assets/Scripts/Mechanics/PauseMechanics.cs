using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMechanics : MonoBehaviour
{
    public static bool isPlaying = true;
    private static bool _isPlaying = false;
    [SerializeField] public GameObject PauseMenuUI;

    void Awake()
    {
        if (GameMechanics.gameState == GameMechanics.GameState.InGame) PauseMenuUI = GameObject.Find("/GUI/PauseMenu");
    }

    // Update is called once per frame
    void Update()
    {
        if(PauseMenuUI != null && Input.GetKeyDown(KeyCode.Escape))
        {
            isPlaying = !isPlaying;
        }

        if (isPlaying != _isPlaying)
        {
            if (PauseMenuUI != null) PauseMenuUI.SetActive(isPlaying);
            Time.timeScale = (isPlaying) ? 1f : 0f;
        }
    }
}
