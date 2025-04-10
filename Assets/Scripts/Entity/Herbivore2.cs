using UnityEngine;

public class Herbivore2 : HerbivoreBase
{
    public Herbivore2(GameObject prefab, PlacementManager _placementManager, Herd parent) : base(prefab, _placementManager, parent, AnimalType.Herbivore2)
    {
        AnimalType type = AnimalType.Herbivore2;
        discoverEnvironment = new HerbivoreSearchInRange(Constants.AnimalVisionRange[type], placementManager, Constants.AnimalViewExtenderScale[type]);
    }
}
