using System.Collections.Generic;
using Project.Scripts.Level;
using Project.Scripts.Units;
using UnityEngine;

namespace Project.Scripts.Other
{
    public static class UnitPositionCalculator
    {
        public static void SetUnitPositions(LevelData levelData, Vector3 startPos, float cellSize,
            List<Unit> units)
        {
            if (levelData?.Units == null || units == null)
            {
                return;
            }

            var counts = new Dictionary<Vector2Int, int>();
            var indices = new Dictionary<Vector2Int, int>();

            foreach (var unit in levelData.Units)
            {
                var cell = new Vector2Int((int)unit.CellX, (int)unit.CellY);

                if (!counts.TryAdd(cell, 1))
                {
                    counts[cell]++;
                }
            }

            for (var i = 0; i < levelData.Units.Count && i < units.Count; i++)
            {
                var unit = levelData.Units[i];
                var cell = new Vector2Int((int)unit.CellX, (int)unit.CellY);

                indices.TryGetValue(cell, out var index);

                var x = startPos.x + cell.x * cellSize + cellSize * 0.5f;
                var y = startPos.y + cell.y * cellSize + cellSize * 0.5f;

                units[i].transform.position = new Vector3(x, y, 0);
                indices[cell] = index + 1;
            }
        }
    }
}