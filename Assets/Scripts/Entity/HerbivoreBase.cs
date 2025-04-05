using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;

public class HerbivoreBase : Animal
{
    List<Vector3Int> discoveredFood;
    public HerbivoreBase(GameObject prefab, PlacementManager _placementManager, Herd parent, AnimalType type) : base(prefab, _placementManager, parent, type)
    {
        discoveredFood = new List<Vector3Int>();
    }
    protected override void MoveToFood()
    {
        List<Vector3Int> foodInviewDistance = SearchInViewDistance();
        if (foodInviewDistance.Count == 1)
        {
            targetPosition = (Vector3)foodInviewDistance[0];
        }
        else if (foodInviewDistance.Count > 1)
        {
            Vector3Int? closestWithinHerd = null;
            float closestDistance = float.MaxValue;
            foreach(Vector3Int position in foodInviewDistance)
            {
                if (Vector3Int.Distance(myHerd.Spawnpoint, position) <= myHerd.DistributionRadius)
                {
                    if (Vector3Int.Distance(Vector3Int.RoundToInt(Position), position) < closestDistance)
                    {
                        closestWithinHerd = position;
                        closestDistance = Vector3Int.Distance(Vector3Int.RoundToInt(Position), position);
                    }
                }
            }
            if (closestWithinHerd != null) 
            {
                targetPosition = (Vector3)closestWithinHerd;
            }
        }
        else
        {
            discoveredFood.Sort((a, b) => Vector3Int.Distance(Vector3Int.RoundToInt(Position), a).CompareTo(Vector3Int.Distance(Vector3Int.RoundToInt(Position), b))); // TODO check if there is any in herd radius
            while (Vector3Int.Distance(Vector3Int.RoundToInt(Position), discoveredFood[0]) <= ViewDistance)
            {
                discoveredFood.RemoveAt(0);
            }
            foreach (Vector3Int position in discoveredFood)
            {
                targetPosition = discoveredFood[0];
            }
        }
    }
    protected override void DiscoverEnvironment()
    {
        List<Vector3Int> inViewDistance = SearchInViewDistance();
        foreach (Vector3Int position in inViewDistance)
        {
            if (placementManager.GetTypeOfPosition(position) == CellType.Nature && !discoveredFood.Contains(position))
            {
                discoveredFood.Add(position);
            }
            if (placementManager.GetTypeOfPosition(position) == CellType.Water && !discoveredDrink.Contains(position))
            {
                discoveredDrink.Add(position);
            }

        }
    }
}
