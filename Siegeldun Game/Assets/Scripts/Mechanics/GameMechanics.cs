using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

[System.Serializable]
public class DifficultyProperties
{
    public GlobalVariableStorage.Difficulty difficulty = GlobalVariableStorage.Difficulty.Normal;
    public int statsMultiplier = 1;
    public int maxBulwarkRes = -1;
    public int maxEShrineRes = -1;
}

public class GameMechanics : MonoBehaviour
{
    // ========================================= GAME MECHANICS PROPERTIES =========================================
    public static GameMechanics instance;

    public static LevelProperties levelProperties;

    [SerializeField] public GameObject loadingScreen;
    [SerializeField] public Slider slider;
    [SerializeField] public Text progressText;

    // Game Properties
    public enum GameState { MainMenu, Loading, InGame }
    public static GameState gameState = GameState.InGame;

    [SerializeField] public List<DifficultyProperties> difficultyProperties;

    [SerializeField] public List<int> numberOfLevelsPerAct;

    public Dictionary<string, string> scenes = new Dictionary<string, string>();

    void Awake()
    {
        SaveAndLoadManager.LoadGameData();
        instance = this;
        foreach (EditorBuildSettingsScene scene in EditorBuildSettings.scenes)
        {
            scenes.Add(GetSceneName(scene.path), scene.path);
        }
    }

    public void StartNewGame(string difficulty)
    {
        for (int i=0; i < difficultyProperties.Count; i++)
        {
            if (difficultyProperties[i].difficulty.ToString() == difficulty)
            {
                GlobalVariableStorage.curDifficulty = difficultyProperties[i];
                DefaultsLoader();
                LoadScene(GetSceneName(SpecialScene.TutorialGrounds));
                break;
            }
        }
    }

    public void ContinueGame()
    {
        LoadScene(GetSceneName(GlobalVariableStorage.curAct, GlobalVariableStorage.curLvl));
    }

    private void DefaultsLoader()
    {
        GlobalVariableStorage.difficulty = GlobalVariableStorage.curDifficulty.difficulty;
        GlobalVariableStorage.statsMultiplier = GlobalVariableStorage.curDifficulty.statsMultiplier;
        GlobalVariableStorage.maxBulwarkLives = GlobalVariableStorage.curDifficulty.maxBulwarkRes;
        GlobalVariableStorage.maxEShrineLives = GlobalVariableStorage.curDifficulty.maxEShrineRes;
    }

    public string GetSceneName(string scenePath)
    {
        while (scenePath.IndexOf('/') != -1)
        {
            scenePath = scenePath.Substring(scenePath.IndexOf('/') + 1);
        }
        scenePath = scenePath.Substring(0, scenePath.IndexOf(".unity"));
        return scenePath;
    }

    public string GetSceneName(int curAct, int curLvl)
    {
        if (curAct != 0) return $"Level {curAct} - {curLvl}";
        return $"{((curLvl == 0) ? "Testing Grounds" : "Tutorial Grounds")}";
    }

    public enum SpecialScene { MainMenu, TestingGrounds, TutorialGrounds}
    public string GetSceneName(SpecialScene specialScene)
    {
        if (specialScene == SpecialScene.MainMenu) return "Main Menu";
        else if (specialScene == SpecialScene.TestingGrounds) return "Testing Grounds";
        else if (specialScene == SpecialScene.TutorialGrounds) return "Tutorial Grounds";
        return "";
    }

    public void LoadScene(string sceneName)
    {
        GlobalVariableStorage.numberOfLevelsPerAct = numberOfLevelsPerAct;
        StartCoroutine(LoadSceneAsynchronously(scenes[sceneName]));
    }

    private IEnumerator LoadSceneAsynchronously(string scenePath)
    {
        Root.DestroyAllInstances();
        PauseMechanics.instance.SetPlayTime(false, false);

        gameState = GameState.Loading;

        AsyncOperation asyncLoading = SceneManager.LoadSceneAsync(scenePath);

        loadingScreen.SetActive(true);

        // Wait until the asynchronous scene fully loads
        while (!asyncLoading.isDone)
        {
            float progress = Mathf.Clamp01(asyncLoading.progress / .9f);

            slider.value = progress;
            progressText.text = $"{progress * 100f}%";

            yield return null;
        }

        bool inMainMenu = GetSceneName(scenePath) == GetSceneName(SpecialScene.MainMenu);
        gameState = (inMainMenu) ? GameState.MainMenu : GameState.InGame;
        loadingScreen.SetActive(false);
        PauseMechanics.instance.SetPlayTime(true, false);
    }

    // Evaluates if the player can be resurrected or the game is over
    public void PlayerDied()
    {   
        if (GlobalVariableStorage.maxBulwarkLives != -1 && GlobalVariableStorage.curBulwarkLives == 0)
        {
            if (GlobalVariableStorage.maxEShrineLives != -1)
            {
                if (GlobalVariableStorage.curEShrineLives == 0)
                {
                    BackToMainMenu();
                    return;
                }
                else GlobalVariableStorage.curEShrineLives--;
            }

            GlobalVariableStorage.curBulwarkLives = GlobalVariableStorage.maxBulwarkLives;
            GlobalVariableStorage.curLvl = 1;
            LoadScene(GetSceneName(GlobalVariableStorage.curAct, GlobalVariableStorage.curLvl));
            return;
        }
        else if (GlobalVariableStorage.maxBulwarkLives != -1 && GlobalVariableStorage.curBulwarkLives != 0)
        {
            GlobalVariableStorage.curBulwarkLives--;
        }

        LevelProperties.instance.Resurrect();
        return;
    }

    // Loads back to main menu
    public void BackToMainMenu()
    {
        SaveAndLoadManager.SaveLevelData();
        LoadScene(GetSceneName(SpecialScene.MainMenu));
    }

    public void StartNextLevel()
    {
        GlobalVariableStorage.curLvl++;
        if (GlobalVariableStorage.curLvl == GlobalVariableStorage.numberOfLevelsPerAct[GlobalVariableStorage.curAct])
        {
            GlobalVariableStorage.curAct++;
            if (GlobalVariableStorage.curAct == GlobalVariableStorage.numberOfLevelsPerAct.Count)
            {
                LoadScene(GetSceneName(SpecialScene.MainMenu));
                return;
            }
        }
        LoadScene(GetSceneName(GlobalVariableStorage.curAct, GlobalVariableStorage.curLvl));
    }
}
