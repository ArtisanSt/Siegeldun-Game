using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IItemDrops
{
    bool doDrop { get; }

    int dropChance { get; }
    List<GameObject> itemDrops { get; }


    GameObject Drop();
}
