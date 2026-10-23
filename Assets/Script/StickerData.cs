using UnityEngine;

[CreateAssetMenu(fileName = "StickerData", menuName = "MobileGame/StickerData")]
public class StickerData : ScriptableObject
{
    public int id;
    public string stickerName;
    public Sprite icon;
    public GameObject prefab;
    public string description;
}
