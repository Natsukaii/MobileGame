using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.EventSystems;

public class StickerSlot : MonoBehaviour, IPointerClickHandler
{
    [SerializeField] private Image icon;
    [SerializeField] private TMP_Text nameText;
    [SerializeField] private GameObject selectedIndicator;

    private StickerData stickerData;
    private ClasseurUI classeurUI;

    public void Setup(StickerData data, ClasseurUI ui)
    {
        stickerData = data;
        classeurUI = ui;

        if (icon != null && data.icon != null)
            icon.sprite = data.icon;

        if (nameText != null)
            nameText.text = data.stickerName;

        UpdateSelectedIndicator();
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if (classeurUI != null && stickerData != null)
        {
            classeurUI.OnStickerClicked(stickerData);
        }
    }

    public void UpdateSelectedIndicator()
    {
        if (selectedIndicator != null && stickerData != null)
        {
            bool isInDeck = StickerInventory.Instance != null &&
                           StickerInventory.Instance.IsStickerInDeck(stickerData.id);
            selectedIndicator.SetActive(isInDeck);
        }
    }
}
