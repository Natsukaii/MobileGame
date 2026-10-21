using System;
using UnityEngine;

public class ColliderEnemi : MonoBehaviour
{
    private Collider2D collider;
    public EnemiCollidetype _type { get; private set; }
    private void Awake()
    {
        collider = GetComponent<Collider2D>();
        _type = EnemiCollidetype.None;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            _type = EnemiCollidetype.Player;
        }
        else if (other.CompareTag("StickerAlly"))
        {
            _type = EnemiCollidetype.StickerAlly;
        }
        else
        {
            _type = EnemiCollidetype.None;
        }
    }
}

public enum EnemiCollidetype
{
    None,
    Player,
    StickerAlly
}