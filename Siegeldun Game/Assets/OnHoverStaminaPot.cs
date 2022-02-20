using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class OnHoverStaminaPot : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public GameObject childText, childTitle;
    void Start()
    {
        childText.SetActive(false);
        childTitle.SetActive(false);
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        childText.SetActive(true);
        childTitle.SetActive(true);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        childText.SetActive(false);
        childTitle.SetActive(false);
    }
}
