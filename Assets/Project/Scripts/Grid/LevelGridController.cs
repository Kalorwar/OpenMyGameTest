using Project.Scripts.Level;
using Project.Scripts.Units;
using UnityEngine;
using Zenject;

namespace Project.Scripts.Grid
{
    public class LevelGridController : MonoBehaviour
    {
        private const float ScreenFillAmount = 0.95f;
        private const float NormalizeGridDelay = 0.3f;
        private const float FixedBottomGridY = -2.9f;

        private int _height;
        private GridLayoutCalculator _layout;
        private ILevelDataProvider _levelDataProvider;
        private GridNormalizer _normalizer;
        private GridStorage _storage;
        private int _width;

        [Inject]
        private void Construct(ILevelDataProvider levelDataProvider)
        {
            _levelDataProvider = levelDataProvider;
        }

        private void Awake()
        {
            _width = _levelDataProvider.CurrentLevelData.Width;
            _height = _levelDataProvider.CurrentLevelData.Height;

            _layout = new GridLayoutCalculator(ScreenFillAmount, FixedBottomGridY);
            _layout.Calculate(Camera.main, _width, _height);

            _storage = new GridStorage(_width, _height);
            _normalizer = new GridNormalizer(_storage, _layout, this, NormalizeGridDelay);
        }

        public void PlaceUnitAtCell(Unit unit, int x, int y)
        {
            var cell = _storage.GetCell(x, y);
            if (cell == null)
            {
                return;
            }

            cell.SetUnit(unit);
            unit.transform.position = _layout.GetCellCenter(x, y);
            unit.ChangeSortOrder(y * 10 + x);
        }

        public void TryMoveUnit(int fromX, int fromY, int toX, int toY)
        {
            var fromCell = _storage.GetCell(fromX, fromY);
            var toCell = _storage.GetCell(toX, toY);

            if (fromCell == null || toCell == null)
            {
                return;
            }

            if (!toCell.IsOccupied)
            {
                var unit = fromCell.OccupiedUnit;
                fromCell.ClearUnit();
                PlaceUnitAtCell(unit, toX, toY);
            }
            else
            {
                SwapUnits(fromCell, toCell);
            }

            if (_normalizer.NeedsNormalization(_width, _height))
            {
                _normalizer.StartNormalize(_width, _height);
            }
        }

        public float GetCellSize()
        {
            return _layout.CellSize;
        }

        public GridCell GetCell(int x, int y)
        {
            return _storage.GetCell(x, y);
        }

        public GridCell GetCellOfUnit(Unit unit)
        {
            return _storage.GetCellOfUnit(unit);
        }

        private void SwapUnits(GridCell fromCell, GridCell toCell)
        {
            var unitA = fromCell.OccupiedUnit;
            var unitB = toCell.OccupiedUnit;

            fromCell.ClearUnit();
            toCell.ClearUnit();

            PlaceUnitAtCell(unitA, toCell.Position.x, toCell.Position.y);
            PlaceUnitAtCell(unitB, fromCell.Position.x, fromCell.Position.y);
        }
    }
}