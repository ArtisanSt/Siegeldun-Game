using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class BaseObject : Root
{
    // =========================================  ITEM DROPS PROPERTIES =========================================
    [Header("ITEM DROP SETTINGS", order = 0)]
    [SerializeField] protected bool doDrop = false;
    [SerializeField] protected int dropChance = 1;
    [SerializeField] protected float onDestroyDropDelay = 0;
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
