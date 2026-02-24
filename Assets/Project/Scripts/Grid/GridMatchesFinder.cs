using System;
using System.Collections.Generic;
using System.Linq;

namespace Project.Scripts.Grid
{
    public class GridMatchesFinder
    {
        private readonly (int dx, int dy)[] _directions =
        {
            (1, 0), (-1, 0), (0, 1), (0, -1)
        };

        private readonly GridStorage _storage;

        public GridMatchesFinder(GridStorage storage)
        {
            _storage = storage;
        }

        public List<List<GridCell>> FindAllValidMatches(int width, int height)
        {
            var visited = new bool[width, height];
            var result = new List<List<GridCell>>();

            for (var x = 0; x < width; x++)
            {
                for (var y = 0; y < height; y++)
                {
                    if (IsAlreadyProcessed(x, y, visited))
                    {
                        continue;
                    }

                    var area = CollectArea(x, y, visited);

                    if (HasValidLine(area))
                    {
                        result.Add(area);
                    }
                }
            }

            return result;
        }

        private bool IsAlreadyProcessed(int x, int y, bool[,] visited)
        {
            var cell = _storage.GetCell(x, y);
            return cell == null || !cell.IsOccupied || visited[x, y];
        }

        private List<GridCell> CollectArea(int startX, int startY, bool[,] visited)
        {
            var startCell = _storage.GetCell(startX, startY);
            var targetType = startCell.OccupiedUnit.ElementType;

            var stack = new Stack<GridCell>();
            var area = new List<GridCell>();

            stack.Push(startCell);
            visited[startX, startY] = true;

            while (stack.Count > 0)
            {
                var cell = stack.Pop();
                area.Add(cell);

                foreach (var neighbor in GetMatchingNeighbors(cell, targetType, visited))
                {
                    visited[neighbor.Position.x, neighbor.Position.y] = true;
                    stack.Push(neighbor);
                }
            }

            return area;
        }

        private bool IsValidMatch(GridCell neighbor, string type, bool[,] visited)
        {
            if (visited[neighbor.Position.x, neighbor.Position.y])
            {
                return false;
            }

            if (!neighbor.IsOccupied)
            {
                return false;
            }

            return neighbor.OccupiedUnit.ElementType == type;
        }

        private IEnumerable<GridCell> GetMatchingNeighbors(GridCell cell, string type, bool[,] visited)
        {
            foreach (var neighbor in GetNeighbors(cell))
            {
                if (IsValidMatch(neighbor, type, visited))
                {
                    yield return neighbor;
                }
            }
        }

        private IEnumerable<GridCell> GetNeighbors(GridCell cell)
        {
            foreach (var (dx, dy) in _directions)
            {
                var neighbor = _storage.GetCell(cell.Position.x + dx, cell.Position.y + dy);
                if (neighbor != null)
                {
                    yield return neighbor;
                }
            }
        }

        private bool HasValidLine(List<GridCell> area)
        {
            return HasLineOfLength(area, c => c.Position.y, 3) ||
                   HasLineOfLength(area, c => c.Position.x, 3);
        }

        private bool HasLineOfLength(List<GridCell> area, Func<GridCell, int> groupByKey, int minLength)
        {
            return area.GroupBy(groupByKey)
                .Any(g => g.Count() >= minLength);
        }
    }
}