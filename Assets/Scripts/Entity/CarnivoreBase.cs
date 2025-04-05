using System.Collections.Generic;
using UnityEngine;

public class CarnivoreBase : Animal
{
    public CarnivoreBase(GameObject prefab, PlacementManager _placementManager, Herd parent, AnimalType type) : base(prefab, _placementManager, parent, type)
    {
        List<Vector3Int> empty = new List<Vector3Int>();
        discoverEnvironment = new SearchViewDistance(ref empty, ref discoveredDrink, placementManager);
    }

    protected override void MoveToFood()
    {
        throw new System.NotImplementedException();
    }

}
