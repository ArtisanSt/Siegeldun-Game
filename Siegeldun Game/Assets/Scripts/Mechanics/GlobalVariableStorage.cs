using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class GlobalVariableStorage
{
    public enum Difficulty { Normal, Hard, Insane }
    public static DifficultyProperties curDifficulty = new DifficultyProperties();

    public static int curAct = 0;
    public static int curLvl = 1;
    // 0-0 Testing Grounds
    // 0-1 Tutorial Grounds
    // 1-1 Act 1 Level 1...

    public static List<int> numberOfLevelsPerAct;

    public static int curBulwarkLives = 0, curEShrineLives = 0;
    public static int maxBulwarkLives = -1, maxEShrineLives = -1; // -1 inf , can't be 0
}
