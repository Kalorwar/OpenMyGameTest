using System.Collections;
using UnityEngine;

namespace Project.Scripts.Grid
{
    public class GridNormalizer
    {
        private readonly float _delay;
        private readonly GridLayoutCalculator _layout;
        private readonly GridStorage _storage;
        private Coroutine _normalizeCoroutine;

        public GridNormalizer(GridStorage storage, GridLayoutCalculator layout, float delay)
        {
            _storage = storage;
            _layout = layout;
            _delay = delay;
        }

        public bool NeedsNormalization(int width, int height)
        {
            for (var x = 0; x < width; x++)
            {
                var foundEmpty = false;

                for (var y = 0; y < height; y++)
                {
                    var cell = _storage.GetCell(x, y);

                    if (!cell.IsOccupied)
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

        public void StartNormalize(int width, int height, MonoBehaviour coroutineHost)
        {
            if (_normalizeCoroutine == null)
            {
                _normalizeCoroutine = coroutineHost.StartCoroutine(NormalizeTick(width, height));
            }
        }

        private IEnumerator NormalizeTick(int width, int height)
        {
            yield return new WaitForSeconds(_delay);

            for (var x = 0; x < width; x++)
            {
                var writeY = 0;

                for (var y = 0; y < height; y++)
                {
                    var cell = _storage.GetCell(x, y);

                    if (cell.IsOccupied)
                    {
                        if (y != writeY)
                        {
                            var unit = cell.OccupiedUnit;
                            cell.ClearUnit();

                            var targetCell = _storage.GetCell(x, writeY);
                            targetCell.SetUnit(unit);
                            var worldPos = _layout.GetCellCenter(x, writeY);

                            var sortOrder = writeY * 10 + x;

                            unit.AnimateMoveTo(worldPos, sortOrder);
                        }

                        writeY++;
                    }
                }
            }

            _normalizeCoroutine = null;
        }
    }
}