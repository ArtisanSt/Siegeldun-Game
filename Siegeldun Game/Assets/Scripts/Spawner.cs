using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour
{
    [Header("Spawner Parameters")]
    [SerializeField] GameObject mobPrefab;
    [SerializeField] Transform spawnPoint;
    [SerializeField] float mobInterval = 10f;

    [Header("Proximity Parameters")]
    [SerializeField] Transform player;
    bool isColliding;

    void Start()
    {
        GameObject newMob = Instantiate(mobPrefab, spawnPoint.transform.position, Quaternion.identity);
        //StartCoroutine(spawnMob(mobInterval, mobPrefab));
    }

    void OnTriggerEnter2D(Collider2D coll)
    {
        if(coll.gameObject.name == "Player")
        {
            if(isColliding) return;
            isColliding = true;
            Debug.Log("Player entered");
            StartCoroutine(spawnMob(mobInterval, mobPrefab));
        }
        
        return;
    }

    private IEnumerator spawnMob(float interval, GameObject enemy)
    {
        yield return new WaitForSeconds(interval);
        // Create new game object
        GameObject newMob = Instantiate(enemy, spawnPoint.transform.position, Quaternion.identity);
        StartCoroutine(spawnMob(interval, enemy));
    }
}
