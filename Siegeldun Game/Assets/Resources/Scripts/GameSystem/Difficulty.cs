using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

[System.Serializable]
public class Difficulty
{
    public string name;
    public string description;
    public int multiplier;

    public int bulwarkLives;
    public int shrineLives;

    public string ToJson() => JsonUtility.ToJson(this, true);
    public void FromJson(string json)
    {
        Difficulty difficulty = JsonUtility.FromJson<Difficulty>(json);
        name = difficulty.name;
        description = difficulty.description;
        multiplier = difficulty.multiplier;
        bulwarkLives = difficulty.bulwarkLives;
        shrineLives = difficulty.shrineLives;
    }
}
