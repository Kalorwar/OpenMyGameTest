using Project.Scripts.Level;
using Project.Scripts.Other;
using Project.Scripts.Units;
using UnityEngine;
using Zenject;

namespace Project.Scripts.Grid
{
    public class LevelGridController : MonoBehaviour
    {
        private const float ScreenFillAmount = 0.95f;
        private const float FixedBottomGridY = -2.9f;
        private int _height;
        private GridLayoutCalculator _layout;
        private ILevelDataProvider _levelDataProvider;

        private GridMatchesFinder _matchesFinder;
        private GridNormalizer _normalizer;
        private IPlayerInputState _playerInputState;
        private GridStorage _storage;
        private int _width;

        [Inject]
        private void Construct(ILevelDataProvider levelDataProvider, IPlayerInputState playerInputState)
        {
            _levelDataProvider = levelDataProvider;
            _playerInputState = playerInputState;
        }

        private void Awake()
        {
            InitializeGrid();
        }

        public void OnDestroy()
        {
            _normalizer?.Dispose();
        }

        private void InitializeGrid()
        {
            _width = _levelDataProvider.CurrentLevelData.Width;
            _height = _levelDataProvider.CurrentLevelData.Height;

            _layout = new GridLayoutCalculator(ScreenFillAmount, FixedBottomGridY);
            _layout.Calculate(Camera.main, _width, _height);

            _storage = new GridStorage(_width, _height);
            _matchesFinder = new GridMatchesFinder(_storage);
            _normalizer =
                new GridNormalizer(_width, _height, _storage, _layout, this, _playerInputState, _matchesFinder);
        }

        public void PlaceUnitAtCell(Unit unit, int x, int y)
        {
            var cell = GetValidCell(x, y);
            if (cell == null)
            {
                return;
            }

            cell.SetUnit(unit);
            SetUnitPosition(unit, cell);
        }

        public void TryMoveUnit(int fromX, int fromY, int toX, int toY)
        {
            if (!TryGetMovePair(fromX, fromY, toX, toY, out var fromCell, out var toCell))
            {
                return;
            }

            ExecuteMove(fromCell, toCell);
            _normalizer.TryNormalize();
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

        private bool TryGetMovePair(int fromX, int fromY, int toX, int toY, out GridCell fromCell, out GridCell toCell)
        {
            fromCell = _storage.GetCell(fromX, fromY);
            toCell = _storage.GetCell(toX, toY);
            return fromCell != null && toCell != null;
        }

        private void ExecuteMove(GridCell fromCell, GridCell toCell)
        {
            if (toCell.IsOccupied)
            {
                SwapUnits(fromCell, toCell);
            }
            else
            {
                MoveUnit(fromCell.OccupiedUnit, toCell);
            }
        }

        private void SwapUnits(GridCell fromCell, GridCell toCell)
        {
            var unitA = fromCell.OccupiedUnit;
            var unitB = toCell.OccupiedUnit;

            fromCell.ClearUnit();
            toCell.ClearUnit();

            fromCell.SetUnit(unitB);
            toCell.SetUnit(unitA);

            AnimateSwap(unitA, toCell, unitB, fromCell);
        }

        private void AnimateSwap(Unit unitA, GridCell cellA, Unit unitB, GridCell cellB)
        {
            var posA = GetCellWorldPosition(cellA);
            var posB = GetCellWorldPosition(cellB);

            unitA.AnimateMoveTo(posA, GetSortOrder(cellA));
            unitB.AnimateMoveTo(posB, GetSortOrder(cellB));
        }

        private void MoveUnit(Unit unit, GridCell toCell)
        {
            var currentCell = _storage.GetCellOfUnit(unit);
            currentCell.ClearUnit();
            toCell.SetUnit(unit);
            unit.AnimateMoveTo(GetCellWorldPosition(toCell), GetSortOrder(toCell));
        }

        private void SetUnitPosition(Unit unit, GridCell cell)
        {
            unit.transform.position = GetCellWorldPosition(cell);
            unit.ChangeSortOrder(GetSortOrder(cell));
        }

        private Vector3 GetCellWorldPosition(GridCell cell)
        {
            return _layout.GetCellCenter(cell.Position.x, cell.Position.y);
        }

        private int GetSortOrder(GridCell cell)
        {
            return GridSortOrderCalculator.Calculate(cell.Position.x, cell.Position.y);
        }

        private GridCell GetValidCell(int x, int y)
        {
            var cell = _storage.GetCell(x, y);
            return cell?.IsOccupied == false ? cell : null;
        }
    }
}