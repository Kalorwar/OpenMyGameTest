using System.Collections.Generic;
using Project.Scripts.Grid;
using Project.Scripts.Units;

public class GridStorage
{
    private readonly GridCell[,] _cells;

    public GridStorage(int width, int height)
    {
        Width = width;
        Height = height;
        _cells = new GridCell[width, height];

        for (var x = 0; x < width; x++)
        for (var y = 0; y < height; y++)
        {
            _cells[x, y] = new GridCell(x, y);
        }
    }

    public int Width { get; }

    public int Height { get; }

    public GridCell GetCell(int x, int y)
    {
        if (x < 0 || x >= Width || y < 0 || y >= Height)
        {
            return null;
        }

        return _cells[x, y];
    }

    public GridCell GetCellOfUnit(Unit unit)
    {
        for (var x = 0; x < Width; x++)
        for (var y = 0; y < Height; y++)
        {
            var cell = _cells[x, y];
            if (cell.OccupiedUnit == unit)
            {
                return cell;
            }
        }

        return null;
    }

    public IEnumerable<GridCell> GetOccupiedCells()
    {
        for (var x = 0; x < Width; x++)
        for (var y = 0; y < Height; y++)
        {
            var cell = _cells[x, y];
            if (cell.IsOccupied)
            {
                yield return cell;
            }
        }
    }
}