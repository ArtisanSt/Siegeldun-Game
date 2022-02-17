using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ExtrasTab : MonoBehaviour
{
    public static int curState = 0;
    [SerializeField] public Image extrasBG;
    [SerializeField] public List<Sprite> extrasSprites;
    [SerializeField] public List<GameObject> extrasbuttons;

    void Awake()
    {
        SetState(curState);
    }

    public void SetState(int newState)
    {
        curState = newState;
        extrasBG.sprite = extrasSprites[curState];
        for (int i = 0; i < extrasbuttons.Count; i++)
        {
            extrasbuttons[i].SetActive(curState != i);
        }
        Debug.Log(curState);
    }
}
