using UnityEngine;

public class Herbivore1 : HerbivoreBase
{
    public Herbivore1(GameObject prefab, PlacementManager _placementManager, Herd parent) : base(prefab, _placementManager, parent, AnimalType.Herbivore1)
    {
        AnimalType type = AnimalType.Herbivore1;
        discoverEnvironment = new HerbivoreSearchInRange(Constants.AnimalVisionRange[type], placementManager, Constants.AnimalViewExtenderScale[type]);
    }

    public Herbivore1(HerbivoreData data, PlacementManager placementManager, GameObject prefab, GameObject parent) : base(data, placementManager, prefab, parent)
    {

    }
}
