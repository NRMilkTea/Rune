using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements.Experimental;

public class SnakeBodySegment : MonoBehaviour
{
    public static GameObject _bodySegmentObject;

    public GameObject centerPiece;
    public GameObject connectionPiece;

    public Vector2Int coordinate;

    public Direction fromDirection;
    public Direction toDirection;
    public bool isTail;

    public static SnakeBodySegment Create()
    {
        GameObject newBodySegmentObject = Instantiate(_bodySegmentObject);
        SnakeBodySegment newbodySegment = newBodySegmentObject.GetComponent<SnakeBodySegment>();
        return newbodySegment;
    }

    public void Draw()
    {
        centerPiece.transform.rotation = GetDirectionToRotation(toDirection);
        connectionPiece.transform.rotation = GetDirectionToRotation(toDirection);
        connectionPiece.transform.localPosition = GetDirectionToConnectionPieceLocalPosition(toDirection);
    }

    private Vector3 GetDirectionToConnectionPieceLocalPosition(Direction toDirection)
    {
        switch (toDirection)
        {
            case Direction.Right: return new Vector3(0.5f, 0, 0);
            case Direction.Up: return new Vector3(0, 0.5f, 0);
            case Direction.Left: return new Vector3(-0.5f, 0, 0);
            case Direction.Down: return new Vector3(0, -0.5f, 0);
            default: throw new ArgumentOutOfRangeException(nameof(toDirection));
        }
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

    public void Remove()
    {
        Destroy(this.gameObject);
    }
}
