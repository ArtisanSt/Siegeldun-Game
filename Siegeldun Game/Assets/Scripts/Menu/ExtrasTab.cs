using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ExtrasTab : MonoBehaviour
{
    public static int curState = 0;
    [SerializeField] public Image extrasBG;
    [SerializeField] public List<GameObject> extrasButtons;
    [SerializeField] public List<GameObject> extrasCategories;

    [Header("Category Settings")]
    [SerializeField] public List<GameObject> category1Achievements;
    [SerializeField] public List<GameObject> category2Hostiles;
    [SerializeField] public List<GameObject> category2Potions;
    [SerializeField] public List<GameObject> category2Weapons;

    void Awake()
    {
        SetState(curState);
    }

    public void SetState(int newState)
    {
        curState = newState;
        for (int i = 0; i < extrasButtons.Count; i++)
        {
            extrasButtons[i].SetActive(curState != i);
            extrasCategories[i].SetActive(curState == i);
        }
        ShowCategoryContents();
    }

    public void ShowCategoryContents()
    {
        switch (curState)
        {
            case 0:
                Category0Content();
                break;

            case 1:
                Category1Content();
                break;

            case 2:
                Category2Content();
                break;
        }
    }

    public void Category0Content()
    {
        for (int i=0; i < GlobalVariableStorage.achievements.Length; i++)
        {
            category1Achievements[i].SetActive(GlobalVariableStorage.achievements[i]);
        }
    }

    public void Category1Content()
    {
        for (int i = 0; i < GlobalVariableStorage.hostiles.Length; i++)
        {
            category2Hostiles[i].SetActive(GlobalVariableStorage.hostiles[i]);
        }

        for (int i = 0; i < GlobalVariableStorage.potions.Length; i++)
        {
            category2Potions[i].SetActive(GlobalVariableStorage.potions[i]);
        }

        for (int i = 0; i < GlobalVariableStorage.weapons.Length; i++)
        {
            category2Weapons[i].SetActive(GlobalVariableStorage.weapons[i]);
        }
    }

    public void Category2Content()
    {

    }
}
