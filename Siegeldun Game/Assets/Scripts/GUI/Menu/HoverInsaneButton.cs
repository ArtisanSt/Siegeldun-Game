using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class HoverInsaneButton : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public GameObject childText;
    void Start()
    {
        childText.SetActive(false);
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        childText.SetActive(true);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        childText.SetActive(false);
    }
}
