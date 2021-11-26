using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Aerean : Entity
{
    public string type;

    public static int aereanCount;

    public Aerean(int aereanType = -1)
    {
        int[] types = new int[2] { 0, 1 };

        if (System.Array.IndexOf(types, aereanType) != -1) //If the instane has no specific type, randomization occurs
        {
            aereanType = Random.Range(0, 2);
        }

        if (aereanType == 0)
        {
            type = "melee";
            meleeType();
        }
        else
        {
            type = "range";
            rangeType();
        }

        name = System.String.Format("Aerean{0}{1}_{2}", type, entityCount, aereanCount);
        entityCount += 1;
        aereanCount += 1;
        Debug.Log(type);
    }

    private void meleeType()
    {
        hp = 10 * difficulty;
        damage = 4 * difficulty;
        attackSpeed = System.Convert.ToSingle((500 / 1000) * difficulty);
    }

    private void rangeType()
    {

    }

    public void attack()
    {

    }

    public void aereanDeath()
    {
        entityCount -= 1;
        aereanCount -= 1;
    }

}
