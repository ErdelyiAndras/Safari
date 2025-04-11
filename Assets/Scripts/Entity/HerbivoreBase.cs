using System.Collections.Generic;
using UnityEngine;

public class HerbivoreBase : Animal
{
    public HerbivoreBase(GameObject prefab, PlacementManager _placementManager, Herd parent, AnimalType type) : base(prefab, _placementManager, parent, type)
    {
    }
    protected override void MoveToFood()
    {
        discoverEnvironment.SearchInViewDistance(Position);
        MoveToTarget(_ => ((HerbivoreSearchInRange)discoverEnvironment).GetClosestFood(_, myHerd.Spawnpoint, myHerd.DistributionRadius));
    }

    protected override void ArrivedAtFood(CellType? targetType = null)
    {
        if (targetType == CellType.Nature)
        {
            MyState = State.Eating;
        }
    }
}
