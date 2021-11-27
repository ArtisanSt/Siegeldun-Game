using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Entity Parent Class
public class Entity : MonoBehaviour
{      
    public string entityName;
    public int hp;
    public int damage;
    public float attackSpeed;
    public string[] statusEffect;

    public int difficulty = 1;

    public void movement()
    {

    }
}
