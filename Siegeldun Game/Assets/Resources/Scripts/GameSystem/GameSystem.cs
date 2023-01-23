using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameSystem : SingletonDontDestroy<GameSystem>
{
    // First one to be awakened and last one to get destroyed
    protected override void Awake()
    {
        if (initialized) return;
        state = State.Opening;

        base.Awake();
        /*ExecutionInit();
        MainMenuInit();



        GameInit();*/
    }

    protected override void Start()
    {
        if (initialized) return;

        base.Start();

        state = State.MainMenu;
        initialized = true; // last line
    }

    protected override void Update()
    {
        base.Update();

        // Game Preparation Phase
        // Scene Preparation Phase
        // Scene Change Phase
        // Scene End Phase
        // Main Menu Phase
        // Closing Phase 
    }

    protected virtual void FixedUpdate()
    {

    }

    protected virtual void LateUpdate()
    {


    }

    // When turned disabled
    protected virtual void OnDisable()
    {

    }

    // When turned enabled
    protected virtual void OnEnable()
    {

    }

    // When scene ends
    protected virtual void OnDestroy()
    {

    }


    // ============================== OBJECT PROPERTIES AND METHODS ==============================
    public static bool initialized { get; private set; }
    public enum State { Opening, MainMenu, Preparation, Playing, Ending, Saving, Closing, Debug }
    //public State state { get; private set; }
    public State state { get { return State.Debug; } set { State x = value; } }

    public List<Difficulty> difficulties { get; private set; }

    public void SetSiegeldunData(SiegeldunData sgldn)
    {
        if (!(state == State.Opening || state == State.Debug)) return;
        difficulties = new List<Difficulty>();
        for (int i=0; i < sgldn.difficulties.Length; i++)
        {
            difficulties.Add(new Difficulty());
            difficulties[i].FromJson(sgldn.difficulties[i]);
        }
    }

    // Calls when game is opened
    private void ExecutionInit()
    {
        SetSiegeldunData(DataManager.LoadSiegeldunData());
        // loads all data

    }

    // Loads Main Menu
    private void MainMenuInit()
    {

    }


    private void GameInit()
    {
        // Debug
        paused = false;
        NewGame(0);
    }

    public static bool paused;
    public GameData gameData { get; private set; }
    public int difficulty => gameData.difficulty.multiplier;
    [SerializeField] public LayerMask groundLayer;

    public static string gamePath => Application.persistentDataPath;
    public static string dataPath => $"{gamePath}/Data";
    public static string siegeldunDataPath => $"{dataPath}/Siegeldun.sgldn";
    public string GameDataPath(int saveIdx) => $"{dataPath}/GameData_{saveIdx}.sgldn";

    public void NewGame(int difficulty)
    {
        gameData = DataManager.CreateNewGame(difficulties[difficulty]);
    }

    public void LoadGame(int saveIdx)
    {
        string path = GameDataPath(saveIdx);
        gameData = DataManager.LoadSgldnFile<GameData>(path);
    }

    public void SaveGame(int saveIdx)
    {
        string path = GameDataPath(saveIdx);
        DataManager.CreateSgldnFile<GameData>(path, gameData);
    }

    public void DeleteGame(int saveIdx)
    {
        string path = GameDataPath(saveIdx);
        DataManager.DeleteSgldnFile(path);
    }
}
