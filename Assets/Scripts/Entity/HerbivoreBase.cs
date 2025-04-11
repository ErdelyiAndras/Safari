using System.Collections.Generic;
using UnityEngine;

public abstract class HerbivoreBase : Animal
{
    public HerbivoreBase(GameObject prefab, PlacementManager _placementManager, Herd parent, AnimalType type) : base(prefab, _placementManager, parent, type)
    {
    }

    public HerbivoreBase(HerbivoreData data) : base(data)
    {
        LoadData(data);
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

    public override EntityData SaveData()
    {
        return new HerbivoreData(
            Id, spawnPosition, placementManager, (HerbivoreSearchInRange)discoverEnvironment, Position, entityInstance.transform.rotation,
            MyState, state, targetPosition, myHerd, callOnceFlag, elapsedTime
        );
    }

    public override void LoadData(EntityData data)
    {
        base.LoadData(data);
        discoverEnvironment = ((HerbivoreData)data).DiscoverEnvironment;
    }
}
