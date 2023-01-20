using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameSystem : SingletonDontDestroy<GameSystem>
{
    //UnityEvent instanceConfigEvent;

    protected override void Awake()
    {
        base.Awake();
        Init();
        GameDataManager.DataInit();
        GameDataManager.LoadSavedData(0);
        Debug.Log(GameDataManager.currentData);
    }

    protected override void Start()
    {
        base.Start();
    }

    protected override void Update()
    {
        base.Update();
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
    private void Init()
    {
        paused = false;
        difficulty = 0;
    }

    public static bool paused;
    public List<Difficulty> difficulties;
    public GameDAta gameData { get; private set; }
    [SerializeField] public LayerMask groundLayer;

    public void NewGame(int difficulty)
    {
        GameDataManager.CreateGameData("New Game", difficulty);
    }
}
