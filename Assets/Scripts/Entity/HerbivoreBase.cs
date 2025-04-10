using System.Collections.Generic;
using UnityEngine;

public class HerbivoreBase : Animal
{
    List<Vector3Int> discoveredFood;
    public HerbivoreBase(GameObject prefab, PlacementManager _placementManager, Herd parent, AnimalType type) : base(prefab, _placementManager, parent, type)
    {
        discoveredFood = new List<Vector3Int>();
        discoverEnvironment = new HerbivoreSearchInRange(6.0f, placementManager, 2.0f);
        baseMoveSpeed = 1.5f;
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
