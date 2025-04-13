using System.Collections.Generic;
using System;
using UnityEngine;

public class Carnivore1 : CarnivoreBase
{
    public Carnivore1(GameObject prefab, PlacementManager _placementManager, Guid parentId) : base(prefab, _placementManager, parentId, AnimalType.Carnivore1) 
    {
        AnimalType type = AnimalType.Carnivore1;
        discoverEnvironment = new CarnivoreSearchInRange(Constants.AnimalVisionRange[type], placementManager, Constants.AnimalViewExtenderScale[type]);
    }

    public Carnivore1(CarnivoreData data, PlacementManager placementManager, GameObject prefab, GameObject parent) : base(data, placementManager, prefab, parent)
    {

    }
}
