using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;


public class MobSpawner
{
    public GameObject mobPrefab;
    public List<GameObject> instancesInField = new List<GameObject>();
    public int maxInstances { get; private set; }

    public MobSpawner(GameObject mobPrefab, int maxInstances)
    {
        this.mobPrefab = mobPrefab;
        this.maxInstances = maxInstances;
    }
}

public class Spawner : Root
{
    private bool _isInstanceLimited = true;
    public override bool isInstanceLimited { get { return _isInstanceLimited; } }

    private int _maxEachEntityInField = 5;
    public override int maxEachEntityInField { get { return _maxEachEntityInField; } }

    private string _objectName = "Spawner";
    public override string objectName { get { return _objectName; } }


    protected GameObject levelSystem;

    [Header("Spawner Parameters", order = 0)]
    [SerializeField] protected List<GameObject> spawnMobs = new List<GameObject>();
    [SerializeField] protected List<int> instancePerMob = new List<int>();
    protected Dictionary<string, MobSpawner> mobPrefabs = new Dictionary<string, MobSpawner>(); // Name of entity, mobSpawner

    [SerializeField] protected Transform[] activePoints = new Transform[2] { null, null};
    [SerializeField] protected Transform[] spawnPoints = new Transform[2] { null, null };

    [SerializeField] protected GameObject spawner;
    [SerializeField] protected Transform target;

    protected bool isAlive = true; 
    protected bool targetAlive = true; 
    protected bool canSpawn = true;

    public bool targetInProximity { get; protected set; }

    [SerializeField] protected int spawnInterval = 1;
    [SerializeField] protected float targetStraightDistance;

    void Awake()
    {
        GameMechanicsPropInit();
        InstanceInit();
    }

    void Start()
    {
        //activePoints = new List<Transform>() { GameObject.Find("LeftActivePoint"), GameObject.Find("RightActivePoint")}
        // Pseudo level mob prefab

        // Pseudo Mobs initializer
        for (int i=0; i < spawnMobs.Count; i++)
        {
            MobSpawner mobInstance = new MobSpawner(spawnMobs[i], instancePerMob[i]);
            mobPrefabs.Add(spawnMobs[i].GetComponent<Root>().objectName, mobInstance);
        }
    }

    void Update()
    {
        if (GameObject.Find("Player") == null) return;
        
        target = GameObject.Find("Player").transform;

        isAlive = spawner != null && spawner.GetComponent<Entity>().isAlive;
        targetAlive = target != null && target.GetComponent<Entity>().isAlive;
        // Checks if spawner object does exist

        if (canSpawn)
        {
            // Limitations
            Vector2 targetLocation = target.position;
            Vector2 lActivePnt = activePoints[0].position;
            Vector2 rActivePnt = activePoints[1].position;

            // Checks if target is inside the active points
            targetInProximity = targetLocation.x >= lActivePnt.x && targetLocation.x <= rActivePnt.x;
            if (targetInProximity)
            {
                // SpawnPoints
                Vector2 lSpawnPnt = spawnPoints[0].position;
                Vector2 rSpawnPnt = spawnPoints[1].position;
                Vector2 mainSpawnPnt = new Vector2(lSpawnPnt.x + ((rSpawnPnt.x - lSpawnPnt.x) * Random.Range(0, 101) / 100), rSpawnPnt.y);

                bool instanceCreated = CreateInstance(mainSpawnPnt);
            }
        }
    }

    private IEnumerator spawnReset()
    {
        canSpawn = false;
        yield return new WaitForSeconds(spawnInterval);
        canSpawn = isAlive && targetAlive;
    }

    public bool CreateInstance(Vector2 mainSpawnPnt)
    {
        bool isSuccess = false;
        if (mobPrefabs.Count > 0)
        {
            // Choose mob to spawn
            int idxMobChosen = Random.Range(0, mobPrefabs.Count);
            string objectName = new List<string>(mobPrefabs.Keys)[idxMobChosen];
            MobSpawner mobInstance = mobPrefabs[objectName];

            if (CanCreateInstance(mobInstance.mobPrefab) && mobInstance.instancesInField.Count < mobInstance.maxInstances)
            {
                GameObject newMob = (GameObject)Instantiate(mobInstance.mobPrefab, mainSpawnPnt, Quaternion.identity, GameObject.Find("Enemies").transform);

                newMob.GetComponent<Entity>().CreatedBySpawner(true, spawner, activePoints);

                mobPrefabs[objectName].instancesInField.Add(newMob);

                StartCoroutine(spawnReset());
                isSuccess = true;
            }
        }

        return isSuccess;
    }

    public void ClearInstance(string objectName, GameObject mobInstance)
    {
        mobPrefabs[objectName].instancesInField.Remove(mobInstance);
    }
}
