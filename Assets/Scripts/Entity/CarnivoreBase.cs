using System.Collections.Generic;
using UnityEngine;

public class CarnivoreBase : Animal
{
    public CarnivoreBase(GameObject prefab, PlacementManager _placementManager, Herd parent, AnimalManager manager, AnimalType type) : base(prefab, _placementManager, parent, manager, type)
    {
    }

    protected override void MoveToFood()
    {
        throw new System.NotImplementedException();
    }

    protected override void DiscoverEnvironment()
    {
        List<Vector3Int> inViewDistance = SearchInViewDistance();
        foreach (Vector3Int position in inViewDistance)
        {
            if (placementManager.GetTypeOfPosition(position) == CellType.Water && !discoveredDrink.Contains(position))
            {
                discoveredDrink.Add(position);
            }
        }
    }
}
