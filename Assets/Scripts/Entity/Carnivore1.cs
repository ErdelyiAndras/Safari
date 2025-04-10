using System.Collections.Generic;
using UnityEngine;

public class Carnivore1 : CarnivoreBase
{
    public Carnivore1(GameObject prefab, PlacementManager _placementManager, Herd parent, List<Herd> herds) : base(prefab, _placementManager, parent, AnimalType.Carnivore1, herds) 
    {
    }
}
