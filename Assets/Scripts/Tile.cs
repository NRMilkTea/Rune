using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEditor.UIElements;
using UnityEngine.UIElements;
using System.ComponentModel;

public class Tile : MonoBehaviour
{
    // Tile information
    public PlayerSpellBase spell; // the spell to whom this tile is attached
    public TileData data;
    public TileType tileType;
    [SerializeField] private Sprite _fillUpTileSprite;
    [SerializeField] private Sprite _directionalPassTileSprite;
    [SerializeField] private Sprite _unassignedTileSprite;

    public void Draw()
    {
        SpriteRenderer renderer = GetComponent<SpriteRenderer>();
        switch (tileType)
        {
            case TileType.FillUp:
                renderer.sprite = _fillUpTileSprite;
                break;
            case TileType.DirectionalPass:
                renderer.sprite = _directionalPassTileSprite;
                break;
            case TileType.Unassigned:
                renderer.sprite = _unassignedTileSprite;
                break;
        }
        renderer.color = spell.color;
    }
}

[Serializable]
public class TileData
{
    public Vector2Int coordinate;
    // public TileType(Enum)
}

public enum TileType
{
    Unassigned,
    FillUp, 
    DirectionalPass,

}