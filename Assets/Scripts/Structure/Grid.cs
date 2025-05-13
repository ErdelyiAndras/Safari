using System;
using System.Collections.Generic;

[System.Serializable]
public class Point
{
    public int X;
    public int Y;

    public Point(int x, int y)
    {
        this.X = x;
        this.Y = y;
    }

    public override bool Equals(object obj)
    {
        if (obj == null)
        {
            return false;
        }
        if (obj is Point)
        {
            Point p = obj as Point;
            return this.X == p.X && this.Y == p.Y;
        }
        return false;
    }

    public override int GetHashCode()
    {
        unchecked
        {
            int hash = 6949;
            hash = hash * 7907 + X.GetHashCode();
            hash = hash * 7907 + Y.GetHashCode();
            return hash;
        }
    }

    public override string ToString()
    {
        return "P(" + this.X + ", " + this.Y + ")";
    }
}

public enum CellType
{
    Empty,
    Road,
    Water,
    Nature,
    Hill,
    None
}

public class Grid : ISaveable<GridData>
{
    private CellType[,] _grid;
    private int _width;
    public int Width { get { return _width; } }
    private int _height;
    public int Height { get { return _height; } }

    private List<Point> _roadList = new List<Point>();
    public List<Point> Roads { get; }

    public Grid(int width, int height)
    {
        _width = width;
        _height = height;
        _grid = new CellType[width, height];
    }

    public Grid(GridData data)
    {
        LoadData(data);
    }

    public CellType this[int i, int j]
    {
        get
        {
            return _grid[i, j];
        }
        set
        {
            if (value == CellType.Road)
            {
                _roadList.Add(new Point(i, j));
            }
            else
            {
                _roadList.Remove(new Point(i, j));
            }
            _grid[i, j] = value;
        }
    }

    public static bool IsCellWalkable(CellType cellType, bool aiAgent = false)
    {
        if (aiAgent)
        {
            return cellType == CellType.Road;
        }
        return cellType == CellType.Empty || cellType == CellType.Road;
    }

    public List<Point> GetAdjacentCells(Point cell, bool isAgent)
    {
        return GetWalkableAdjacentCells((int)cell.X, (int)cell.Y, isAgent);
    }

    public float GetCostOfEnteringCell(Point cell)
    {
        return 1;
    }

    public List<Point> GetAllAdjacentCells(int x, int y)
    {
        List<Point> adjacentCells = new List<Point>();
        if (x > 0)
        {
            adjacentCells.Add(new Point(x - 1, y));
        }
        if (x < _width - 1)
        {
            adjacentCells.Add(new Point(x + 1, y));
        }
        if (y > 0)
        {
            adjacentCells.Add(new Point(x, y - 1));
        }
        if (y < _height - 1)
        {
            adjacentCells.Add(new Point(x, y + 1));
        }
        return adjacentCells;
    }

    public List<Point> GetWalkableAdjacentCells(int x, int y, bool isAgent)
    {
        List<Point> adjacentCells = GetAllAdjacentCells(x, y);
        for (int i = adjacentCells.Count - 1; i >= 0; i--)
        {
            if (IsCellWalkable(_grid[adjacentCells[i].X, adjacentCells[i].Y], isAgent) == false)
            {
                adjacentCells.RemoveAt(i);
            }
        }
        return adjacentCells;
    }

    public List<Point> GetAdjacentCellsOfType(int x, int y, CellType type)
    {
        List<Point> adjacentCells = GetAllAdjacentCells(x, y);
        for (int i = adjacentCells.Count - 1; i >= 0; i--)
        {
            if (_grid[adjacentCells[i].X, adjacentCells[i].Y] != type)
            {
                adjacentCells.RemoveAt(i);
            }
        }
        return adjacentCells;
    }

    public AdjacentCellTypes GetAllAdjacentCellTypes(int x, int y)
    {
        AdjacentCellTypes neighbours = new AdjacentCellTypes();

        if (x > 0)
        {
            neighbours.Left = _grid[x - 1, y];
        }
        if (x < _width - 1)
        {
            neighbours.Right = _grid[x + 1, y];
        }
        if (y > 0)
        {
            neighbours.Down = _grid[x, y - 1];
        }
        if (y < _height - 1)
        {
            neighbours.Up = _grid[x, y + 1];
        }
        if (neighbours.Left != CellType.None && neighbours.Up != CellType.None)
        {
            neighbours.LeftUp = _grid[x - 1, y + 1];
        }
        if (neighbours.Up != CellType.None && neighbours.Right != CellType.None)
        {
            neighbours.RightUp = _grid[x + 1, y + 1];
        }
        if (neighbours.Right != CellType.None && neighbours.Down != CellType.None)
        {
            neighbours.RightDown = _grid[x + 1, y - 1];
        }
        if (neighbours.Down != CellType.None && neighbours.Left != CellType.None)
        {
            neighbours.LeftDown = _grid[x - 1, y - 1];
        }

        return neighbours;
    }

    public GridData SaveData()
    {
        return new GridData(_width, _height, _grid, _roadList);
    }

    public void LoadData(GridData data, PlacementManager placementManager = null)
    {
        _width = data.Width;
        _height = data.Height;
        _grid = data.Grid;
        _roadList = data.RoadList;
    }
}