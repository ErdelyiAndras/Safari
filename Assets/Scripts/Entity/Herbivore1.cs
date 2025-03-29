using UnityEngine;

public class Herbivore1 : Animal
{
    public Herbivore1(GameObject prefab, PlacementManager _placementManager, Herd parent) : base(prefab, _placementManager, parent)
    {

    }
    protected override void Drink()
    {
        throw new System.NotImplementedException();
    }

    protected override void Eat()
    {
        throw new System.NotImplementedException();
    }
}
