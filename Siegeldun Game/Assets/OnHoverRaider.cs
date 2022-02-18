using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class OnHoverRaider : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public GameObject childText, childTitle, childImage;
    void Start()
    {
        childText.SetActive(false);
        childTitle.SetActive(false);
        childImage.SetActive(false);
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        childText.SetActive(true);
        childTitle.SetActive(true);
        childImage.SetActive(true);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        childText.SetActive(false);
        childTitle.SetActive(false);
        childImage.SetActive(false);
    }
}
