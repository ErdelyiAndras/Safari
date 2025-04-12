using System.Collections.Generic;
using System;
using UnityEngine;

public class Carnivore2 : CarnivoreBase
{
    public Carnivore2(GameObject prefab, PlacementManager _placementManager, Guid parentId) : base(prefab, _placementManager, parentId, AnimalType.Carnivore2)
    {
        AnimalType type = AnimalType.Carnivore2;
        discoverEnvironment = new CarnivoreSearchInRange(Constants.AnimalVisionRange[type], placementManager, Constants.AnimalViewExtenderScale[type]);
    }
}
