using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TileSlot : MonoBehaviour
{
    public TileSlotData data;
}

[Serializable]
public class TileSlotData
{
    public int x, y;
    // public TileType(Enum)
}