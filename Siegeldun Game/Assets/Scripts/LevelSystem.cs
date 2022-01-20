using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelProperties
{
    private int gameDifficulty;
    private int actNumber;
    private int levelNumber;

    private int statsMultiplier;
    private int essenceShrine; //max life amount (-1: infinite)
    private int bulwark; // max life amount (-1: infinite)

    private Dictionary<int, int[]> statsLevelValue = new Dictionary<int, int[]>() // statsMultiplier, essenceShrineMax, bulwarkMax
    {
        [0] = new int[3] { 1, -1, -1 },
        [1] = new int[3] { 2, -1, -1 },
        [2] = new int[3] { 4, 3, 1 },
    };

    private int[] numberOfLevelsPerAct = new int[] { 1, 10, 15, 20 };

    public LevelProperties(int gameDifficulty, int actNumber = 0, int levelNumber = 0)
    {
        this.gameDifficulty = gameDifficulty;
        this.actNumber = actNumber;
        this.levelNumber = levelNumber;

        this.statsMultiplier = statsLevelValue[gameDifficulty][0];
        this.essenceShrine = statsLevelValue[gameDifficulty][1];
        this.bulwark = statsLevelValue[gameDifficulty][2];
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

    public int UpdateLifeChances(int lifeChange) // returns 0 if the player still has lives left, 1 if the player used all of Bulwark lives, 2 if no EssenceShrine lives left
    {
        if (bulwark == -1)
        {
            return 0;
        }
        else
        {
            bulwark += lifeChange;
            if (bulwark > 0)
            {
                return 0;
            }
            else
            {
                if (essenceShrine == -1)
                {
                    bulwark = statsLevelValue[gameDifficulty][2];
                    return 0;
                }
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


public class LevelSystem : MonoBehaviour
{
    protected Dictionary<string, LevelProperties> levelInitilization = new Dictionary<string, LevelProperties>();
    public LevelProperties lvlProp;

    public int gameDifficulty = 0;
    public int actNumber = 0;
    public int levelNumber = 0;

    // Start is called before the first frame update
    void Start()
    {
        LevelInitialize(gameDifficulty, actNumber, levelNumber);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void LevelInitialize(int gameDifficulty, int actNumber, int levelNumber)
    {
        lvlProp = new LevelProperties(gameDifficulty, actNumber, levelNumber);
    }

    public void UpdateLevelProperties()
    {

    }


}
