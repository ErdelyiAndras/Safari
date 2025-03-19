using UnityEngine;
using System.Collections.Generic;
using UnityEngine.UIElements;


public class MapGenerator : MonoBehaviour
{
    public PlacementManager placementManager;
    public GameObject[] naturePrefabs;
    public GameObject waterPrefab;
    public GameObject hillPrefab;
    public GameObject deadEnd;

    private Dictionary<Vector3Int, CellType> usedPositions = new Dictionary<Vector3Int, CellType>();

    private System.Random random = new System.Random();
    private void Start()
    {
        GenerateRoads();

        for (int i = 0; i < random.Next(2, 5); ++i)
        {
            GenerateHill();
        }

        for (int i = 0; i < random.Next(2, 9); ++i)
        {
            GenerateRiver();
        }

        GenerateNature();
        PlaceStructures();
    }

    private void GenerateNature()
    {
        for (int i = 0; i < placementManager.width * placementManager.height / 3; ++i)
        {
            Vector3Int position = GetRandomPosition();
            if (usedPositions.ContainsKey(position))
            {
                i--;
                continue;
            }
            else
            {
                usedPositions.Add(position, CellType.Nature);
            }

        }
    }

    private void GenerateRiver()
    {
        Vector3Int position = GetRandomPosition();
        int length = random.Next(10, 40);
        int width = random.Next(10, 40);
        Vector3Int position2 = position + new Vector3Int(length, 0, width);
        List<Vector3Int> river = placementManager.GetPathBetween(position, position2);
        while (river.Count == 0 || !placementManager.CheckIfPositionInBound(position2))
        {
            position = GetRandomPosition();
            length = random.Next(5, 20);
            width = random.Next(5, 20);
            position2 = position + new Vector3Int(length, 0, width);
            river = placementManager.GetPathBetween(position, position2);
        }
        foreach (var water in river)
        {
            if (!usedPositions.ContainsKey(water))
                usedPositions.Add(water, CellType.Water);
        }
    }


    private Vector3Int GetRandomPosition()
    {
        return new Vector3Int(random.Next(0, placementManager.width), 0, random.Next(0, placementManager.height));
    }

    private void PlaceStructures()
    {
        foreach (var position in usedPositions)
        {
            if (position.Value == CellType.Nature)
            {
                placementManager.PlaceStructure(position.Key, naturePrefabs[random.Next(0, naturePrefabs.Length)], CellType.Nature);
            }
            else if (position.Value == CellType.Water)
            {
                placementManager.PlaceStructure(position.Key, waterPrefab, CellType.Water);
            }
            else if (position.Value == CellType.Hill)
            {
                 placementManager.PlaceStructure(position.Key, hillPrefab, CellType.Hill);
            }
        }
    }

    private void GenerateRoads()
    {
        placementManager.PlaceStructure(new Vector3Int(0, 0, 0), deadEnd, CellType.Road);
        usedPositions.Add(new Vector3Int(0, 0, 0), CellType.Road);
        placementManager.PlaceStructure(new Vector3Int(placementManager.width -1, 0, placementManager.height - 1), deadEnd, CellType.Road);
        usedPositions.Add(new Vector3Int(placementManager.width - 1, 0, placementManager.height - 1), CellType.Road);
    }

    private void GenerateHill()
    {
        List<Vector3> hillPositions = new List<Vector3>();

        int maxHillSize = random.Next(10, 30);

        Vector3Int startPosition;
        do
        {
            startPosition = GetRandomPosition();
        }
        while (usedPositions.ContainsKey(startPosition));

        Queue<Vector3Int> queue = new Queue<Vector3Int>();
        HashSet<Vector3Int> visited = new HashSet<Vector3Int>();

        queue.Enqueue(startPosition);
        visited.Add(startPosition);

        while (queue.Count > 0 && visited.Count < maxHillSize)
        {
            Vector3Int current = queue.Dequeue();
            usedPositions[current] = CellType.Hill;

            foreach (Vector3Int neighbor in GetNeighbors(current))
            {
                if (!visited.Contains(neighbor) && placementManager.CheckIfPositionInBound(neighbor))
                {
                    queue.Enqueue(neighbor);
                    visited.Add(neighbor);
                    hillPositions.Add(neighbor);
                }
            }
        }

        foreach (Vector3Int position in visited)
        {
            if (!usedPositions.ContainsKey(position))
            {
                usedPositions.Add(position, CellType.Hill);
            }
        }
    }

    private IEnumerable<Vector3Int> GetNeighbors(Vector3Int position)
    {
        yield return position + new Vector3Int(1, 0, 0);
        yield return position + new Vector3Int(0, 0, 1);
        yield return position + new Vector3Int(-1, 0, 0);
        yield return position + new Vector3Int(0, 0, -1);
    }
}
