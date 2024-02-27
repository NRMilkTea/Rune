using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class PlayerSpellBase : MonoBehaviour
{
    // Prefabs
    [SerializeField] private GameObject _tilePrefab;

    // Spell information
    // The spell id is identical to the inheriting class name
    public string Id { get => GetType().Name; }
    public string displayName;
    public string description;
    
    // In case of runtime spell tile changes, this will only be used in initialization
    public abstract List<TileData> DefaultTileLayout { get; set; }

    [HideInInspector]
    public List<Tile> tiles;
    public Color color;

    private void Awake()
    {
        Init();
    }
    private void Init()
    {
        // Instantiate the default tiles for this spell
        foreach (var tileData in DefaultTileLayout)
        {
            var newTileObject = Instantiate(_tilePrefab, this.transform);
            var newTile = newTileObject.GetComponent<Tile>();

            newTile.spell = this;
            newTile.data = tileData;
            newTile.transform.localPosition = new Vector2(tileData.x, tileData.y);

            newTile.Draw();
            tiles.Add(newTile);
        }
    }
}