using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

namespace Mockup
{
    public abstract class PlayerSpellBase : MonoBehaviour
    {
        // Prefabs
        [SerializeField] private GameObject _tilePrefab;

        // Spell information
        // The spell id is identical to the inheriting class name
        public string Id { get => GetType().Name; }
        public string displayName;
        public string description;

        public abstract List<TileData> DefaultTileLayout { get; set; }

        [HideInInspector]
        public List<Tile> tiles;
        [HideInInspector]
        public Tile pivotTile;
        public Color color;

        public float size;
        public Vector2Int lowerLeftPosition;
        public Vector2 centerPosition;

        private void Awake()
        {
            Init();
            CalculateSpellSize();
        }
        private void Init()
        {
            // Instantiate the default tiles for this spell
            foreach (var tileData in DefaultTileLayout)
            {
                var newTileObject = Instantiate(_tilePrefab, this.transform);
                var newTile = newTileObject.GetComponent<Tile>();

                tiles.Add(newTile);

                newTile.spell = this;
                newTile.data = tileData;
                newTile.transform.localPosition = (Vector2)newTile.data.coordinate;
                newTile.GetComponent<SpriteRenderer>().sortingLayerID = SortingLayer.NameToID("Spell");
            }
            Draw();
        }
        private void Draw()
        {
            foreach (var tile in tiles)
            {
                tile.Draw();
            }
        }
        private void CalculateSpellSize()
        {
            int xMin = tiles[0].data.coordinate.x,
                xMax = tiles[0].data.coordinate.x,
                yMin = tiles[0].data.coordinate.y,
                yMax = tiles[0].data.coordinate.y;

            foreach (var tile in tiles)
            {
                xMin = Mathf.Min(xMin, tile.data.coordinate.x);
                xMax = Mathf.Max(xMax, tile.data.coordinate.x);
                yMin = Mathf.Min(yMin, tile.data.coordinate.y);
                yMax = Mathf.Max(yMax, tile.data.coordinate.y);
            }

            this.size = Mathf.Max(xMax - xMin + 1, yMax - yMin + 1);
            this.lowerLeftPosition = new Vector2Int(xMin, yMin);
            this.centerPosition = new Vector2((xMax + xMin) * 0.5f, (yMax + yMin) * 0.5f);
        }
    }
}