
using System.Collections.Generic;
using System.Runtime.InteropServices.WindowsRuntime;
using UnityEngine;
using UnityEngine.UIElements;

public class SearchViewDistance
{
    private List<Vector3Int> discoveredFood;
    private List<Vector3Int> discoveredDrink;

    private List<Vector3Int> foodResult = new List<Vector3Int>();
    private List<Vector3Int> drinkResult = new List<Vector3Int>();

    PlacementManager placementManager;
    public List<Vector3Int> GetFoodResult()
    {
        return foodResult;
    }
    public List<Vector3Int> GetDrinkResult()
    {
        return drinkResult;
    }


    public SearchViewDistance(ref List<Vector3Int> food, ref List<Vector3Int> water, PlacementManager _placementManager)
    {
        discoveredFood = food;
        discoveredDrink = water;
        placementManager = _placementManager;
    }

    public void SearchInViewDistance(float ViewDistance, Vector3 Position)
    {
        foodResult.Clear();
        drinkResult.Clear();
        Queue<Vector3Int> queue = new Queue<Vector3Int>();
        HashSet<Vector3Int> visited = new HashSet<Vector3Int>();

        Vector3Int startPosition = Vector3Int.RoundToInt(Position);
        queue.Enqueue(startPosition);
        visited.Add(startPosition);

        while (queue.Count > 0)
        {
            Vector3Int current = queue.Dequeue();
            if (placementManager.GetTypeOfPosition(current) == CellType.Nature)
            {
                foodResult.Add(current);
                if (!discoveredFood.Contains(current))
                {
                    discoveredFood.Add(current);
                }
            }
            if (placementManager.GetTypeOfPosition(current) == CellType.Water)
            {
                drinkResult.Add(current);
                if (!discoveredDrink.Contains(current))
                {
                    discoveredDrink.Add(current);
                }
            }

            if (Vector3Int.Distance(startPosition, current) <= ViewDistance)
            {
                foreach (Vector3Int neighbor in GetNeighbors(current))
                {
                    if (!visited.Contains(neighbor))
                    {
                        queue.Enqueue(neighbor);
                        visited.Add(neighbor);
                    }
                }
            }
        }
    }

    private List<Vector3Int> GetNeighbors(Vector3Int position)
    {
        List<Vector3Int> neighbors = new List<Vector3Int>
        {
            position + new Vector3Int(1, 0, 0),
            position + new Vector3Int(-1, 0, 0),
            position + new Vector3Int(0, 0, 1),
            position + new Vector3Int(0, 0, -1),
            position + new Vector3Int(1, 0, 1),
            position + new Vector3Int(1, 0, -1),
            position + new Vector3Int(-1, 0, 1),
            position + new Vector3Int(-1, 0, -1)
        };

        neighbors.RemoveAll(neighbor => !placementManager.CheckIfPositionInBound(neighbor));

        return neighbors;
    }


}

