using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class GridData
{
    [SerializeField]
    private int width;
    [SerializeField]
    private int height;
    [SerializeField]
    private List<CellType> grid;
    [SerializeField]
    private List<Point> roadList;

    public int Width { get { return width; } }
    public int Height { get { return height; } }
    public CellType[,] Grid
    {
        get
        {
            CellType[,] _grid = new CellType[width, height];
            for (int i = 0; i < width; i++)
            {
                for (int j = 0; j < height; j++)
                {
                    _grid[i, j] = grid[i * height + j];
                }
            }
            return _grid;
        }
    }
    public List<Point> RoadList
    {
        get { return roadList; }
    }

    public GridData(int width, int height, CellType[,] grid, List<Point> roadList)
    {
        this.width = width;
        this.height = height;
        this.grid = new List<CellType>(width * height);
        for (int i = 0; i < width; i++)
        {
            for (int j = 0; j < height; j++)
            {
                this.grid.Add(grid[i, j]);
            }
        }
        this.roadList = roadList;
    }
}