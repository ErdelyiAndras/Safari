using System;
using UnityEngine;

public class HerbivoreBase : Animal
{
    public HerbivoreBase(GameObject prefab, PlacementManager _placementManager, Guid parentID, AnimalType type) : base(prefab, _placementManager, parentID, type)
    {
    }
    protected override void MoveToFood()
    {
        discoverEnvironment.SearchInViewDistance(Position);
        MoveToTarget(_ => ((HerbivoreSearchInRange)discoverEnvironment).GetClosestFood(_, GetMyHerd.Position, GetMyHerd.DistributionRadius));
    }

    protected override void ArrivedAtFood(CellType? targetType = null)
    {
        if (targetType == CellType.Nature)
        {
            MyState = State.Eating;
        }
    }
}
