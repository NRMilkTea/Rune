using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements.Experimental;

public class SnakeBodySegment : MonoBehaviour
{
    public static GameObject _bodySegmentPrefab;

    [SerializeField] private Sprite _bodySegmentStraight;
    [SerializeField] private Sprite _bodySegmentLeftTurn;
    [SerializeField] private Sprite _bodySegmentRightTurn;
    [SerializeField] private Sprite _tailSegment;

    public GameObject centerPiece;
    public GameObject connectionPiece;

    public Vector2Int coordinate;

    public Direction fromDirection;
    public Direction toDirection;
    public bool isTail;

    public static SnakeBodySegment Create(GameObject parentObject)
    {
        GameObject newBodySegmentObject = Instantiate(_bodySegmentPrefab, parentObject.transform);
        SnakeBodySegment newbodySegment = newBodySegmentObject.GetComponent<SnakeBodySegment>();
        return newbodySegment;
    }

    public void Draw()
    {
        this.transform.rotation = GetRotationFromDirection(toDirection);
        
        if (isTail) centerPiece.GetComponent<SpriteRenderer>().sprite = _tailSegment;
        else
        {
            switch (GetRotationIndexFromDirections(fromDirection, toDirection))
            {
                case 0: // straight
                    centerPiece.GetComponent<SpriteRenderer>().sprite = _bodySegmentStraight; break;
                case 1: // left turn
                    centerPiece.GetComponent<SpriteRenderer>().sprite = _bodySegmentLeftTurn; break;
                case 3: // right turn
                    centerPiece.GetComponent<SpriteRenderer>().sprite = _bodySegmentRightTurn; break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }
    }

    private int GetRotationIndexFromDirections(Direction fromDirection, Direction toDirection)
    {
        int fromDirectionIndex = GetDirectionIndexFromDirection(fromDirection);
        int toDirectionIndex   = GetDirectionIndexFromDirection(toDirection);

        int rotationIndex = (toDirectionIndex - fromDirectionIndex + 4) % 4;
        return rotationIndex;
    }

    private int GetDirectionIndexFromDirection(Direction direction)
    {
        switch (direction)
        {
            case Direction.Right: return 0;
            case Direction.Up: return 1;
            case Direction.Left: return 2;
            case Direction.Down: return 3;
            default: throw new ArgumentOutOfRangeException(nameof(direction));
        }
    }

    private Vector3 GetVectorFromDirection(Direction direction)
    {
        switch (direction)
        {
            case Direction.Up: return Vector2.up;
            case Direction.Down: return Vector2.down;
            case Direction.Left: return Vector2.left;
            case Direction.Right: return Vector2.right;
            default: throw new ArgumentOutOfRangeException(nameof(direction));
        }
    }
    
    private Quaternion GetRotationFromDirection(Direction facing)
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