using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace Project.Scripts.Grid
{
    public class GridMatchesFinder
    {
        private readonly List<GridCell> _areaBuffer = new(32);
        private readonly int[] _dx = { 1, -1, 0, 0 };
        private readonly int[] _dy = { 0, 0, 1, -1 };
        private readonly Stack<GridCell> _stack = new(32);
        private readonly GridStorage _storage;

        public GridMatchesFinder(GridStorage storage)
        {
            _storage = storage;
        }

        public void FindAllValidMatches(int width, int height, List<List<GridCell>> result)
        {
            result.Clear();

            var visited = new BitArray(width * height);

            for (var x = 0; x < width; x++)
            {
                for (var y = 0; y < height; y++)
                {
                    var index = x + y * width;
                    if (visited[index])
                    {
                        continue;
                    }

                    var cell = _storage.GetCell(x, y);
                    if (cell == null || !cell.IsOccupied)
                    {
                        visited[index] = true;
                        continue;
                    }

                    _areaBuffer.Clear();
                    CollectArea(cell, _areaBuffer, visited, width);

                    if (HasValidLine(_areaBuffer))
                    {
                        result.Add(new List<GridCell>(_areaBuffer));
                    }
                }
            }
        }

        private void CollectArea(GridCell start, List<GridCell> area, BitArray visited, int width)
        {
            _stack.Clear();
            _stack.Push(start);
            MarkVisited(visited, start.Position.x, start.Position.y, width);

            var targetType = start.OccupiedUnit.ElementType;

            while (_stack.Count > 0)
            {
                var cell = _stack.Pop();
                area.Add(cell);

                for (var i = 0; i < 4; i++)
                {
                    var nx = cell.Position.x + _dx[i];
                    var ny = cell.Position.y + _dy[i];

                    if (!IsValid(nx, ny, targetType, visited, width))
                    {
                        continue;
                    }

                    MarkVisited(visited, nx, ny, width);
                    _stack.Push(_storage.GetCell(nx, ny));
                }
            }
        }

        private bool IsValid(int x, int y, string type, BitArray visited, int width)
        {
            if (x < 0 || y < 0 || x >= _storage.Width || y >= _storage.Height)
            {
                return false;
            }

            var index = x + y * width;
            if (visited[index])
            {
                return false;
            }

            var cell = _storage.GetCell(x, y);
            if (!cell.IsOccupied)
            {
                return false;
            }

            return cell.OccupiedUnit.ElementType == type;
        }

        private static void MarkVisited(BitArray visited, int x, int y, int width)
        {
            visited[x + y * width] = true;
        }

        private bool HasValidLine(List<GridCell> area)
        {
            return area.GroupBy(c => c.Position.y).Any(g => g.Count() >= 3) ||
                   area.GroupBy(c => c.Position.x).Any(g => g.Count() >= 3);
        }
    }
}