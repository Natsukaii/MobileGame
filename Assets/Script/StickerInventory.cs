using System.Collections.Generic;
using UnityEngine;

public class StickerInventory : MonoBehaviour
{
    public static StickerInventory Instance { get; private set; }

    [SerializeField] private List<StickerData> availableStickers = new List<StickerData>();
    private List<int> playerDeck = new List<int>();
    private const int MAX_DECK_SIZE = 5;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
        DontDestroyOnLoad(gameObject);
    }

    private void Start()
    {
        LoadDeck();
    }

    public List<StickerData> GetAvailableStickers()
    {
        return availableStickers;
    }

    public bool AddStickerToDeck(int stickerId)
    {
        if (playerDeck.Count >= MAX_DECK_SIZE)
        {
            Debug.Log("Le deck est plein!");
            return false;
        }

        if (playerDeck.Contains(stickerId))
        {
            Debug.Log("Ce sticker est d\u00e9j\u00e0 dans le deck!");
            return false;
        }

        playerDeck.Add(stickerId);
        SaveDeck();
        return true;
    }

    public void RemoveStickerFromDeck(int stickerId)
    {
        if (playerDeck.Contains(stickerId))
        {
            playerDeck.Remove(stickerId);
            SaveDeck();
        }
    }

    public bool IsStickerInDeck(int stickerId)
    {
        return playerDeck.Contains(stickerId);
    }

    public List<int> GetPlayerDeck()
    {
        return new List<int>(playerDeck);
    }

    public int GetDeckCount()
    {
        return playerDeck.Count;
    }

    public bool IsDeckFull()
    {
        return playerDeck.Count >= MAX_DECK_SIZE;
    }

    public List<StickerData> GetDeckStickers()
    {
        List<StickerData> deckStickers = new List<StickerData>();
        foreach (int id in playerDeck)
        {
            StickerData sticker = availableStickers.Find(s => s.id == id);
            if (sticker != null)
            {
                deckStickers.Add(sticker);
            }
        }
        return deckStickers;
    }

    private void SaveDeck()
    {
        string deckJson = string.Join(",", playerDeck);
        PlayerPrefs.SetString("PlayerDeck", deckJson);
        PlayerPrefs.Save();
    }

    private void LoadDeck()
    {
        string deckJson = PlayerPrefs.GetString("PlayerDeck", "");
        playerDeck.Clear();

        if (!string.IsNullOrEmpty(deckJson))
        {
            string[] ids = deckJson.Split(',');
            foreach (string id in ids)
            {
                if (int.TryParse(id, out int stickerId))
                {
                    playerDeck.Add(stickerId);
                }
            }
        }
    }
}
