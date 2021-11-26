using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Entity Parent Class
public class Entity
{
    public string name;
    public int hp;
    public int damage;
    public float attackSpeed;
    public string[] statusEffect;

    public static int entityCount;
    public static int difficulty; // 1 = Normal, 2 = Hard, 3 = Insane


    public Entity(int diff = 1)
    {
        difficulty = diff;
    }
}
