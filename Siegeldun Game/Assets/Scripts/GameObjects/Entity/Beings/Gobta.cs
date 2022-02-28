using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gobta: NPC
{
    [SerializeField] private List<GameObject> dummiesPrefabs;

    // ========================================= ITEM DROPS INITIALIZATION =========================================
    protected override void itemDropsInit()
    {

    }

    // ========================================= UNITY MAIN METHODS =========================================
    protected override void Awake()
    {
        base.Awake();
        NPCInit();
    }

    // Update is called once per frame
    protected override void Update()
    {
        base.Update();
    }

    // Updates Every Physics Frame
    protected override void FixedUpdate()
    {
        base.FixedUpdate(); // Updates the Animation of the Entity
    }

    // ========================================= NPC ABILITY =========================================
    protected override void Ability()
    {
        base.Ability();
        isDoingAbility = true;
        anim.SetTrigger("ability");
        StartCoroutine(SpawnDummies(0));

    }

    IEnumerator SpawnDummies(int count = 0)
    {
        yield return new WaitForSeconds(1f);

        int num = Random.Range(0, dummiesPrefabs.Count);
        GameObject x = Drop(1, new Vector2(0, 0), dummiesPrefabs[num], GameObject.Find("Entities").transform, true);
        x.GetComponent<NPC>().InstanceTimed(5);

        if (count < 2) StartCoroutine(SpawnDummies(count + 1));
        else
        {
            lastAbilityTime = Time.time;
            isDoingAbility = false;
        }
    }

    // ========================================= NPC DEATH =========================================
    // Executes after death animation and instance clearing on memory
    protected override void Die()
    {
        base.Die();
        // Clear Inventory
    }

    // Executes right before entity to be destroyed
    protected override void OnEntityDestroy()
    {
        base.OnEntityDestroy();
    }
}
