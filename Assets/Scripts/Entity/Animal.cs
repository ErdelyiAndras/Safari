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
    public Guid myHerd;
    protected bool callOnceFlag, targetCorrection;
    protected float elapsedTime = 0.0f;
    public float Health { get => (state.Hunger * state.Thirst * state.RemainingLifetime * state.Health); }
    public AnimalType Type { get { return state.type; } }

    public State MyState { get; protected set; }


    public Animal(GameObject prefab, PlacementManager _placementManager, Guid parentID, AnimalType _type) : base(_placementManager)
    {
        myHerd = parentID;
        spawnPosition = placementManager.GetTypeOfPosition(Vector3Int.RoundToInt(GetMyHerd.Position)) == CellType.Hill ? new Vector3(GetMyHerd.Position.x, 0.65f, GetMyHerd.Position.z) : GetMyHerd.Position;
        state = new AnimalInternalState(_type);
        targetPosition = spawnPosition;
        SpawnEntity(prefab, GetMyHerd.ObjectInstance.transform);
        MyState = State.Moving;
        baseMoveSpeed = Constants.AnimalBaseMoveSpeed[_type];
        baseRotationSpeed = Constants.AnimalBaseRotationSpeed[_type];
    }

    public Animal(AnimalData data, PlacementManager placementManager, GameObject prefab, GameObject parent) : base(data, placementManager, prefab, parent)
    {
        LoadData(data, placementManager);
    }

    protected bool IsAnimalDead() => Health <= 0;
    private float SlowingTerrain { get => (placementManager.GetTypeOfPosition(Vector3Int.RoundToInt(Position)) == CellType.Water || placementManager.GetTypeOfPosition(Vector3Int.RoundToInt(Position)) == CellType.Hill ? 0.3f : 1.0f); }
    
    public Herd GetMyHerd 
    {
        get
        {
            return placementManager.PlacedObjects.GetMyHerd(myHerd);
        }
    }
    public override void CheckState()
    {   
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
            elapsedTime += Time.deltaTime * SpeedMultiplier;
        }
        else
        {
            
            toAdvance += (toAdvance + advanceStep) < maxvalue ? advanceStep : maxvalue - toAdvance;
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
            elapsedTime += Time.deltaTime * SpeedMultiplier;
        }
        else
        {
            MyState = changeStateTo;
            elapsedTime = 0.0f;
        }
    }

    public virtual void MatureAnimal()
    {
        Math.Clamp(--state.RemainingLifetime, 0, 100);
        Math.Clamp(--state.Hunger, 0, 100);
        Math.Clamp(--state.Thirst, 0, 100);
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
    public void AnimalDies()
    {
        UnityEngine.Object.Destroy(ObjectInstance);
        AnimalDied?.Invoke(this);
    }

    abstract protected void MoveToFood();
    private void MoveToWater()
    {
        discoverEnvironment.SearchInViewDistance(Position);
        MoveToTarget((_) => ((AnimalSearchInRange)discoverEnvironment).GetClosestWater(_, GetMyHerd.Position, GetMyHerd.DistributionRadius));
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
            List<Vector3Int> neighbours = placementManager.GetNeighboursOfType(Vector3Int.RoundToInt(targetPosition), CellType.Empty);
            if (neighbours.Count > 0)
            {
                neighbours.Sort((a, b) => Vector3Int.Distance(Vector3Int.RoundToInt(Position), a).CompareTo(Vector3Int.Distance(Vector3Int.RoundToInt(Position), b)));
                targetPosition = (neighbours[0] + targetPosition) / 2.0f;
                targetCorrection = true;
            }
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
            if(placementManager.GetTypeOfPosition(Vector3Int.RoundToInt(Position)) == CellType.Hill)
            {
                Position = new Vector3(Position.x, 0.65f, Position.z);
            }
            else if (Position.y != 0)
            {
                Position = new Vector3(Position.x, Position.y/6.0f, Position.z);
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
                if (targetType == CellType.Water || targetCorrection)
                {
                    MyState = State.Drinking;
                    targetCorrection = false;
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
                randomX = UnityEngine.Random.Range(0, GetMyHerd.DistributionRadius + 1);
                randomZ = UnityEngine.Random.Range(0, GetMyHerd.DistributionRadius + 1);
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

    public override void LoadData(EntityData data, PlacementManager placementManager)
    {
        base.LoadData(data, placementManager);
        MyState = ((AnimalData)data).MyState;
        state = ((AnimalData)data).State;
        targetPosition = ((AnimalData)data).TargetPosition;
        myHerd = ((AnimalData)data).MyHerd;
        callOnceFlag = ((AnimalData)data).CallOnceFlag;
        targetCorrection = ((AnimalData)data).TargetCorrection;
        elapsedTime = ((AnimalData)data).ElapsedTime;
    }
}