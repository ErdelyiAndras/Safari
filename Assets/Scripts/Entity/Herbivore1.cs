using UnityEngine;
using System;

public class Herbivore1 : HerbivoreBase
{
    public Herbivore1(GameObject prefab, PlacementManager _placementManager, Guid parentId) : base(prefab, _placementManager, parentId, AnimalType.Herbivore1)
    {
        AnimalType type = AnimalType.Herbivore1;
        discoverEnvironment = new HerbivoreSearchInRange(Constants.AnimalVisionRange[type], placementManager, Constants.AnimalViewExtenderScale[type]);
    }
}
