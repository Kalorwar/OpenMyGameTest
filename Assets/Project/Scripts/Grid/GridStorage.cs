using System.Collections.Generic;
using System.Linq;
using Project.Scripts.Units;

namespace Project.Scripts.Grid
{
    public class GridStorage
    {
        private readonly List<GridCell> _cells;

        public GridStorage(int width, int height)
        {
            _cells = new List<GridCell>();
            for (var x = 0; x < width; x++)
            for (var y = 0; y < height; y++)
            {
                _cells.Add(new GridCell(x, y));
            }
        }

        public GridCell GetCell(int x, int y)
        {
            return _cells.FirstOrDefault(c => c.Position.x == x && c.Position.y == y);
        }

        public GridCell GetCellOfUnit(Unit unit)
        {
            return _cells.FirstOrDefault(c => c.OccupiedUnit == unit);
        }
    }
}