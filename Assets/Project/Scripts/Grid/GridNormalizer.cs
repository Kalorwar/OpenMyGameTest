using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Project.Scripts.Other;
using Project.Scripts.Units;
using UnityEngine;

namespace Project.Scripts.Grid
{
    public class GridNormalizer
    {
        private const float NormalizeGridDelay = 0.3f;
        private readonly MonoBehaviour _coroutineHost;
        private readonly GridMatchesFinder _gridMatchesFinder;
        private readonly int _height;
        private readonly GridLayoutCalculator _layout;
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
                var emptyFound = false;

                for (var y = 0; y < _height; y++)
                {
                    var occupied = _storage.GetCell(x, y).IsOccupied;

                    if (!occupied)
                    {
                        emptyFound = true;
                    }
                    else if (emptyFound)
                    {
                        return true;
                    }
                }
            }

            return false;
        }

        private void CheckFigures(Action onComplete)
        {
            var figures = _gridMatchesFinder.FindAllValidMatches(_width, _height);

            if (figures.Count == 0)
            {
                _playerInputState.SetPlayerCanAct(true);
                onComplete?.Invoke();
                return;
            }

            var destroyCounter = new DestroyCounter(figures.Sum(area => area.Count), () =>
            {
                OnAllDestroyed();
                onComplete?.Invoke();
            });

            foreach (var area in figures)
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

            var moves = CollectMoves();

            if (moves.Count == 0)
            {
                CheckFigures(onComplete);
                _normalizeCoroutine = null;
                yield break;
            }

            var pendingAnimations = moves.Count;

            foreach (var move in moves)
            {
                ExecuteMove(move, () => { pendingAnimations--; });
            }

            yield return new WaitUntil(() => pendingAnimations <= 0);
            CheckFigures(onComplete);
            _normalizeCoroutine = null;
        }

        private List<MoveOperation> CollectMoves()
        {
            var moves = new List<MoveOperation>();

            for (var x = 0; x < _width; x++)
            {
                var writeY = 0;

                for (var y = 0; y < _height; y++)
                {
                    var cell = _storage.GetCell(x, y);
                    if (!cell.IsOccupied)
                    {
                        continue;
                    }

                    if (y != writeY)
                    {
                        moves.Add(new MoveOperation(x, y, writeY, cell.OccupiedUnit));
                    }

                    writeY++;
                }
            }

            return moves;
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