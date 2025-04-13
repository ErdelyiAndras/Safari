using UnityEngine;
using System;

public class Herbivore2 : HerbivoreBase
{
    public Herbivore2(GameObject prefab, PlacementManager _placementManager, Guid parentId) : base(prefab, _placementManager, parentId, AnimalType.Herbivore2)
    {
        AnimalType type = AnimalType.Herbivore2;
        discoverEnvironment = new HerbivoreSearchInRange(Constants.AnimalVisionRange[type], placementManager, Constants.AnimalViewExtenderScale[type]);
    }

    public Herbivore2(HerbivoreData data, PlacementManager placementManager, GameObject prefab, GameObject parent) : base(data, placementManager, prefab, parent)
    {

    }
}
