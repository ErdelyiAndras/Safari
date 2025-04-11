using System.Collections.Generic;
using UnityEngine;

public class Carnivore1 : CarnivoreBase
{
    public Carnivore1(GameObject prefab, PlacementManager _placementManager, Herd parent, List<Herd> herds) : base(prefab, _placementManager, parent, AnimalType.Carnivore1, herds) 
    {
        AnimalType type = AnimalType.Carnivore1;
        discoverEnvironment = new CarnivoreSearchInRange(Constants.AnimalVisionRange[type], placementManager, Constants.AnimalViewExtenderScale[type], herds);
    }

    public Carnivore1(CarnivoreData data) : base(data)
    {

    }
}
