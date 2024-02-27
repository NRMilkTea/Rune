using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class spellExample : PlayerSpellBase
{
    public override List<TileData> DefaultTileLayout 
    {
        get => new List<TileData>
        {
            new TileData(){x = 0, y = 0},
            new TileData(){x = 1, y = 0},
            new TileData(){x = 2, y = 0},
            new TileData(){x = 0, y = 1},
        };
        set { }
    }
}
