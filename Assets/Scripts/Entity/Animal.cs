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
    public readonly AnimalType type;
    public Action<Animal> AnimalDied;
    protected float maxFood = 100.0f, maxDrink = 100.0f, foodThreshold = 0.97f, drinkThreshold = 0.7f, foodNutrition = 3.0f, drinkNutrition = 30.0f;
    protected float remainingLifetime = 100.0f, food = 100.0f, drink = 100.0f;
    protected readonly float basicViewDistance = 10.0f, viewExtendScale = 2.0f;
    protected List<Vector3Int> discoveredDrink;
    protected float eatingTime = 2.0f, drinkingTime = 2.0f, restTime = 10.0f;
    private float elapsedTime = 0.0f;
    public float rotationSpeed = 5.0f;
    protected Vector3 targetPosition;
    public Herd myHerd;
    protected SearchViewDistance discoverEnvironment;
    private bool callOnceFlag;
    

    // TODO: ViewDistance cuccait kiszervezni a SearchViewDiasntce classba
    protected float ViewDistance { 
        get 
        {
            return placementManager.GetTypeOfPosition(placementManager.RoundPosition(Position)) == CellType.Hill ? basicViewDistance * viewExtendScale : basicViewDistance;
        }
    }

    public float Health { get => (food * drink + remainingLifetime); } // TODO : balance health

    public State MyState { get; private set; }
    protected bool IsAnimalDead() => remainingLifetime <= 0 || Health <= 0 || food <= 0 || drink <= 0;

    public Animal(GameObject prefab, PlacementManager _placementManager, Herd parent, AnimalType _type)
    {
        myHerd = parent;
        placementManager = _placementManager;
        spawnPosition = parent.Spawnpoint;
        baseMoveSpeed = 2.0f;
        type = _type;
        discoveredDrink = new List<Vector3Int>();
        SpawnEntity(prefab, parent.gameObject.transform);
        targetPosition = spawnPosition;
        MyState = State.Moving;
        
    }

    public override void CheckState()
    {   
        switch(MyState)
        {
            case State.Resting:
                Debug.Log("RESING");
                WaitAction(restTime, State.Moving);
                break;
            case State.Moving:
                Debug.Log("MOVING");
                Move();
                break;
            case State.SearchingForFood:
                Debug.Log("LOOKING FOR FOOD");
                MoveToFood();
                break;
            case State.SearchingForWater:
                MoveToWater();
                break;
            case State.Eating:
                Debug.Log("EATING");
                WaitAction(eatingTime, ref food, foodNutrition, State.Resting);
                break;
            case State.Drinking:
                WaitAction(drinkingTime, ref drink, drinkNutrition, State.Resting);
                break;
            case State.Mating:
                WaitAction(restTime, State.Moving);
                break;
        }
    }

    private void WaitAction(float duration, ref float toAdvance, float advanceStep, State changeStateTo)
    {
        if (elapsedTime < duration)
        {
            elapsedTime += Time.deltaTime;
        }
        else
        {
            Debug.Log("Eating Done");
            toAdvance += advanceStep;
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

    protected virtual void MatureAnimal()
    {
        remainingLifetime--;
        food--;
        drink--;
        if (IsAnimalDead())
        {
            AnimalDies();
        }
        if (MyState != State.Eating && MyState != State.Drinking && MyState != State.Mating)
        {
            if (food < maxFood * foodThreshold && MyState != State.SearchingForWater)
            {
                MyState = State.SearchingForFood;
            }
            if (drink < maxDrink * drinkThreshold && MyState != State.SearchingForFood)
            {
                MyState = State.SearchingForWater;
            }
            if (food < maxFood * foodThreshold && drink < maxDrink * drinkThreshold)
            {
                if (food / foodThreshold < drink / drinkThreshold)
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
    abstract protected void MoveToFood();
    private void MoveToWater() => MoveToTarget(discoverEnvironment.GetDrinkResult, discoveredDrink);
    //TODO: Mi van ha nincs se a közelbe se a listába?
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

    public void AgeAnimal()
    {
        MatureAnimal();
    }

    protected void AnimalDies()
    {
        UnityEngine.Object.Destroy(entityInstance);
        AnimalDied?.Invoke(this);
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
                entityInstance.transform.rotation = Quaternion.Slerp(entityInstance.transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);
            }
        }
    }

    private void ObjectArrived()
    {
        CellType targetType = placementManager.GetTypeOfPosition(Vector3Int.RoundToInt(targetPosition));
        callOnceFlag = false;
        SetRandomTargetPosition(); // kimehet a csora radiusabol
        switch (MyState) 
        {
            case State.Moving:
                MyState = State.Resting;
                break;
            case State.SearchingForFood:
                if (targetType == CellType.Nature)
                {
                    MyState = State.Eating;
                }
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

    protected void DiscoverEnvironment() => discoverEnvironment.SearchInViewDistance(ViewDistance, Position);
    

    private void SetRandomTargetPosition(bool inHerd = true)
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
                Debug.Log("Random position to find food" + temporatyPosition);
            }
        } while (!placementManager.CheckIfPositionInBound(Vector3Int.RoundToInt(temporatyPosition)) || !placementManager.IsPositionWalkable(Vector3Int.RoundToInt(temporatyPosition)));
        targetPosition = temporatyPosition;
    }
}