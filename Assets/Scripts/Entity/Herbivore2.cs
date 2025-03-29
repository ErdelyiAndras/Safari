using UnityEngine;

public class Herbivore2 : Animal
{
    public Herbivore2(GameObject prefab, PlacementManager _placementManager, Herd parent) : base(prefab, _placementManager, parent)
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
