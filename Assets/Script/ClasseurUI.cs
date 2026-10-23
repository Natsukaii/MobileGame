using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ClasseurUI : MonoBehaviour
{
    [Header("Sticker List")]
    [SerializeField] private GameObject stickerSlotPrefab;
    [SerializeField] private Transform stickerListContent;
    [SerializeField] private GridLayoutGroup gridLayout;

    [Header("Deck")]
    [SerializeField] private GameObject deckSlotPrefab;
    [SerializeField] private Transform deckContainer;

    private List<StickerSlot> stickerSlots = new List<StickerSlot>();
    private List<DeckSlot> deckSlots = new List<DeckSlot>();
    private const int MAX_DECK_SIZE = 5;

    private void Start()
    {
        SetupDeckSlots();
        PopulateStickerList();
        UpdateDeckDisplay();
    }

    private void SetupDeckSlots()
    {
        if (deckSlotPrefab == null || deckContainer == null)
        {
            Debug.LogError("DeckSlotPrefab ou DeckContainer n'est pas assign\u00e9!");
            return;
        }

        for (int i = 0; i < MAX_DECK_SIZE; i++)
        {
            GameObject slotObj = Instantiate(deckSlotPrefab, deckContainer);
            DeckSlot slot = slotObj.GetComponent<DeckSlot>();
            if (slot != null)
            {
                slot.Setup(i, this);
                deckSlots.Add(slot);
            }
        }
    }

    private void PopulateStickerList()
    {
        if (StickerInventory.Instance == null)
        {
            Debug.LogError("StickerInventory.Instance est null!");
            return;
        }

        if (stickerSlotPrefab == null || stickerListContent == null)
        {
            Debug.LogError("StickerSlotPrefab ou StickerListContent n'est pas assign\u00e9!");
            return;
        }

        // Clear existing slots
        foreach (var slot in stickerSlots)
        {
            if (slot != null)
                Destroy(slot.gameObject);
        }
        stickerSlots.Clear();

        // Configure grid for 2 columns
        if (gridLayout != null)
        {
            gridLayout.constraint = GridLayoutGroup.Constraint.FixedColumnCount;
            gridLayout.constraintCount = 2;
        }

        // Create slots for all available stickers
        List<StickerData> stickers = StickerInventory.Instance.GetAvailableStickers();
        foreach (StickerData sticker in stickers)
        {
            GameObject slotObj = Instantiate(stickerSlotPrefab, stickerListContent);
            StickerSlot slot = slotObj.GetComponent<StickerSlot>();
            if (slot != null)
            {
                slot.Setup(sticker, this);
                stickerSlots.Add(slot);
            }
        }
    }

    public void OnStickerClicked(StickerData sticker)
    {
        if (StickerInventory.Instance == null || sticker == null)
            return;

        if (StickerInventory.Instance.IsStickerInDeck(sticker.id))
        {
            // Remove from deck
            RemoveStickerFromDeck(sticker);
        }
        else
        {
            // Add to deck
            if (StickerInventory.Instance.AddStickerToDeck(sticker.id))
            {
                UpdateDeckDisplay();
                UpdateAllStickerSlots();
            }
        }
    }

    public void RemoveStickerFromDeck(StickerData sticker)
    {
        if (StickerInventory.Instance == null || sticker == null)
            return;

        StickerInventory.Instance.RemoveStickerFromDeck(sticker.id);
        UpdateDeckDisplay();
        UpdateAllStickerSlots();
    }

    private void UpdateDeckDisplay()
    {
        if (StickerInventory.Instance == null)
            return;

        List<StickerData> deckStickers = StickerInventory.Instance.GetDeckStickers();

        // Update deck slots
        for (int i = 0; i < deckSlots.Count; i++)
        {
            if (i < deckStickers.Count)
            {
                deckSlots[i].SetSticker(deckStickers[i]);
            }
            else
            {
                deckSlots[i].ClearSlot();
            }
        }
    }

    private void UpdateAllStickerSlots()
    {
        foreach (var slot in stickerSlots)
        {
            if (slot != null)
                slot.UpdateSelectedIndicator();
        }
    }
}
