using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.EventSystems;

public class DeckSlot : MonoBehaviour, IPointerClickHandler
{
    [SerializeField] private Image icon;
    [SerializeField] private TMP_Text nameText;
    [SerializeField] private GameObject emptyIndicator;

    private StickerData currentSticker;
    private int slotIndex;
    private ClasseurUI classeurUI;

    public void Setup(int index, ClasseurUI ui)
    {
        slotIndex = index;
        classeurUI = ui;
        ClearSlot();
    }

    public void SetSticker(StickerData data)
    {
        currentSticker = data;

        if (data != null)
        {
            if (icon != null)
            {
                icon.sprite = data.icon;
                icon.enabled = true;
            }

            if (nameText != null)
            {
                nameText.text = data.stickerName;
                nameText.enabled = true;
            }

            if (emptyIndicator != null)
                emptyIndicator.SetActive(false);
        }
        else
        {
            ClearSlot();
        }
    }

    public void ClearSlot()
    {
        currentSticker = null;

        if (icon != null)
            icon.enabled = false;

        if (nameText != null)
            nameText.enabled = false;

        if (emptyIndicator != null)
            emptyIndicator.SetActive(true);
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if (currentSticker != null && classeurUI != null)
        {
            classeurUI.RemoveStickerFromDeck(currentSticker);
        }
    }

    public StickerData GetSticker()
    {
        return currentSticker;
    }
}
