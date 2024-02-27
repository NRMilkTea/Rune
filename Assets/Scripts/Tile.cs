using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEditor.UIElements;
using UnityEngine.UIElements;

public class Tile : MonoBehaviour
{
    // Tile information
    public PlayerSpellBase spell; // the spell to whom this tile is attached
    public TileData data;
    [SerializeField] private Sprite _sprite;

    public void Draw()
    {
        SpriteRenderer renderer = GetComponent<SpriteRenderer>();
        renderer.sprite = _sprite;
        renderer.color = spell.color;
    }
}

[Serializable]
public class TileData
{
    public int x, y;
    // public TileType(Enum)
}