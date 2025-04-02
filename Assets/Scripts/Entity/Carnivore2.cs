using UnityEngine;

public class Carnivore2 : CarnivoreBase
{
    public Carnivore2(GameObject prefab, PlacementManager _placementManager, Herd parent, AnimalManager manager) : base(prefab, _placementManager, parent, manager, AnimalType.Herbivore2)
    {

    }
}
