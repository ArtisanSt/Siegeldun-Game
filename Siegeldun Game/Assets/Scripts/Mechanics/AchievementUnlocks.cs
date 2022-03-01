using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AchievementUnlocks : MonoBehaviour
{
    [SerializeField] private string objectName;
    private enum ObjectClassification { Achievements, Hostiles, Potions, Weapons }
    [SerializeField] private ObjectClassification objectClassification;

    private bool InList(string[] targets)
    {
        bool output = false;
        foreach (string target in targets)
        {
            output = objectName == target;
            if (output) break;
        }
        return output;
    }

    private int FindAchievement()
    {
        int output = -1;
        if (InList(new string[] { "Player", "Goblin", "HealthPotion", "Stick" }))
        {
            output = 0;
        }
        else if (InList(new string[] { "Ending", "Wolf", "StaminaPotion", "Katana" }))
        {
            output = 1;
        }
        else if (InList(new string[] { "Statue", "GoblinRider", "Sword" }))
        {
            output = 2;
        }
        else if (InList(new string[] { "GatePortal", "Gobta" }))
        {
            output = 3;
        }
        return output;
    }

    public void ChangeState()
    {
        int idx = FindAchievement();
        if (idx == -1) return;

        if (objectClassification == ObjectClassification.Achievements && GlobalVariableStorage.achievements[idx] == true) return;
        if (objectClassification == ObjectClassification.Hostiles && GlobalVariableStorage.hostiles[idx] == true) return;
        if (objectClassification == ObjectClassification.Potions && GlobalVariableStorage.potions[idx] == true) return;
        if (objectClassification == ObjectClassification.Weapons && GlobalVariableStorage.weapons[idx] == true) return;

        if (objectClassification == ObjectClassification.Achievements)
        {
            GlobalVariableStorage.achievements[idx] = true;
        }
        else if (objectClassification == ObjectClassification.Hostiles)
        {
            GlobalVariableStorage.hostiles[idx] = true;
        }
        else if (objectClassification == ObjectClassification.Potions)
        {
            GlobalVariableStorage.potions[idx] = true;
        }
        else if (objectClassification == ObjectClassification.Weapons)
        {
            GlobalVariableStorage.weapons[idx] = true;
        }

        SaveAndLoadManager.SaveGameData();
    }
}
