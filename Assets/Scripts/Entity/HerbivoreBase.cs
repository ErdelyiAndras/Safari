using System.Collections.Generic;
using UnityEngine;

public class HerbivoreBase : Animal
{
    List<Vector3Int> discoveredFood;
    public HerbivoreBase(GameObject prefab, PlacementManager _placementManager, Herd parent, AnimalType type) : base(prefab, _placementManager, parent, type)
    {
        discoveredFood = new List<Vector3Int>();
        discoverEnvironment = new SearchViewDistance(ref discoveredFood, ref discoveredDrink, placementManager);
        visionRange = 6.0f;
    }
    protected override void MoveToFood()
    {
        MoveToTarget(discoverEnvironment.GetFoodResult, discoveredFood);
    }
    protected override void ArrivedAtFood(CellType? targetType = null)
    {
        if (targetType == CellType.Nature)
        {
            MyState = State.Eating;
        }
    }
}
