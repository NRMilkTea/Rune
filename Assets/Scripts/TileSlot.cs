using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TileSlot : MonoBehaviour
{
    public TileSlotData data;
    [SerializeField] private Sprite _slotSprite;
    [SerializeField] private Sprite _barrierSprite;

    public void Draw()
    {
        SpriteRenderer spriteRenderer = GetComponent<SpriteRenderer>();
        switch(data.type)
        {
            case TileSlotType.Slot: spriteRenderer.sprite = _slotSprite; break;
            case TileSlotType.Barrier: spriteRenderer.sprite = _barrierSprite; break;
        }
    }
}

[Serializable]
public class TileSlotData
{
    public Vector2Int coordinate;
    public TileSlotType type;
    // public TileType(Enum)
}

public enum TileSlotType
{
    Slot,
    Barrier
}