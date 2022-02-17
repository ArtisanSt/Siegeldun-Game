using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class OnHoverDifficultyButton : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    [SerializeField] DifficultiesTab diffTab;
    [SerializeField] string difficulty;

    public void OnPointerEnter(PointerEventData eventData)
    {
        diffTab.ShowInfo(difficulty);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        diffTab.HideInfo();
    }
}