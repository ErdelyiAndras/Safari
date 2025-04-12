using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class CarnivoreBase : Animal
{
    public CarnivoreBase(GameObject prefab, PlacementManager _placementManager, Guid parentId, AnimalType type) : base(prefab, _placementManager, parentId, type)
    {
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


}
