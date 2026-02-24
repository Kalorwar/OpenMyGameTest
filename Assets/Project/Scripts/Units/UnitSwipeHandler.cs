using Project.Scripts.Grid;
using Project.Scripts.Other;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Project.Scripts.Units
{
    public class SwipeHandler
    {
        private const float SwipeThreshold = 0.5f;
        private readonly Camera _camera;
        private readonly LevelGridController _gridController;
        private readonly IPlayerInputState _playerInputState;

        private readonly Unit _unit;

        private Vector2 _startPos;
        private bool _swipeDetected;

        public SwipeHandler(Unit unit, LevelGridController gridController, IPlayerInputState playerInputState)
        {
            _unit = unit;
            _gridController = gridController;
            _playerInputState = playerInputState;
            _camera = Camera.main;
        }

        public void OnPointerDown(PointerEventData eventData)
        {
            _startPos = _camera.ScreenToWorldPoint(eventData.position);
            _swipeDetected = false;
        }

        public void OnDrag(PointerEventData eventData)
        {
            if (_swipeDetected || !_playerInputState.PlayerCanAct)
            {
                return;
            }

            var currentPos = (Vector2)_camera.ScreenToWorldPoint(eventData.position);
            var delta = currentPos - _startPos;

            if (delta.magnitude < SwipeThreshold)
            {
                return;
            }

            _swipeDetected = true;

            TryMove(GetDirection(delta));
        }

        private static Vector2Int GetDirection(Vector2 delta)
        {
            return Mathf.Abs(delta.x) > Mathf.Abs(delta.y)
                ? delta.x > 0 ? Vector2Int.right : Vector2Int.left
                : delta.y > 0
                    ? Vector2Int.up
                    : Vector2Int.down;
        }

        private void TryMove(Vector2Int dir)
        {
            var cell = _gridController.GetCellOfUnit(_unit);
            if (cell == null)
            {
                return;
            }

            var newX = cell.Position.x + dir.x;
            var newY = cell.Position.y + dir.y;
            var targetCell = _gridController.GetCell(newX, newY);

            if (dir == Vector2Int.up && (targetCell == null || !targetCell.IsOccupied))
            {
                return;
            }

            _playerInputState.SetPlayerCanAct(false);
            _gridController.TryMoveUnit(cell.Position.x, cell.Position.y, newX, newY);
        }
    }
}