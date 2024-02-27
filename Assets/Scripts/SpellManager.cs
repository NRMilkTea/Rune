using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpellManager : MonoBehaviour
{
    private Camera _camera;

    private bool _isDragging;
    private PlayerSpellBase _spellDragged;
    private Vector2 _offset;

    private int spellOnly;

    private void Awake()
    {
        _camera = Camera.main;
        _isDragging = false;
        spellOnly = LayerMask.GetMask("Spell");
    }

    private void Update()
    {
        Vector2 mousePosition = GetMousePosition();

        // Attempt to grab a spell
        if (Input.GetMouseButtonDown(0))
        {
            RaycastHit2D spellHit = Physics2D.Raycast(mousePosition, Vector2.zero, Mathf.Infinity, spellOnly);
            if (spellHit.collider != null)
            {
                _spellDragged = spellHit.collider.gameObject.GetComponent<Tile>().spell;
                _isDragging = true;
                _offset = mousePosition - (Vector2)_spellDragged.transform.position;

                // Lift dragged spell up so it doesn't get obstructed
                SetSpellTileSortingLayer(_spellDragged, SortingLayer.NameToID("SpellDragged"));
            }
        }
        // During spell Dragging
        if (_isDragging)
        {
            _spellDragged.transform.position = mousePosition - _offset;
        }

        // Attempt to release the dragged spell
        if (Input.GetMouseButtonUp(0))
        {
            if (_isDragging)
            {
                // Put the spell down
                SetSpellTileSortingLayer(_spellDragged, SortingLayer.NameToID("Default"));

                _spellDragged = null;
                _isDragging = false;
            }
        }
    }

    private void SetSpellTileSortingLayer(PlayerSpellBase spellDragged, int sortingLayer)
    {
        foreach (var tile in  spellDragged.tiles)
        {
            tile.GetComponent<SpriteRenderer>().sortingLayerID = sortingLayer;
        }
    }

    private Vector2 GetMousePosition() => _camera.ScreenToWorldPoint(Input.mousePosition);
}
