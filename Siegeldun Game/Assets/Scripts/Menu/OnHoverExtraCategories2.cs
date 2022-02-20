using UnityEngine.EventSystems;
using UnityEngine;
using UnityEngine.UI;

public class OnHoverExtraCategories2 : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    [Header("Content Reference Settings")]
    [SerializeField] private GameObject contents;
    [SerializeField] private TMPro.TextMeshProUGUI nameText;
    [SerializeField] private Image contentImg;
    [SerializeField] private TMPro.TextMeshProUGUI contentText;

    public enum ContentType { Enemy, Item }
    [Header("Entity Properties")]
    [SerializeField] private ContentType contentType;
    [SerializeField] private string nameToShow;
    [SerializeField] private Sprite imgToShow;
    [TextArea(3, 10)]
    [SerializeField] private string infoToShow;


    public void OnPointerEnter(PointerEventData eventData)
    {
        contents.SetActive(true);
        nameText.text = nameToShow;
        contentImg.sprite = imgToShow;
        contentText.text = infoToShow;
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        nameText.text = "";
        contentText.text = "";
        contents.SetActive(false);
    }
}
