using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public abstract class CarnivoreBase : Animal
{
    public CarnivoreBase(GameObject prefab, PlacementManager _placementManager, Guid parentId, AnimalType type) : base(prefab, _placementManager, parentId, type)
    {
    }

    public CarnivoreBase(CarnivoreData data, PlacementManager placementManager, GameObject prefab, GameObject parent) : base(data, placementManager, prefab, parent)
    {
        LoadData(data, placementManager);
    }

    protected override void MoveToFood() => MoveToTarget(((CarnivoreSearchInRange)discoverEnvironment).GetClosestFood);

    protected override void ArrivedAtFood(CellType? targetType = null)
    {
        if (((CarnivoreSearchInRange)discoverEnvironment).PreyGuid == Guid.Empty)
        {
            return;
        }
        IEnumerable<Guid> animalsInClosestHerd = ((CarnivoreSearchInRange)discoverEnvironment).ClosestHerd?.Animals.Select(animal => animal.Id);
        if (animalsInClosestHerd == null)
        {
            return;
        }

        bool isPreyDead = true;
        
        foreach (Guid id in ((CarnivoreSearchInRange)discoverEnvironment).ClosestHerd?.Animals.Select(animal => animal.Id))
        {
            if (id == ((CarnivoreSearchInRange)discoverEnvironment).PreyGuid)
            {
                isPreyDead = false;
                break;
            }
        }
        if (isPreyDead)
        {
            MyState = State.Eating;
            targetPosition = Position;
        }
        else
        {
            Animal prey = ((CarnivoreSearchInRange)discoverEnvironment).ClosestHerd.Animals.FirstOrDefault(animal => animal.Id == ((CarnivoreSearchInRange)discoverEnvironment).PreyGuid);
            prey.DamageTaken(state.Damage);
            ArrivedAtFood();
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
        return new CarnivoreData(
            Id, spawnPosition, Position, ObjectInstance.transform.rotation, baseMoveSpeed, baseRotationSpeed,
            (CarnivoreSearchInRange)discoverEnvironment, MyState, state, targetPosition, myHerd, callOnceFlag, targetCorrection, elapsedTime
        );
    }

    public override void LoadData(EntityData data, PlacementManager placementManager)
    {
        base.LoadData(data, placementManager);
        discoverEnvironment = ((CarnivoreData)data).DiscoverEnvironment(placementManager);
    }
}
