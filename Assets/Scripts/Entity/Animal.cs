using System;
using System.Collections.Generic;
using UnityEngine;
public enum AnimalType
{
    Herbivore1,
    Herbivore2,
    Carnivore1,
    Carnivore2
}

public abstract class Animal : Entity
{
    public enum State
    {
        Resting,
        Moving,
        SearchingForFood,
        SearchingForWater,
        Eating,
        Drinking,
        Mating
    }
    public Action<Animal> AnimalDied;
    public AnimalInternalState state;
    protected Vector3 targetPosition;
    public Herd myHerd;
    protected bool callOnceFlag;
    protected float elapsedTime = 0.0f;
    public float Health { get => (state.Hunger * state.Thirst * state.RemainingLifetime * state.Health); }
    public AnimalType Type { get { return state.type; } }

    public State MyState { get; protected set; }

    public Animal(GameObject prefab, PlacementManager _placementManager, Herd parent, AnimalType _type)
    {
        Id = Guid.NewGuid();
        myHerd = parent;
        placementManager = _placementManager;
        spawnPosition = parent.Spawnpoint;
        state = new AnimalInternalState(_type);
        targetPosition = spawnPosition;
        SpawnEntity(prefab, parent.gameObject.transform);
        MyState = State.Moving;
        baseMoveSpeed = Constants.AnimalBaseMoveSpeed[_type];
        baseRotationSpeed = Constants.AnimalBaseRotationSpeed[_type];
    }

    public Animal(AnimalData data)
    {
        LoadData(data);
    }

    protected bool IsAnimalDead() => Health <= 0; // ez így nem mûködik pl: hunger és thirst is negatív akkor még nem halt meg

    public override void CheckState()
    {   
        Debug.Log(MyState.ToString());
        switch(MyState)
        {
            case State.Resting:
                WaitAction(state.RestTime, State.Moving);
                break;
            case State.Moving:
                Move();
                break;
            case State.SearchingForFood:
                MoveToFood();
                break;
            case State.SearchingForWater:
                MoveToWater();
                break;
            case State.Eating:
                WaitAction(state.EatingTime, ref state.Hunger, state.MaxFood, state.FoodNutrition, State.Resting);
                break;
            case State.Drinking:

                WaitAction(state.DrinkingTime, ref state.Thirst, state.MaxDrink, state.DrinkNutrition, State.Resting);
                break;
            case State.Mating:
                WaitAction(state.RestTime, State.Moving);
                break;
        }
    }

    private void WaitAction(float duration, ref float toAdvance, float maxvalue, float advanceStep, State changeStateTo)
    {
        if (elapsedTime < duration)
        {
            elapsedTime += Time.deltaTime;
        }
        else
        {
            
            toAdvance += toAdvance < maxvalue ? advanceStep : 0.0f;
            if ( MyState == State.Eating && Health < state.MaxHealth)
            {
                state.Health += (state.MaxHealth - state.Health)/2;
            }
            MyState = changeStateTo;
            elapsedTime = 0.0f;
        }
    }

    private void WaitAction(float duration, State changeStateTo)
    {
        if (elapsedTime < duration)
        {
            elapsedTime += Time.deltaTime;
        }
        else
        {
            MyState = changeStateTo;
            elapsedTime = 0.0f;
        }
    }

    public virtual void MatureAnimal()
    {
        state.RemainingLifetime--;
        state.Hunger--;
        state.Thirst--;
        if (IsAnimalDead())
        {
            AnimalDies();
        }
        if (MyState != State.Eating && MyState != State.Drinking && MyState != State.Mating)
        {
            if (state.Hunger < state.MaxFood * state.FoodThreshold && MyState != State.SearchingForWater)
            {
                MyState = State.SearchingForFood;
            }
            if (state.Thirst < state.MaxDrink * state.DrinkThreshold && MyState != State.SearchingForFood)
            {
                MyState = State.SearchingForWater;
            }
            if (state.Hunger < state.MaxFood * state.FoodThreshold && state.Thirst < state.MaxDrink * state.DrinkThreshold)
            {
                if (state.Hunger / state.FoodThreshold < state.Thirst / state.DrinkThreshold)
                {
                    MyState = State.SearchingForFood;
                }
                else
                {
                    MyState = State.SearchingForWater;

                }
            }
        }
    }
    protected void AnimalDies()
    {
        Debug.Log("Animal dies");
        UnityEngine.Object.Destroy(entityInstance);
        AnimalDied?.Invoke(this);
    }

    abstract protected void MoveToFood();
    private void MoveToWater()
    {
        discoverEnvironment.SearchInViewDistance(Position);
        MoveToTarget((_) => ((AnimalSearchInRange)discoverEnvironment).GetClosestWater(_, myHerd.Spawnpoint, myHerd.DistributionRadius));
    }
    protected void MoveToTarget(Func<Vector3, Vector3?> getClosestFunction)
    {
        Vector3? target = getClosestFunction(Position);
        
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
        if (placementManager.GetTypeOfPosition(Vector3Int.RoundToInt(targetPosition)) == CellType.Water)
        {
            /*
            List<Vector3Int> neighbours = placementManager.GetNeighboursOfType(Vector3Int.RoundToInt(targetPosition), CellType.Empty);
            if (neighbours.Count > 0)
            {
                neighbours.Sort((a, b) => Vector3Int.Distance(Vector3Int.RoundToInt(Position), a).CompareTo(Vector3Int.Distance(Vector3Int.RoundToInt(Position), b)));
                targetPosition = (neighbours[0] + targetPosition) / 2.0f;
            }*/
        }
        Move();
    }

    protected override void Move()
    {
        if (Vector3.Distance(Position, targetPosition) < 0.1f)
        {
            ObjectArrived();
        }
        else
        {
            Position = Vector3.MoveTowards(Position, targetPosition, MoveSpeed * Time.deltaTime);
            Vector3 direction = targetPosition - Position;
            DiscoverEnvironment();
            if (direction != Vector3.zero)
            {
                // Szabadon tudjon mozogni, de vízre ne menjen --> 
                Quaternion targetRotation = Quaternion.LookRotation(direction);
                entityInstance.transform.rotation = Quaternion.Slerp(entityInstance.transform.rotation, targetRotation, RotationSpeed * Time.deltaTime);
            }
        }
    }

    private void ObjectArrived()
    {
        CellType targetType = placementManager.GetTypeOfPosition(Vector3Int.RoundToInt(targetPosition));
        callOnceFlag = false;
        SetRandomTargetPosition();
        switch (MyState) 
        {
            case State.Moving:
                MyState = State.Resting;
                break;
            case State.SearchingForFood:
                ArrivedAtFood(targetType);
                break;
            case State.SearchingForWater:
                if (targetType == CellType.Water)
                {
                    MyState = State.Drinking;
                }
                break;
            default:
                break;
        }
    }

    protected abstract void ArrivedAtFood(CellType? targetType = null);
    protected void DiscoverEnvironment() => discoverEnvironment.SearchInViewDistance(Position);

    protected void SetRandomTargetPosition(bool inHerd = true)
    {
        float randomX, randomZ; ;
        bool xDirection, zDirection;
        Vector3 temporatyPosition;
        do
        {
            if (inHerd)
            {
                randomX = UnityEngine.Random.Range(0, myHerd.DistributionRadius + 1);
                randomZ = UnityEngine.Random.Range(0, myHerd.DistributionRadius + 1);
                xDirection = UnityEngine.Random.Range(0, 2) == 0;
                zDirection = UnityEngine.Random.Range(0, 2) == 0;
                randomX = xDirection ? randomX : -randomX;
                randomZ = zDirection ? randomZ : -randomZ;
                temporatyPosition = new Vector3(Position.x + randomX, 0, Position.z + randomZ);
            }
            else
            {
                randomX = UnityEngine.Random.Range(0, placementManager.width);
                randomZ = UnityEngine.Random.Range(0, placementManager.height);
                temporatyPosition = new Vector3(randomX, 0, randomZ);
            }
        } while (!placementManager.CheckIfPositionInBound(Vector3Int.RoundToInt(temporatyPosition)) || !placementManager.IsPositionWalkable(Vector3Int.RoundToInt(temporatyPosition)));
        targetPosition = temporatyPosition;
    }

    public void DamageTaken(float damageAmout)
    {
        state.Health -= damageAmout;
        if (IsAnimalDead())
        {
            AnimalDies();
        }
    }

    public override void LoadData(EntityData data)
    {
        base.LoadData(data);
        MyState = ((AnimalData)data).MyState;
        state = ((AnimalData)data).State;
        targetPosition = ((AnimalData)data).TargetPosition;
        myHerd = ((AnimalData)data).MyHerd;
        callOnceFlag = ((AnimalData)data).CallOnceFlag;
        elapsedTime = ((AnimalData)data).ElapsedTime;
    }
}