using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class MobSpawner
{
    public static int spawnerEntityInField = 0;
    public static int maxSpawnerEntityInField = 5;
    public GameObject mobPrefab;
    public List<GameObject> instancesInField = new List<GameObject>();
    public int maxInstances;

    public MobSpawner(GameObject mobPrefab, int maxInstances)
    {
        this.mobPrefab = mobPrefab;
        this.maxInstances = maxInstances;
    }

    public bool CanCreateInstance()
    {
        return spawnerEntityInField < maxSpawnerEntityInField && instancesInField.Count < maxInstances;
    }

    public void CreateInstance(GameObject mobInstance)
    {
        instancesInField.Add(mobInstance);
        spawnerEntityInField++;
    }

    public void RemoveInstance(GameObject mobInstance)
    {
        instancesInField.Remove(mobInstance);
        spawnerEntityInField--;
    }
}

public class Spawner : MonoBehaviour
{
    protected GameObject levelSystem;

    [Header("Spawner Parameters", order = 0)]
    protected List<string> levelMobs = new List<string>(); 
    [SerializeField] protected Dictionary<string, MobSpawner> mobPrefabs = new Dictionary<string, MobSpawner>(); // Name of entity, mobSpawner

    [SerializeField] protected List<Transform> activePoints = new List<Transform>();
    [SerializeField] protected List<Transform> spawnPoints = new List<Transform>();

    [SerializeField] protected GameObject spawnerObject;
    [SerializeField] protected Transform target;

    protected bool isAlive = true; 
    protected bool canSpawn = true;
    [SerializeField] protected int spawnInterval = 1;
    [SerializeField] protected float targetStraightDistance;

    void Awake()
    {
        //activePoints = new List<Transform>() { GameObject.Find("LeftActivePoint"), GameObject.Find("RightActivePoint")}
        // Pseudo level mob prefab
    }

    void Start()
    {
        levelSystem = GameObject.Find("/Mechanics/LevelSystem");

        // Pseudo level initializer
        levelMobs.Add("Wolf");
        MobSpawner mobInstance = new MobSpawner(AssetDatabase.LoadAssetAtPath<UnityEngine.GameObject>($"Assets/Prefabs/EntityPrefabs/Wolf_No_Drop.prefab"), 20);
        mobPrefabs.Add("Wolf", mobInstance);

        

        //activePoints[0] = gameObject.transform.GetChild(0).GetChild(0);
        //activePoints[1] = gameObject.transform.GetChild(0).GetChild(1);
        //spawnPoints[0] = gameObject.transform.GetChild(0).GetChild(2);
        //spawnPoints[1] = gameObject.transform.GetChild(0).GetChild(3);

        spawnerObject = gameObject.transform.GetChild(1).gameObject;
        target = GameObject.Find("Player").transform;
    }

    void Update()
    {
        if (canSpawn)
        {
            // Checks if spawner object does exist
            if (spawnerObject != null && target != null)
            {
                // Checks if the spawner is alive
                this.isAlive = spawnerObject.GetComponent<Breakables>().isAlive;
                if (this.isAlive)
                {
                    Vector2 targetLocation = target.position;
                    Vector2 lActivePnt = activePoints[0].position;
                    Vector2 rActivePnt = activePoints[1].position;

                    // Checks if target is inside the active points
                    if (targetLocation.x >= lActivePnt.x && targetLocation.x <= rActivePnt.x)
                    {
                        // SpawnPoints
                        Vector2 lSpawnPnt = spawnPoints[0].position;
                        Vector2 rSpawnPnt = spawnPoints[1].position;
                        Vector2 mainSpawnPnt = new Vector2(lSpawnPnt.x + ((rSpawnPnt.x - lSpawnPnt.x) * Random.Range(0, 101) / 100), rSpawnPnt.y);

                        int idxMobChosen = Random.Range(0, levelMobs.Count);
                        MobSpawner mobInstance = mobPrefabs[levelMobs[idxMobChosen]];

                        if (mobInstance.CanCreateInstance())
                        {
                            GameObject newMob = (GameObject)Instantiate(mobInstance.mobPrefab, mainSpawnPnt, Quaternion.identity, GameObject.Find("Enemies").transform);
                            mobInstance.CreateInstance(newMob);

                            newMob.GetComponent<EnemyAI>().spawnerObject = spawnerObject;
                            newMob.GetComponent<EnemyAI>().hasLimitations = true;
                            newMob.GetComponent<EnemyAI>().activePoints = this.activePoints;
                            canSpawn = false;
                            StartCoroutine(spawnReset());
                        }
                    }
                }
            }
            else
            {
                canSpawn = false;
            }
        }
    }

    private IEnumerator spawnReset()
    {
        yield return new WaitForSeconds(spawnInterval);
        canSpawn = true;
    }

    public void ClearInstance(string entityMob, GameObject entityInstance)
    {
        MobSpawner mobInstance = mobPrefabs[entityMob];
        mobInstance.RemoveInstance(entityInstance);
    }
}
