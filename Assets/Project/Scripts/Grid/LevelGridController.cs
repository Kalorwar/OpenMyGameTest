using System.Collections.Generic;
using System.Linq;
using Project.Scripts.Level;
using Project.Scripts.Units;
using UnityEditor;
using UnityEngine;
using Zenject;

namespace Project.Scripts.Grid
{
    public class LevelGridController : MonoBehaviour
    {
        private const float ScreenFillAmount = 0.95f;
        [SerializeField] private float _fixedBottomGridY = -2.9f;

        private Vector3 _gridStartPosition;

        private int _height;

        private ILevelDataProvider _levelDataProvider;
        private Camera _mainCamera;

        private int _width;

        [Inject]
        private void Construct(ILevelDataProvider levelDataProvider)
        {
            _levelDataProvider = levelDataProvider;
        }

        public float CellSize { get; private set; } = 0.8f;

        public List<GridCell> Cells { get; private set; }

        private void Awake()
        {
            _mainCamera = Camera.main;
            Initialize();
        }

        private void OnDrawGizmosSelected()
        {
            if (_width == 0 || _height == 0 || Cells == null)
            {
                return;
            }

            for (var x = 0; x < _width; x++)
            {
                for (var y = 0; y < _height; y++)
                {
                    var center = GetCellCenter(x, y);
                    var cell = GetCell(x, y);
                    Gizmos.color = cell != null && cell.IsOccupied
                        ? new Color(1, 0, 0, 0.3f)
                        : new Color(0, 1, 0, 0.3f);

                    Gizmos.DrawWireCube(center, new Vector3(CellSize, CellSize, 0.1f));
#if UNITY_EDITOR
                    Handles.color = Color.white;
                    Handles.Label(center - new Vector3(0.1f, 0.1f, 0), $"{x},{y}");
#endif
                }
            }

            if (_mainCamera != null)
            {
                Gizmos.color = Color.yellow;
                var screenHeight = _mainCamera.orthographicSize * 2;
                var screenWidth = screenHeight * _mainCamera.aspect;
                Gizmos.DrawWireCube(Vector3.zero, new Vector3(screenWidth, screenHeight, 0.1f));
            }
        }

        private void Initialize()
        {
            _width = _levelDataProvider.CurrentLevelData.Width;
            _height = _levelDataProvider.CurrentLevelData.Height;

            if (_mainCamera != null)
            {
                CalculateOptimalGridPosition();
            }

            Cells = new List<GridCell>();
            for (var x = 0; x < _width; x++)
            {
                for (var y = 0; y < _height; y++)
                {
                    Cells.Add(new GridCell(x, y));
                }
            }
        }

        public void PlaceUnitAtCell(Unit unit, int x, int y)
        {
            var cell = GetCell(x, y);
            if (cell != null)
            {
                cell.SetUnit(unit);
                unit.transform.position = GetCellCenter(x, y);
            }
        }

        public void RemoveUnitFromCell(int x, int y)
        {
            var cell = GetCell(x, y);
            cell?.ClearUnit();
        }

        private void CalculateOptimalGridPosition()
        {
            var screenHeight = _mainCamera.orthographicSize * 2;
            var screenWidth = screenHeight * _mainCamera.aspect;

            var availableWidth = screenWidth * ScreenFillAmount;
            var availableHeight = screenHeight * ScreenFillAmount;

            var cellSizeByWidth = availableWidth / _width;
            var cellSizeByHeight = availableHeight / _height;

            CellSize = Mathf.Min(cellSizeByWidth, cellSizeByHeight);

            var totalGridWidth = _width * CellSize;
            var totalGridHeight = _height * CellSize;

            _gridStartPosition = new Vector3(
                -totalGridWidth / 2,
                _fixedBottomGridY - CellSize / 2,
                0
            );
        }

        private Vector3 GetCellCenter(int x, int y)
        {
            return new Vector3(
                _gridStartPosition.x + x * CellSize + CellSize / 2,
                _gridStartPosition.y + y * CellSize + CellSize / 2,
                0
            );
        }

        private GridCell GetCell(int x, int y)
        {
            return Cells.FirstOrDefault(cell => cell.Position.x == x && cell.Position.y == y);
        }
    }
}