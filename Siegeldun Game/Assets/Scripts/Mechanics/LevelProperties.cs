using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelProperties
{
    public int gameDifficulty { get; protected set; }
    public int actNumber { get; protected set; }
    public int levelNumber { get; protected set; }

    public int statsMultiplier { get; protected set; }
    public int essenceShrine;
    public int bulwark;

    // difficulty : {statsMultiplier, essenceShrineMax, bulwarkMax}
    private Dictionary<int, int[]> statsLevelValue = new Dictionary<int, int[]>()
    {
        [0] = new int[3] { 1, -1, -1 },
        [1] = new int[3] { 2, -1, -1 },
        [2] = new int[3] { 4, 3, 1 },
    };

    private int[] numberOfLevelsPerAct = new int[] { 1, 10, 15, 20 };

    public LevelProperties(int gameDifficulty = 0, int actNumber = 0, int levelNumber = 0)
    {
        this.gameDifficulty = gameDifficulty;
        this.actNumber = actNumber;
        this.levelNumber = levelNumber;

        this.statsMultiplier = statsLevelValue[gameDifficulty][0];
        this.essenceShrine = statsLevelValue[gameDifficulty][1];
        this.bulwark = statsLevelValue[gameDifficulty][2];
    }

    public void CloneProperties(LevelProperties origProp)
    {
        this.gameDifficulty = origProp.gameDifficulty;
        this.actNumber = origProp.actNumber;
        this.levelNumber = origProp.levelNumber;

        this.statsMultiplier = origProp.statsMultiplier;
        this.essenceShrine = origProp.essenceShrine;
        this.bulwark = origProp.bulwark;
    }

    public bool LevelFinished() // returns true if the final level and act is finished
    {
        levelNumber++;
        if (levelNumber == numberOfLevelsPerAct[actNumber])
        {
            actNumber++;
            if (actNumber == numberOfLevelsPerAct.Length)
            {
                return true;
            }
            else
            {
                levelNumber = 0;
            }
        }
        return false;
    }

    public int PlayerDied() // returns 0 if the player still has lives left, 1 if the player used all of Bulwark lives, 2 if no EssenceShrine lives left
    {
        // If Bulwark is Infinite
        if (bulwark == -1)
        {
            return 0;
        }
        // If Bulwark is not Infinite
        else
        {
            bulwark--;
            if (bulwark > 0)
            {
                return 0;
            }
            else
            {
                // Essence Shrine is Infinite
                // Bulwark lives will refill but will be resurrected at last Essence Shrine
                if (essenceShrine == -1)
                {
                    bulwark = statsLevelValue[gameDifficulty][2];
                    return 1;
                }
                // Essence Shrine is not Infinite
                else
                {
                    essenceShrine--;
                    if (essenceShrine > 0)
                    {
                        return 1;
                    }
                    else
                    {
                        return 2;
                    }
                }
            }
        }
    }
}
