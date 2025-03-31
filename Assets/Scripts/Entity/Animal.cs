using System;
using System.Collections.Generic;
using TMPro.EditorUtilities;
using Unity.VisualScripting;
using UnityEngine;


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
    public Action AnimalDied;
    protected float maxFood = 100.0f, maxDrink = 100.0f, foodThreshold = 70.0f, drinkThreshold = 70.0f, foodNutrition = 30.0f, drinkNutrition = 30.0f;
    protected float remainingLifetime = 100.0f, food = 100.0f, drink = 100.0f;
    protected readonly float basicViewDistance = 10.0f, viewExtendScale = 2.0f;
    protected List<Vector3Int> discoveredDrink; // TODO : ÁTTÉRNI RENDEZETT LISSTÁRA (pl: SortedArray), csak a herbivorenak van discoveredfood
    protected float eatingTime = 2.0f, drinkingTime = 2.0f, restTime = 2.0f;
    private float elapsedTime = 0.0f;
    public float rotationSpeed = 5.0f;
    protected Vector3 targetPosition;
    protected Herd myHerd;

    protected float ViewDistance { 
        get 
        {
            return placementManager.GetTypeOfPosition(placementManager.RoundPosition(Position)) == CellType.Hill ? basicViewDistance * viewExtendScale : basicViewDistance; }
        }
    public float Health { get => (food * drink + remainingLifetime); } // TODO : balance health

    public State MyState { get; private set; }
    protected bool IsAnimalDead() => remainingLifetime <= 0 || Health <= 0 || food <= 0 || drink <= 0;

    public Animal(GameObject prefab, PlacementManager _placementManager, Herd parent)
    {
        myHerd = parent;
        //parent.AddAnimalToHerd(this);
        placementManager = _placementManager;
        spawnPosition = parent.Spawnpoint;
        SpawnEntity(prefab);
        baseMoveSpeed = 2.0f; // DEFAULT ÉRTÉK?!
        SpeedMultiplier = 1.0f; // EZT KELL ÁLLÍTANI
        MyState = State.Moving;
        targetPosition = spawnPosition;
    }

    public override void CheckState()
    {
        switch(MyState)
        {
            case State.Resting:
                WaitAction(restTime, State.Moving);
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
        Debug.Log("Before decrese:" +remainingLifetime + " " + Health + " " + food + " " + drink);
        remainingLifetime--;
        food--;
        drink--;
        if (IsAnimalDead())
        {
            Debug.Log("KILL CALL");
            Debug.Log(remainingLifetime + " " + Health + " " + food + " " + drink);
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
    abstract protected void MoveToFood(); // logika: keres a viewDistancen belül,
                                          // majd ha nem talál megy a legközelebbi eltárolthoz, ha ez sincs akkor megy random
    protected void MoveToWater()
    {

    }

    public void Advance()
    {
        MatureAnimal();
        CheckState();
    }

    protected void AnimalDies()
    {
        Debug.Log("Kill myself");
        UnityEngine.Object.Destroy(entityInstance);
        AnimalDied?.Invoke();
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
        SetRandomTargetPosition();
        switch (MyState) 
        {
            case State.Moving:
                MyState = State.Resting;
                break;
            case State.SearchingForFood:
                MyState = State.Eating;
                break;
            case State.SearchingForWater:
                MyState = State.Drinking;
                break;
            default:
                break;
        }
    }

    abstract protected void DiscoverEnvironment();    

    private void SetRandomTargetPosition(bool inHerd = true)
    {
        // TODO belekalkulálni a Herd radiusát
        float randomX = 0, randomZ = 0;
        do
        {
            if (inHerd)
            {
                // TODO A herden belül random pozicio
            }
            else
            {
                randomX = UnityEngine.Random.Range(0, placementManager.width);
                randomZ = UnityEngine.Random.Range(0, placementManager.height);
            }
        } while (placementManager.IsPositionWalkable(new Vector3Int((int)randomX, 0, (int)randomZ)));
        
        targetPosition = new Vector3(randomX, 0, randomZ);
    }

    protected List<Vector3Int> SearchInViewDistance()
    {
        List<Vector3Int> result = new List<Vector3Int>();
        Queue<Vector3Int> queue = new Queue<Vector3Int>();
        HashSet<Vector3Int> visited = new HashSet<Vector3Int>();

        Vector3Int startPosition = Vector3Int.RoundToInt(Position);
        queue.Enqueue(startPosition);
        visited.Add(startPosition);

        while (queue.Count > 0)
        {
            Vector3Int current = queue.Dequeue();
            if (placementManager.GetTypeOfPosition(current) == CellType.Nature)
            {
                result.Add(current);
            }

            if (Vector3Int.Distance(startPosition, current) <= ViewDistance)
            {
                foreach (Vector3Int neighbor in GetNeighbors(current))
                {
                    if (!visited.Contains(neighbor))
                    {
                        queue.Enqueue(neighbor);
                        visited.Add(neighbor);
                    }
                }
            }
        }

        return result;
    }

    private List<Vector3Int> GetNeighbors(Vector3Int position)
    {
        List<Vector3Int> neighbors = new List<Vector3Int>
        {
            position + Vector3Int.right,
            position + Vector3Int.left,
            position + Vector3Int.up,
            position + Vector3Int.down,
            position + new Vector3Int(1, 0, 1),
            position + new Vector3Int(1, 0, -1),
            position + new Vector3Int(-1, 0, 1),
            position + new Vector3Int(-1, 0, -1)
        };

        neighbors.RemoveAll(neighbor => !placementManager.CheckIfPositionInBound(neighbor) || !placementManager.IsPositionWalkable(neighbor));

        return neighbors;
    }
}