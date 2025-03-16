using UnityEngine;
using System.Collections.Generic;


public class MapGenerator : MonoBehaviour
{
    public PlacementManager placementManager;
    public GameObject[] naturePrefabs;
    public GameObject waterPrefab;

    private Dictionary<Vector3Int, CellType> usedPositions = new Dictionary<Vector3Int, CellType>();

    private System.Random random = new System.Random();
    private void Start()
    {
        for (int i = 0; i < 6; ++i)
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
        }
    }

}

