using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DifficultiesTab : MonoBehaviour
{
    [SerializeField] public GameObject textDiffInfo;

    [System.Serializable]
    public class DifficultyProp
    {
        public string diffTitle;
        [TextArea(3, 10)]
        public string diffInfo;
    }

    [SerializeField] public List<DifficultyProp> difficulty;

    public void ShowInfo(string diffTitle)
    {
        for (int i=0; i < difficulty.Count; i++)
        {
            if (diffTitle == difficulty[i].diffTitle)
            {
                textDiffInfo.SetActive(true);
                textDiffInfo.GetComponent<TMPro.TextMeshProUGUI>().text = difficulty[i].diffInfo;
                break;
            }
        }
    }

    public void HideInfo()
    {
        textDiffInfo.SetActive(false);
    }
}
