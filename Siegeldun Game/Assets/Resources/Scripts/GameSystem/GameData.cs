using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class GameData
{
    public string dataName { get; set; }

    public int act { get; set; }
    public int segment { get; set; }

    public Vector2Int bulwarkLives { get; set; }
    public Vector2Int shrineLives { get; set; }

    public int difficulty { get; set; }

    //public List<Item> lastSavedItems { get; set; }

    // public List<Achievement> achievements;

    /*public GameData()
    {
        bulwarkLives = new Vector2Int(0, 0);
        shrineLives = new Vector2Int(0, 0);

        lastSavedItems = new List<Item>();
    }*/

    /*public GameData Duplicate()
    {
        GameData temp = (GameData)MemberwiseClone();
        temp.lastSavedItems = this.lastSavedItems;
        return temp;
    }*/
}
