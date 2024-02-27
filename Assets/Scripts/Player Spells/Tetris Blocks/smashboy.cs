using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class smashboy : PlayerSpellBase
{
    public override List<TileData> DefaultTileLayout
    {
        get => new List<TileData>
        {
            new TileData{coordinate = new Vector2Int(0, 0)},
            new TileData{coordinate = new Vector2Int(1, 0)},
            new TileData{coordinate = new Vector2Int(0, 1)},
            new TileData{coordinate = new Vector2Int(1, 1)},
        };
        set { }
    }
}
