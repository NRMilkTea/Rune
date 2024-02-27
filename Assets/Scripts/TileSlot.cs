using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TileSlot : MonoBehaviour
{
    public TileSlotData data;
    [SerializeField] private Sprite _sprite;

    public void Draw()
    {
        GetComponent<SpriteRenderer>().sprite = _sprite;
    }
}

[Serializable]
public class TileSlotData
{
    public int x, y;
    // public TileType(Enum)
}