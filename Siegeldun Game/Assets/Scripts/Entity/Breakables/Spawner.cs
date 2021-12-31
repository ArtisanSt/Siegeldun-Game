using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour
{
    [Header("Spawner Parameters", order = 0)]
    [SerializeField] protected List<GameObject> mobPrefabs = new List<GameObject>();
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
    }

    void Update()
    {
        this.isAlive = spawnerObject.GetComponent<Breakables>().isAlive;
        if (this.isAlive)
        {
            Vector2 targetLocation = target.position;
            Vector2 lActivePnt = activePoints[0].position;
            Vector2 rActivePnt = activePoints[1].position;

            if (targetLocation.x >= lActivePnt.x && targetLocation.x <= rActivePnt.x)
            {
                if (canSpawn)
                {
                    // SpawnPoints
                    Vector2 lSpawnPnt = spawnPoints[0].position;
                    Vector2 rSpawnPnt = spawnPoints[1].position;
                    Vector2 mainSpawnPnt = new Vector2(lSpawnPnt.x + ((rSpawnPnt.x - lSpawnPnt.x) * Random.Range(0, 101) / 100), rSpawnPnt.y);

                    GameObject newMob = Instantiate(mobPrefabs[Random.Range(0, mobPrefabs.Count)], mainSpawnPnt, Quaternion.identity, GameObject.Find("Enemies").transform);
                    newMob.GetComponent<EnemyAI>().hasLimitations = true;
                    newMob.GetComponent<EnemyAI>().activePoints = this.activePoints;
                    canSpawn = false;
                    StartCoroutine(spawnReset());
                }
            }
        }
    }

    private IEnumerator spawnReset()
    {
        yield return new WaitForSeconds(spawnInterval);
        canSpawn = true;
    }
}
