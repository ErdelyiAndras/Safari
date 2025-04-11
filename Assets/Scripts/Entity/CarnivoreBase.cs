using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public abstract class CarnivoreBase : Animal
{
    public CarnivoreBase(GameObject prefab, PlacementManager _placementManager, Herd parent, AnimalType type, List<Herd> herds) : base(prefab, _placementManager, parent, type)
    {
    }

    public CarnivoreBase(CarnivoreData data) : base(data)
    {
        LoadData(data);
    }

    protected override void MoveToFood() => MoveToTarget(((CarnivoreSearchInRange)discoverEnvironment).GetClosestFood);

    protected override void ArrivedAtFood(CellType? targetType = null)
    {
        if (((CarnivoreSearchInRange)discoverEnvironment).PreyGuid == Guid.Empty)
        {
            return;
        }
        bool isPreyDead = true;
        foreach (Guid id in ((CarnivoreSearchInRange)discoverEnvironment).ClosestHerd.Animals.Select(animal => animal.Id))
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

    public override EntityData SaveData()
    {
        return new CarnivoreData(
            Id, spawnPosition, placementManager, (CarnivoreSearchInRange)discoverEnvironment, Position, entityInstance.transform.rotation,
            MyState, state, targetPosition, myHerd, callOnceFlag, elapsedTime
        );
    }

    public override void LoadData(EntityData data)
    {
        base.LoadData(data);
        discoverEnvironment = ((CarnivoreData)data).DiscoverEnvironment;
    }
}
