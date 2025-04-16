using System;
using UnityEngine;

public abstract class HerbivoreBase : Animal
{
    public HerbivoreBase(GameObject prefab, PlacementManager _placementManager, Guid parentID, AnimalType type) : base(prefab, _placementManager, parentID, type)
    {
    }

    public HerbivoreBase(HerbivoreData data, PlacementManager placementManager, GameObject prefab, GameObject parent) : base(data, placementManager, prefab, parent)
    {
        LoadData(data, placementManager);
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

    public override EntityData SaveData()
    {
        return new HerbivoreData(
            Id, spawnPosition, Position, ObjectInstance.transform.rotation, baseMoveSpeed, baseRotationSpeed,
            (HerbivoreSearchInRange)discoverEnvironment, MyState, state, targetPosition, myHerd, callOnceFlag, targetCorrection, elapsedTime
        );
    }

    public override void LoadData(EntityData data, PlacementManager placementManager)
    {
        base.LoadData(data, placementManager);
        discoverEnvironment = ((HerbivoreData)data).DiscoverEnvironment(placementManager);
    }
}
