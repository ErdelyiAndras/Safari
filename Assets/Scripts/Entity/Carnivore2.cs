using UnityEngine;

public class Carnivore2 : Animal
{
    public Carnivore2(GameObject prefab, PlacementManager _placementManager) : base(prefab, _placementManager)
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
