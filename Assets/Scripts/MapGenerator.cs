using UnityEngine;
using System.Collections.Generic;
using UnityEngine.UIElements;

public class MapGenerator : MonoBehaviour
{
    public int minHillAmount;
    public int maxHillAmount;
    public int minHillSize;
    public int maxHillSize;
    public int minRiverAmount;
    public int maxRiverAmount;
    public int natureDensity;


    public PlacementManager placementManager;
    public GameObject[] naturePrefabs;
    public GameObject waterPrefab;
    public GameObject hillPrefab;
    public GameObject deadEnd;

    private Dictionary<Vector3Int, CellType> usedPositions = new Dictionary<Vector3Int, CellType>();

    private System.Random random = new System.Random();
    private void Start()
    {
        if (natureDensity < 0)
        {
            natureDensity = 0;
        }
        else if (natureDensity > 100)
        {
            natureDensity = 100;
        }

        GenerateRoads();

        for (int i = 0; i < random.Next(minHillAmount, maxHillAmount); ++i)
        {
            GenerateHill();
        }

        for (int i = 0; i < random.Next(minRiverAmount, maxRiverAmount); ++i)
        {
            GenerateRiver();
        }

        GenerateNature();
        PlaceStructures();
    }

    private void GenerateNature()
    {
        for (int i = 0; i < placementManager.width * placementManager.height * natureDensity / 100; ++i)
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

        int hillSize = random.Next(minHillSize, maxHillSize);

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

        while (queue.Count > 0 && visited.Count < hillSize)
        {
            Vector3Int current = queue.Dequeue();
            usedPositions[current] = CellType.Hill;

            foreach (Vector3Int neighbor in GetNeighborsInRandomOrder(current))
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

    private IEnumerable<Vector3Int> GetNeighborsInRandomOrder(Vector3Int position)
    {
        List<Vector3Int> neighbors = new List<Vector3Int>
        {
            position + new Vector3Int(1, 0, 0),
            position + new Vector3Int(0, 0, 1),
            position + new Vector3Int(-1, 0, 0),
            position + new Vector3Int(0, 0, -1)

        };

        // shuffles the list
        for (int i = neighbors.Count - 1; i > 0; i--)
        {
            int j = random.Next(0, i + 1);
            Vector3Int temp = neighbors[i];
            neighbors[i] = neighbors[j];
            neighbors[j] = temp;
        }

        foreach (Vector3Int neighbor in neighbors)
        {
            yield return neighbor;
        }
    }
}
