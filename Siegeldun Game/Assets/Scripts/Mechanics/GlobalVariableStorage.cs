using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class GlobalVariableStorage
{
    // ================================================ GameData ================================================
    // Achievements
    public static bool[] achievements = new bool[4] { false, false, false, false };


    // Unlocks
    public static bool[] hostiles = new bool[4] { false, false, false, false };
    public static bool[] potions = new bool[2] { false, false };
    public static bool[] weapons = new bool[1] { false };


    public enum Difficulty { Normal, Hard, Insane }
    public static DifficultyProperties curDifficulty = new DifficultyProperties();

    public static int curAct = 0;
    public static int curLvl = 1;
    // 0-0 Testing Grounds
    // 0-1 Tutorial Grounds
    // 1-1 Act 1 Level 1...

    public static List<int> numberOfLevelsPerAct;

    public static Difficulty difficulty;
    public static int statsMultiplier;
    public static int curBulwarkLives = 0, curEShrineLives = 0;
    public static int maxBulwarkLives = -1, maxEShrineLives = -1; // -1 inf , can't be 0
}
