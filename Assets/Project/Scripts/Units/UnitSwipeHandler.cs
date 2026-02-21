using Project.Scripts.Grid;
using UnityEngine;
using UnityEngine.EventSystems;
using Zenject;

namespace Project.Scripts.Units
{
    public class UnitSwipeHandler : MonoBehaviour, IPointerDownHandler, IDragHandler
    {
        private const float SwipeThreshold = 0.5f;
        private Camera _camera;

        private LevelGridController _gridController;
        private Vector2 _startPos;
        private bool _swipeDetected;
        private Unit _unit;

        [Inject]
        public void Construct(LevelGridController grid)
        {
            _gridController = grid;
        }

        private void Awake()
        {
            _unit = GetComponent<Unit>();
            _camera = Camera.main;
        }

        public void OnDrag(PointerEventData eventData)
        {
            if (_swipeDetected)
            {
                return;
            }

            Vector2 currentPos = _camera.ScreenToWorldPoint(eventData.position);
            var delta = currentPos - _startPos;

            if (delta.magnitude < SwipeThreshold)
            {
                return;
            }

            _swipeDetected = true;

            Vector2Int dir;

            if (Mathf.Abs(delta.x) > Mathf.Abs(delta.y))
            {
                dir = delta.x > 0 ? Vector2Int.right : Vector2Int.left;
            }
            else
            {
                dir = delta.y > 0 ? Vector2Int.up : Vector2Int.down;
            }

            TryMove(dir);
        }

        public void OnPointerDown(PointerEventData eventData)
        {
            _startPos = _camera.ScreenToWorldPoint(eventData.position);
            _swipeDetected = false;
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

            _gridController.TryMoveUnit(cell.Position.x, cell.Position.y, newX, newY);
        }
    }
}