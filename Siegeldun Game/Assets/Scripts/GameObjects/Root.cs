using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public abstract class Root : Process
{
    // ========================================= Game Properties =========================================
    protected static GameMechanics gameMechanics;
    protected LevelProperties lvlProp;

    protected int idxDiff;

    [Header("ROOT SETTINGS", order = 1)]
    [SerializeField] public string objectName;
    [SerializeField] public bool isInstanceLimited;// Pseudo
    [SerializeField] public int maxEachEntityInField; // Pseudo

    [SerializeField] protected static int maxtotalEntityInField = 500; // Pseudo
    protected static Dictionary<string, List<GameObject>> entityInstances = new Dictionary<string, List<GameObject>>();

    protected virtual void PrefabsInit() { }




    // =========================================  INSTANTIATION =========================================
    protected void GameMechanicsPropInit()
    {
        if (gameMechanics == null) gameMechanics = GameObject.Find("Game System/GameMechanics").GetComponent<GameMechanics>();
        idxDiff = (int)GlobalVariableStorage.difficulty;
        //lvlProp = gameMechanics.curLvlProp;

        PrefabsInit();
    }

    protected void InstanceInit()
    {
        if (!entityInstances.ContainsKey("All")) entityInstances.Add("All", new List<GameObject>());
        if (!entityInstances.ContainsKey(objectName)) entityInstances.Add(objectName, new List<GameObject>());

        // Instance Limiter
        if (CanCreateInstance(gameObject)) InstanceCreated(objectName, gameObject);
        else Destroy(gameObject);
    }




    // =========================================  INSTANCE METHODS =========================================
    private void InstanceInit(string objectName)
    {
        if (!entityInstances.ContainsKey(objectName)) entityInstances.Add(objectName, new List<GameObject>());
    }

    protected bool CanCreateInstance(GameObject mobPrefab)
    {
        return entityInstances["All"].Count < maxtotalEntityInField && (!isInstanceLimited || (isInstanceLimited && entityInstances[mobPrefab.GetComponent<Root>().objectName].Count < maxEachEntityInField));
    }

    protected int InstancesCount(string objectName)
    {
        return entityInstances[objectName].Count;
    }

    protected void InstanceCreated(string objectName, GameObject entityObject)
    {
        entityInstances["All"].Add(entityObject);
        entityInstances[objectName].Add(entityObject);
    }

    protected void InstanceDestroyed(string objectName, GameObject entityObject)
    {
        entityInstances["All"].Remove(entityObject);
        entityInstances[objectName].Remove(entityObject);
    }

    public static void DestroyAllInstances()
    {
        List<string> pseudoInstance = new List<string>();
        foreach(string key in entityInstances.Keys)
        {
            pseudoInstance.Add(key);
        }

        foreach(string eachInstance in pseudoInstance)
        {
            entityInstances.Remove(eachInstance);
        }
    }
}
