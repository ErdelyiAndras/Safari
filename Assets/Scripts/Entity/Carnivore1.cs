using UnityEngine;

public class Carnivore1 : CarnivoreBase
{
    public Carnivore1(GameObject prefab, PlacementManager _placementManager, Herd parent, AnimalManager manager) : base(prefab, _placementManager, parent, manager, AnimalType.Carnivore1)
    {

    }
}
