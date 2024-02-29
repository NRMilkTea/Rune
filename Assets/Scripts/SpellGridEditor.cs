using JetBrains.Annotations;
using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class SpellGridEditor : MonoBehaviour
{
    private Camera _camera;
    [SerializeField] private GameObject _tileSlotHolder;
    [SerializeField] private GameObject _tileSlotPrefab;

    [SerializeField] private GameObject _inventorySlotPrefab;
    [SerializeField] private GameObject _inventoryObject;

    [SerializeField] private GameObject _playerSpellPrefab;
    [SerializeField] private GameObject _tilePrefab;

    // Define the size of the grid
    private const int
        _tileSlotGridXMax = 9,
        _tileSlotGridYMax = 9;
    private TileSlot[,] _tileSlotGrid;

    private const int
        _inventoryColumns = 6,
        _inventoryRows = 2;
    private const float _inventorySize = 1.5f;

    private bool _isDragging;
    private PlayerSpellBase _spellDragged;
    private Vector2 _spellDraggedOriginalPosition;
    private Vector2 _offsetMouseToSpell;

    private void Awake()
    {
        _camera = Camera.main;
        _tileSlotGrid = new TileSlot[_tileSlotGridXMax, _tileSlotGridYMax];
        _isDragging = false;
    }

    private void Start()
    {
        // Generate the grid
        for (int i = 0; i < _tileSlotGridXMax; i++)
        {
            for (int j = 0; j < _tileSlotGridYMax; j++)
            {
                var newTileSlotObject = Instantiate(_tileSlotPrefab, _tileSlotHolder.transform);
                var newTileSlot = newTileSlotObject.GetComponent<TileSlot>();

                newTileSlot.data.coordinate = new Vector2Int(i, j);
                newTileSlot.transform.localPosition = (Vector2)newTileSlot.data.coordinate;
                newTileSlot.Reload();

                _tileSlotGrid[i, j] = newTileSlot;
            }
        }
        // Generate the inventory
        for (int c = 0; c < _inventoryColumns; c++)
        {
            for (int r = 0; r < _inventoryRows; r++)
            {
                var newInventorySlot = Instantiate(_inventorySlotPrefab, _inventoryObject.transform);

                newInventorySlot.transform.localPosition = new Vector2(r * _inventorySize, c * _inventorySize);
            }
        }
    }

    public enum ClickedOn
    {
        None, InventorySlot, SpellTile
    }

    private void Update()
    {
        Vector2 mousePosition = GetMousePosition();

        // Attempt to grab a spell
        if (Input.GetMouseButtonDown(0))
        {
            // Detect the clicked object type
            ClickedOn mouseHit = ClickedOn.None; // default to none

            RaycastHit2D hit = Physics2D.Raycast(mousePosition, Vector2.zero, Mathf.Infinity, LayerMask.GetMask("Inventory Slot"));
            if (hit.collider != null)
            {
                if (hit.collider.gameObject.GetComponent<InventorySlot>() != null) mouseHit = ClickedOn.InventorySlot;
            }
            if (mouseHit == ClickedOn.None)
            {
                hit = Physics2D.Raycast(mousePosition, Vector2.zero, Mathf.Infinity, LayerMask.GetMask("Spell"));
                if (hit.collider != null)
                {
                    if (hit.collider.gameObject.GetComponent<Tile>() != null) mouseHit = ClickedOn.SpellTile;
                }
            }

            if (mouseHit == ClickedOn.InventorySlot)
            {
                InventorySlot inventorySlot = hit.collider.gameObject.GetComponent<InventorySlot>();
                if (inventorySlot.spell != null)
                {
                    _spellDragged = inventorySlot.spell;
                    _isDragging = true;
                    _spellDraggedOriginalPosition = (Vector2)_spellDragged.transform.position;
                    _offsetMouseToSpell = _spellDragged.centerPosition;

                    // Lift dragged spell up so it doesn't get obstructed
                    DragSpell(_spellDragged);

                    inventorySlot.spell = null;
                }
            }
            // if not clicked on an inventory slot, check if it is clicked on a spell
            if (mouseHit == ClickedOn.SpellTile)
            {
                _spellDragged = hit.collider.gameObject.GetComponent<Tile>().spell;
                _isDragging = true;
                _spellDraggedOriginalPosition = (Vector2)_spellDragged.transform.position;
                _offsetMouseToSpell = mousePosition - _spellDraggedOriginalPosition;

                // Lift dragged spell up so it doesn't get obstructed
                DragSpell(_spellDragged);
            }
        }
        // During spell Dragging
        if (_isDragging)
        {
            _spellDragged.transform.position = mousePosition - _offsetMouseToSpell;
            // Fire a raycast from the dragged spell pivot (the first tile) and catch the result

            
        }

        // Attempt to release the dragged spell
        if (Input.GetMouseButtonUp(0))
        {
            if (_isDragging)
            { 
                if (CanPlaceSpellOnInventorySlot())
                {
                    PutIntoInventorySlot();
                }
                else if (CanPlaceSpellOnTileSlotGrid())
                {
                    // Adjust the position of spell so that it sits on the hovered tile slot
                    AdjustToTileSlotGrid();
                }
                else
                {
                    // Put the spell back to its original position
                    _spellDragged.transform.position = _spellDraggedOriginalPosition;
                }
                // Put the spell down
                UndragSpell(_spellDragged);

                _spellDragged = null;
                _isDragging = false;
            }
        }
    }

    private void PutIntoInventorySlot()
    {
        RaycastHit2D hit = Physics2D.Raycast(GetMousePosition(), Vector2.zero, Mathf.Infinity, LayerMask.GetMask("Inventory Slot"));
        InventorySlot inventorySlot = hit.collider.gameObject.GetComponent<InventorySlot>();

        inventorySlot.spell = _spellDragged;
        Vector2 offsetSpellToSlot = -_spellDragged.centerPosition;
        _spellDragged.transform.position = (Vector2)inventorySlot.transform.position + offsetSpellToSlot;
    }
    private bool CanPlaceSpellOnInventorySlot()
    {
        // Fire a raycast to find an inventory slot
        RaycastHit2D hit = Physics2D.Raycast(GetMousePosition(), Vector2.zero, Mathf.Infinity, LayerMask.GetMask("Inventory Slot"));
        if (hit.collider == null) return false;
        if (hit.collider.gameObject.GetComponent<InventorySlot>().spell != null) return false;
        else return true;
    }

    private void AdjustToTileSlotGrid()
    {
        // Only happens if dragged spell can be placed
        Vector2 spellPivotPosition = _spellDragged.tiles[0].transform.position;
        RaycastHit2D hit = Physics2D.Raycast(spellPivotPosition, Vector2.zero, Mathf.Infinity, LayerMask.GetMask("Tile Slot"));
        TileSlot _tileSlotHovered = hit.collider.gameObject.GetComponent<TileSlot>();

        Vector2 spellOffset = -_spellDragged.tiles[0].transform.localPosition;
        _spellDragged.transform.position = (Vector2)_tileSlotHovered.transform.position + spellOffset;
    }
    private bool CanPlaceSpellOnTileSlotGrid()
    {
        foreach (Tile tile in _spellDragged.tiles)
        {
            // Fire a raycast to layer Spell and layer Tile slot
            RaycastHit2D hit = Physics2D.Raycast(tile.transform.position, Vector2.zero, Mathf.Infinity, LayerMask.GetMask("Tile Slot", "Spell"));

            if (hit.collider == null) return false;
            if (hit.collider.gameObject.GetComponent<TileSlot>() == null) return false; // NOT hits on tile slot
        }
        return true;
    }

    private void DragSpell(PlayerSpellBase spell)
    {
        SetSpellTileLayer(_spellDragged, LayerMask.NameToLayer("Dragged Spell"));
        SetSpellTileSortingLayer(_spellDragged, SortingLayer.NameToID("Dragged Spell"));
        SetSpellPositionZ(_spellDragged, -1);
    }
    private void UndragSpell(PlayerSpellBase spell)
    {
        SetSpellTileLayer(_spellDragged, LayerMask.NameToLayer("Spell"));
        SetSpellTileSortingLayer(_spellDragged, SortingLayer.NameToID("Default"));
        SetSpellPositionZ(_spellDragged, 0);
    }

    private void SetSpellTileSortingLayer(PlayerSpellBase spellDragged, int sortingLayer)
    {
        foreach (var tile in spellDragged.tiles)
        {
            tile.GetComponent<SpriteRenderer>().sortingLayerID = sortingLayer;
        }
    }
    private void SetSpellTileLayer(PlayerSpellBase spellDragged, int Layer)
    {
        foreach (var tile in spellDragged.tiles)
        {
            tile.gameObject.layer = Layer;
        }
    }

    private void SetSpellPositionZ(PlayerSpellBase spell, int z)
    {
        Vector3 position = spell.transform.position;
        position.z = z;
        spell.transform.position = position;
    }

    private Vector2 GetMousePosition() => _camera.ScreenToWorldPoint(Input.mousePosition);
}
