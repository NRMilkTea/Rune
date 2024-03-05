using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SnakeHead : MonoBehaviour
{
    public static GameObject _headPrefab;

    public Vector2Int coordinate;
    public Direction facing;

    public static SnakeHead Create(GameObject parentObject)
    {
        GameObject newSnakeHeadObject = Instantiate(_headPrefab, parentObject.transform);
        SnakeHead newSnakeHead = newSnakeHeadObject.GetComponent<SnakeHead>();
        return newSnakeHead;
    }

    public void Draw()
    {
        // Set head object rotation to facing
        this.transform.rotation = GetDirectionToRotation(facing);
    }

    private Quaternion GetDirectionToRotation(Direction facing)
    {
        switch (facing)
        {
            case Direction.Right: return Quaternion.Euler(0, 0, 0);
            case Direction.Up: return Quaternion.Euler(0, 0, 90);
            case Direction.Left: return Quaternion.Euler(0, 0, 180);
            case Direction.Down: return Quaternion.Euler(0, 0, 270);
            default: throw new ArgumentOutOfRangeException(nameof(facing));
        }
    }
}
