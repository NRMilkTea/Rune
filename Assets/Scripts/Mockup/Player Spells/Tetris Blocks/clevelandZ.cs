using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Mockup
{
    public class clevelandZ : PlayerSpellBase
    {
        public override List<TileData> DefaultTileLayout
        {
            get => new List<TileData>
        {
            new TileData{coordinate = new Vector2Int(1, 0)},
            new TileData{coordinate = new Vector2Int(2, 0)},
            new TileData{coordinate = new Vector2Int(0, 1)},
            new TileData{coordinate = new Vector2Int(1, 1)},
        };
            set { }
        }
    }
}