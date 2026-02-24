using Project.Scripts.Units;
using UnityEngine;

namespace Project.Scripts.Grid
{
    public class GridCell
    {
        public GridCell(int x, int y)
        {
            Position = new Vector2Int(x, y);
            IsOccupied = false;
            OccupiedUnit = null;
        }

        public Vector2Int Position { get; private set; }

        public Unit OccupiedUnit { get; private set; }

        public bool IsOccupied { get; private set; }

        public void SetUnit(Unit unit)
        {
            OccupiedUnit = unit;
            IsOccupied = unit != null;
        }

        public void ClearUnit()
        {
            OccupiedUnit = null;
            IsOccupied = false;
        }
    }
}