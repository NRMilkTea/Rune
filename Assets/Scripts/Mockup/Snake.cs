using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Mockup
{
    public class Snake : MonoBehaviour
    {
        private Camera _mainCamera;
        // prefabs
        [SerializeField] private GameObject _headObject;
        [SerializeField] private GameObject _bodySegmentObject;

        public SnakeHead _head;
        public Queue<SnakeBodySegment> _body;
        public int _growingBuffer;
        public Vector2Int _startingPoint;

        private float _playerMovingControlCooldownInterval = 0.1f;
        private bool _canPlayerMovingControl = true;

        private void Awake()
        {
            // Link prefabs to static references
            SnakeHead._headPrefab = _headObject;
            SnakeBodySegment._bodySegmentPrefab = _bodySegmentObject;
            //
            _mainCamera = Camera.main;
        }

        private void Start()
        {
            _startingPoint = Vector2Int.zero;

            _head = SnakeHead.Create(this.gameObject);
            _body = new();
        }

        private void Update()
        {
            // player control
            if (_canPlayerMovingControl)
            {
                Direction direction = GetExactlyOneArrowKey();
                if (direction != Direction.None) // Only accept one arrow key control
                {
                    bool canMoveToDirection = CanMoveTo(direction);
                    if (canMoveToDirection)
                    {
                        MoveTo(direction);
                    }
                }
            }
        }

        private void MoveTo(Direction direction)
        {
            _canPlayerMovingControl = false;

            Direction oldFacing = _head.facing;
            Direction newFacing = direction;
            Vector2 oldHeadPosition = _head.transform.position;
            Vector2 displacement = GetDirectionToVector(direction);
            Vector2 newHeadPosition = oldHeadPosition + displacement;

            // Move head to new position and rotate
            _head.transform.position = newHeadPosition;
            _head.facing = newFacing;
            _head.Draw();

            // Add the body segment in the original position of head
            SnakeBodySegment newBodySegment = SnakeBodySegment.Create(this.gameObject);
            newBodySegment.transform.position = oldHeadPosition;
            newBodySegment.fromDirection = oldFacing;
            newBodySegment.toDirection = newFacing;
            newBodySegment.isTail = false;
            newBodySegment.Draw();
            _body.Enqueue(newBodySegment);

            // Update the snake's tail
            if (_growingBuffer > 0)
            {
                _growingBuffer--;
            }
            else if (_growingBuffer == 0)
            {
                // Remove old tail
                SnakeBodySegment oldTailSegment = _body.Dequeue();
                oldTailSegment.Remove();
            }
            SnakeBodySegment currentTailSegment = _body.Peek();
            currentTailSegment.isTail = true;
            currentTailSegment.Draw();

            StartCoroutine(PlayerMovingControlCooldown());
        }

        private IEnumerator PlayerMovingControlCooldown()
        {
            yield return new WaitForSeconds(_playerMovingControlCooldownInterval);
            _canPlayerMovingControl = true;
        }

        private bool CanMoveTo(Direction direction)
        {
            Vector2 oldHeadPosition = _head.transform.position;
            Vector2 displacement = GetDirectionToVector(direction);
            Vector2 newHeadPosition = oldHeadPosition + displacement;

            RaycastHit2D[] hits = Physics2D.RaycastAll(newHeadPosition, Vector2.zero, Mathf.Infinity);
            bool canMoveTo = true;
            foreach (var hit in hits)
            {
                if (hit.collider.gameObject.GetComponent<SnakeBodySegment>() != null)
                    if (hit.collider.gameObject.GetComponent<SnakeBodySegment>().isTail == false)
                        canMoveTo = false;
                if (hit.collider.gameObject.GetComponent<TileSlot>() != null)
                    if (hit.collider.gameObject.GetComponent<TileSlot>().data.type == TileSlotType.Barrier)
                        canMoveTo = false;
            }
            return canMoveTo;
        }

        private Vector2 GetDirectionToVector(Direction direction)
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

        private Direction GetExactlyOneArrowKey()
        {
            bool getKeyUp = Input.GetKey(KeyCode.UpArrow);
            bool getKeyRight = Input.GetKey(KeyCode.RightArrow);
            bool getKeyDown = Input.GetKey(KeyCode.DownArrow);
            bool getKeyLeft = Input.GetKey(KeyCode.LeftArrow);

            if (getKeyUp && !getKeyRight && !getKeyDown && !getKeyLeft) return Direction.Up;
            else if (!getKeyUp && getKeyRight && !getKeyDown && !getKeyLeft) return Direction.Right;
            else if (!getKeyUp && !getKeyRight && getKeyDown && !getKeyLeft) return Direction.Down;
            else if (!getKeyUp && !getKeyRight && !getKeyDown && getKeyLeft) return Direction.Left;

            return Direction.None;
        }
    }
    public enum Direction
    {
        Up, Right, Down, Left, None
    }
}