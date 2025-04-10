using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class CarnivoreBase : Animal
{
    public CarnivoreBase(GameObject prefab, PlacementManager _placementManager, Herd parent, AnimalType type, List<Herd> herds) : base(prefab, _placementManager, parent, type)
    {
    }

    protected override void MoveToFood()
    {
        Vector3? target = ((CarnivoreSearchInRange)discoverEnvironment).GetClosestFood(Position);
        if (!callOnceFlag)
        {
            if (target == null)
            {
                callOnceFlag = true;
                SetRandomTargetPosition(false);
            }
            else
            {
                targetPosition = (Vector3)target;
            }
        }
        Move();
    }
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


}
