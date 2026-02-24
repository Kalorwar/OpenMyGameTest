using System;
using System.Collections;
using System.Collections.Generic;
using Project.Scripts.Other;
using Project.Scripts.Units;
using UnityEngine;

namespace Project.Scripts.Grid
{
    public class GridNormalizer
    {
        private const float NormalizeGridDelay = 0.3f;
        private readonly MonoBehaviour _coroutineHost;
        private readonly List<List<GridCell>> _figuresBuffer = new(8);
        private readonly GridMatchesFinder _gridMatchesFinder;
        private readonly int _height;
        private readonly GridLayoutCalculator _layout;

        private readonly List<MoveOperation> _movesBuffer = new(32);
        private readonly IPlayerInputState _playerInputState;
        private readonly GridStorage _storage;
        private readonly int _width;
        private Coroutine _normalizeCoroutine;

        public GridNormalizer(int width, int height, GridStorage storage, GridLayoutCalculator layout,
            MonoBehaviour coroutineHost, IPlayerInputState playerInputState, GridMatchesFinder gridMatchesFinder)
        {
            _width = width;
            _height = height;
            _storage = storage;
            _layout = layout;
            _coroutineHost = coroutineHost;
            _playerInputState = playerInputState;
            _gridMatchesFinder = gridMatchesFinder;
        }

        public void Dispose()
        {
            if (_normalizeCoroutine != null)
            {
                _coroutineHost.StopCoroutine(_normalizeCoroutine);
                _normalizeCoroutine = null;
            }
        }

        public void TryNormalize(Action onComplete = null)
        {
            _playerInputState.SetPlayerCanAct(false);

            if (NeedsNormalization())
            {
                StartNormalize(onComplete);
            }
            else
            {
                CheckFigures(onComplete);
            }
        }

        private bool NeedsNormalization()
        {
            for (var x = 0; x < _width; x++)
            {
                var foundEmpty = false;

                for (var y = 0; y < _height; y++)
                {
                    var occupied = _storage.GetCell(x, y).IsOccupied;

                    if (!occupied)
                    {
                        foundEmpty = true;
                    }
                    else if (foundEmpty)
                    {
                        return true;
                    }
                }
            }

            return false;
        }

        private void CheckFigures(Action onComplete)
        {
            _figuresBuffer.Clear();
            _gridMatchesFinder.FindAllValidMatches(_width, _height, _figuresBuffer);

            if (_figuresBuffer.Count == 0)
            {
                _playerInputState.SetPlayerCanAct(true);
                onComplete?.Invoke();
                return;
            }

            var totalUnits = 0;
            foreach (var area in _figuresBuffer)
            {
                totalUnits += area.Count;
            }

            var destroyCounter = new DestroyCounter(totalUnits, () =>
            {
                onComplete?.Invoke();
                OnAllDestroyed();
            });

            foreach (var area in _figuresBuffer)
            {
                foreach (var cell in area)
                {
                    var unit = cell.OccupiedUnit;
                    cell.ClearUnit();
                    unit.Destroy(() => destroyCounter.Decrement());
                }
            }
        }

        private void OnAllDestroyed()
        {
            TryNormalize();
        }

        private void StartNormalize(Action onComplete)
        {
            if (_normalizeCoroutine != null)
            {
                return;
            }

            _normalizeCoroutine = _coroutineHost.StartCoroutine(NormalizeTick(onComplete));
        }

        private IEnumerator NormalizeTick(Action onComplete)
        {
            yield return new WaitForSeconds(NormalizeGridDelay);

            CollectMoves();

            if (_movesBuffer.Count == 0)
            {
                CheckFigures(onComplete);
                _normalizeCoroutine = null;
                yield break;
            }

            var pendingAnimations = _movesBuffer.Count;

            foreach (var move in _movesBuffer)
            {
                ExecuteMove(move, () => pendingAnimations--);
            }

            yield return new WaitUntil(() => pendingAnimations <= 0);
            CheckFigures(onComplete);
            _normalizeCoroutine = null;
        }

        private void CollectMoves()
        {
            _movesBuffer.Clear();

            for (var x = 0; x < _width; x++)
            {
                var emptyCount = 0;

                for (var y = 0; y < _height; y++)
                {
                    var cell = _storage.GetCell(x, y);

                    if (!cell.IsOccupied)
                    {
                        emptyCount++;
                        continue;
                    }

                    if (emptyCount > 0)
                    {
                        var targetY = y - emptyCount;
                        _movesBuffer.Add(new MoveOperation(x, y, targetY, cell.OccupiedUnit));
                    }
                }
            }
        }

        private void ExecuteMove(MoveOperation move, Action onComplete)
        {
            var sourceCell = _storage.GetCell(move.FromX, move.FromY);
            sourceCell.ClearUnit();

            var targetCell = _storage.GetCell(move.FromX, move.ToY);
            targetCell.SetUnit(move.Unit);

            var worldPos = _layout.GetCellCenter(move.FromX, move.ToY);
            var sortOrder = GridSortOrderCalculator.Calculate(move.FromX, move.ToY);

            move.Unit.AnimateMoveTo(worldPos, sortOrder, onComplete);
        }

        private readonly struct MoveOperation
        {
            public readonly int FromX;
            public readonly int FromY;
            public readonly int ToY;
            public readonly Unit Unit;

            public MoveOperation(int x, int fromY, int toY, Unit unit)
            {
                FromX = x;
                FromY = fromY;
                ToY = toY;
                Unit = unit;
            }
        }
    }
}