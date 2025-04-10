using System.Collections.Generic;
using UnityEngine;

public class Carnivore2 : CarnivoreBase
{
    public Carnivore2(GameObject prefab, PlacementManager _placementManager, Herd parent, List<Herd> herds) : base(prefab, _placementManager, parent, AnimalType.Carnivore2, herds)
    {

    }
}
