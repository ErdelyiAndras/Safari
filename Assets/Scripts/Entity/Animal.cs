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
    public Guid Id { get; }
    public Action<Animal> AnimalDied;
    protected List<Vector3Int> discoveredDrink;
    public AnimalInternalState state;
    protected Vector3 targetPosition;
    public Herd myHerd;
    private float elapsedTime = 0.0f;
    protected SearchViewDistance discoverEnvironment;
    protected bool callOnceFlag;
    protected float ViewDistance { get { return placementManager.GetTypeOfPosition(placementManager.RoundPosition(Position)) == CellType.Hill ? visionRange * viewExtenderScale : visionRange; }}
    public float Health { get => (state.Hunger * state.Thirst * state.RemainingLifetime * state.Health); }
    public AnimalType Type { get { return state.type; } }
    //temporary
    public readonly float viewExtenderScale = 2.0f;
    //
    public State MyState { get; protected set; }

    public Animal(GameObject prefab, PlacementManager _placementManager, Herd parent, AnimalType _type)
    {
        Id = Guid.NewGuid();
        discoveredDrink = new List<Vector3Int>();
        myHerd = parent;
        placementManager = _placementManager;
        spawnPosition = parent.Spawnpoint;
        state = new AnimalInternalState(_type);
        targetPosition = spawnPosition;
        SpawnEntity(prefab, parent.gameObject.transform);
        MyState = State.Moving; 
    }
    protected bool IsAnimalDead() => Health <= 0;

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
    private void MoveToWater() => MoveToTarget(discoverEnvironment.GetDrinkResult, discoveredDrink);
    protected void MoveToTarget(Func<List<Vector3Int>> getResultList, List<Vector3Int> discoveredTargets)
    {
        discoverEnvironment.SearchInViewDistance(ViewDistance, Position);
        List<Vector3Int> targetInviewDistance = getResultList();
        if (targetInviewDistance.Count == 1)
        {
            targetPosition = (Vector3)targetInviewDistance[0];
        }
        else if (targetInviewDistance.Count > 1)
        {
            Vector3Int? closestWithinHerd = null;
            Vector3Int? closestOutOFHerd = null;
            float closestDistanceInHerd = float.MaxValue;
            float closestDistanceOutOfHerd = float.MaxValue;
            foreach (Vector3Int position in targetInviewDistance)
            {
                if (Vector3Int.Distance(myHerd.Spawnpoint, position) <= myHerd.DistributionRadius)
                {
                    float distanceFromPostiion = Vector3Int.Distance(Vector3Int.RoundToInt(Position), position);
                    if (distanceFromPostiion < closestDistanceInHerd)
                    {
                        closestWithinHerd = position;
                        closestDistanceInHerd = distanceFromPostiion;
                    }
                }
                else
                {
                    float distanceFromPostiion = Vector3Int.Distance(Vector3Int.RoundToInt(Position), position);
                    if (distanceFromPostiion < closestDistanceOutOfHerd)
                    {
                        closestOutOFHerd = position;
                        closestDistanceOutOfHerd = distanceFromPostiion;
                    }
                }
            }
            if (closestWithinHerd != null)
            {
                targetPosition = (Vector3)closestWithinHerd;
            } 
            else if(closestOutOFHerd != null)
            {
                targetPosition = (Vector3)closestOutOFHerd;
            }
        }
        else if (discoveredTargets.Count != 0)
        {
            discoveredTargets.Sort((a, b) => Vector3Int.Distance(Vector3Int.RoundToInt(Position), a).CompareTo(Vector3Int.Distance(Vector3Int.RoundToInt(Position), b)));
            while (discoveredTargets.Count != 0 && Vector3Int.Distance(Vector3Int.RoundToInt(Position), discoveredTargets[0]) <= ViewDistance)
            {
                discoveredTargets.RemoveAt(0);
            }
            if (discoveredTargets.Count != 0)
            {
                Vector3Int? inHerdRadius = null;
                int i = 0;
                while (inHerdRadius == null && i < discoveredTargets.Count)
                {
                    if (Vector3Int.Distance(Vector3Int.RoundToInt(discoveredTargets[i]), myHerd.Spawnpoint) <= myHerd.DistributionRadius)
                    {
                        inHerdRadius = discoveredTargets[i];
                    }
                    ++i;
                }
                targetPosition = inHerdRadius ?? discoveredTargets[0];
            }
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
    protected void DiscoverEnvironment() => discoverEnvironment.SearchInViewDistance(ViewDistance, Position);

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
}