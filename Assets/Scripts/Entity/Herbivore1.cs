using UnityEngine;

public class Herbivore1 : HerbivoreBase
{
    public Herbivore1(GameObject prefab, PlacementManager _placementManager, Herd parent, AnimalManager manager) : base(prefab, _placementManager, parent, manager, AnimalType.Herbivore1)
    {

    }
}
