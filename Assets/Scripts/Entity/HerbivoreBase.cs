using System.Collections.Generic;
using UnityEngine;

public class HerbivoreBase : Animal
{
    public HerbivoreBase(GameObject prefab, PlacementManager _placementManager, Herd parent, AnimalType type) : base(prefab, _placementManager, parent, type)
    {
    }
    protected override void MoveToFood() => MoveToTarget(((HerbivoreSearchInRange)discoverEnvironment).GetClosestFood);

    protected override void ArrivedAtFood(CellType? targetType = null)
    {
        if (targetType == CellType.Nature)
        {
            MyState = State.Eating;
        }
    }
}
