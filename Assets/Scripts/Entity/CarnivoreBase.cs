using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class CarnivoreBase : Animal
{
    private List<Herd> herds;
    private Guid preyGuid = Guid.Empty;
    Herd closestHerd;
    public CarnivoreBase(GameObject prefab, PlacementManager _placementManager, Herd parent, AnimalType type, List<Herd> herds) : base(prefab, _placementManager, parent, type)
    {
        List<Vector3Int> empty = new List<Vector3Int>();
        discoverEnvironment = new SearchViewDistance(ref empty, ref discoveredDrink, placementManager);
        visionRange = 12.0f;
        this.herds = herds;
        baseMoveSpeed = 3.0f;

    }

    protected override void MoveToFood()
    {   
        //the possibility is negligeble but theoratically possible that the closes animal is not the animal that is the closes from the closes herd
        //however this implementation is more efficient as it is not needed to examine every animal
        List<Herd> carnivoreHerds = herds.Where(h => h.animalTypesOfHerd == AnimalType.Herbivore1 || h.animalTypesOfHerd == AnimalType.Herbivore2).ToList();
        if (carnivoreHerds.Count != 0)
        {
            closestHerd = carnivoreHerds.OrderBy(h => Vector3Int.Distance(h.Spawnpoint, Vector3Int.RoundToInt(Position))).FirstOrDefault();
            closestHerd.Animals.Sort((a, b) => Vector3.Distance(Position, a.Position).CompareTo(Vector3.Distance(Position, b.Position)));
            //known issue: if between the count != 0 check and the next line the only animal dies, then it is an outofbounds exception
            //maybe putting it in a try catch will fix it
            try
            {
                if (Vector3.Distance(closestHerd.Animals[0].Position, Position) > ViewDistance)
                {
                    if (!callOnceFlag)
                    {
                        callOnceFlag = true;
                        SetRandomTargetPosition(false);
                    }
                }
                else
                {
                    preyGuid = closestHerd.Animals[0].Id;
                    targetPosition = closestHerd.Animals[0].Position;
                }
            } catch { }
        }
        else
        {
            if (!callOnceFlag)
            {
                callOnceFlag = true;
                SetRandomTargetPosition(false);
            }
        }
        Move();
    }
    protected override void ArrivedAtFood(CellType? targetType = null)
    {
        if (preyGuid == Guid.Empty)
        {
            return;
        }
        bool isPreyDead = true;
        foreach (Guid id in closestHerd.Animals.Select(animal => animal.Id))
        {
            if (id == preyGuid)
            {
                isPreyDead = false;
            }
        }
        if (isPreyDead)
        {
            MyState = State.Eating;
            targetPosition = Position;
        }
        else
        {
            Animal prey = closestHerd.Animals.FirstOrDefault(animal => animal.Id == preyGuid);
            prey.DamageTaken(state.Damage);
            ArrivedAtFood();
        }
    }


}
