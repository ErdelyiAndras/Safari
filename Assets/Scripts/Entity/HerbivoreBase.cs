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

    protected override void Move()
    {
        if (Vector3.Distance(Position, targetPosition) < 0.1f)
        {
            ObjectArrived();
        }
        else
        {
            if (placementManager.GetTypeOfPosition(Vector3Int.RoundToInt(Position)) == CellType.Hill)
            {
                Position = new Vector3(Position.x, 0.65f, Position.z);
            }
            else if (Position.y != 0)
            {
                Position = new Vector3(Position.x, Position.y / 6.0f, Position.z);
            }
            if (placementManager.GetTypeOfPosition(Vector3Int.RoundToInt(targetPosition)) == CellType.Hill)
            {
                targetPosition = new Vector3(targetPosition.x, 0.65f, targetPosition.z);
            }
            Position = Vector3.MoveTowards(Position, targetPosition, MoveSpeed * SlowingTerrain * Time.deltaTime);
            Vector3 direction = targetPosition - Position;
            //direction.y = 0;
            DiscoverEnvironment();
            if (direction != Vector3.zero)
            {
                Quaternion targetRotation = Quaternion.LookRotation(direction);
                ObjectInstance.transform.rotation = Quaternion.Slerp(ObjectInstance.transform.rotation, targetRotation, RotationSpeed * Time.deltaTime);
            }
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
