using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public abstract class Root : MonoBehaviour
{
    // ========================================= Game Properties =========================================
    [SerializeField] protected GameObject systemGameObject;
    [SerializeField] protected GameMechanics gameMechanics;
    protected LevelProperties lvlProp;

    protected static int difficulty = GlobalVariableStorage.gameDifficulty; // Pseudo Difficulty
    protected static int idxDiff = difficulty - 1;

    protected System.Func<int, bool> ChanceRandomizer = dropChance => Random.Range(0, dropChance) == 0;
    protected System.Func<float, float, bool> TimerIncrement = (timeStart, timeDuration) => Time.time - timeStart >= timeDuration;

    protected List<float> curProcess = new List<float>();


    public abstract string objectName { get; }
    public abstract bool isInstanceLimited { get; } // Pseudo
    public abstract int maxEachEntityInField { get; } // Pseudo

    [SerializeField] protected static int maxtotalEntityInField = 500; // Pseudo
    protected static Dictionary<string, List<GameObject>> entityInstances = new Dictionary<string, List<GameObject>>();

    protected virtual void PrefabsInit() { }


    // =========================================  PROCESS EVALUATOR =========================================
    protected bool ProcessEvaluator(float instanceID, float time = 1)
    {
        if (!curProcess.Contains(instanceID))
        {
            curProcess.Add(instanceID);

            StartCoroutine(ClearID(instanceID, time));
            return true;
        }
        else
        {
            return false;
        }
    }

    protected IEnumerator ClearID(float instanceID, float time = 1)
    {
        yield return new WaitForSeconds(time);
        curProcess.Remove(instanceID);
    }




    // =========================================  INSTANTIATION =========================================

    protected void GameMechanicsPropInit()
    {
        systemGameObject = GameObject.Find("System");
        gameMechanics = systemGameObject.GetComponent<GameMechanics>();
        lvlProp = gameMechanics.curLvlProp;

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




    // ========================================= LAYER METHODS =========================================
    protected static void LayersJoin(ref int numOfLayers, ref List<int> layerInInt, ref LayerMask mainLayer)
    {
        string[] layerNames = new string[numOfLayers];
        for (int i = 0; i < numOfLayers; i++)
        {
            layerNames[i] = LayerMask.LayerToName(layerInInt[i]);
        }

        mainLayer = LayerMask.GetMask(layerNames);
    }

    protected static LayerMask LayersJoin(int numOfLayers, List<int> layerInInt)
    {
        if (numOfLayers != layerInInt.Count) { numOfLayers = layerInInt.Count; }

        string[] layerNames = new string[numOfLayers];
        for (int i = 0; i < numOfLayers; i++)
        {
            layerNames[i] = LayerMask.LayerToName(layerInInt[i]);
        }

        return LayerMask.GetMask(layerNames);
    }




    // =========================================  ITEM DROPS PROPERTIES =========================================
    [Header("ITEM DROP SETTINGS", order = 0)]
    [SerializeField] protected bool doDrop = false;
    [SerializeField] protected int dropChance = 1;
    [SerializeField] protected List<GameObject> itemDrops = new List<GameObject>();


    protected GameObject Drop(int dropChance, Vector2 dropPosition, GameObject itemG = null, Transform parentT = null)
    {
        if (doDrop && ChanceRandomizer(dropChance))
        {
            if (itemG == null && itemDrops.Count != 0) itemG = itemDrops[Random.Range(0, itemDrops.Count)];
            if (parentT == null) parentT = GameObject.Find("Drops").transform;

            if (itemG != null)
            {
                GameObject newDrop = (GameObject)Instantiate(itemG, new Vector3(transform.position.x + dropPosition.x, transform.position.y + dropPosition.y, 0), Quaternion.identity, parentT);
                return newDrop;
            }
        }

        return null;
    }

    protected virtual void itemDropsInit() { }
}
