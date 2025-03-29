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
    protected float maxFood, maxDrink, foodThreshold, drinkThreshold, foodNutrition, drinkNutrition;
    protected float remainingLifetime, food, drink;
    protected readonly float basicViewDistance, viewExtendScale;
    protected List<Vector3Int> discoveredFood, discoveredDrink; // TODO : ÁTTÉRNI RENDEZETT LISSTÁRA (pl: SortedArray), csak a herbivorenak van discoveredfood
    protected float eatingTime, drinkingTime, restTime;
    private float elapsedTime = 0.0f;
    public float rotationSpeed = 5.0f;
    private Vector3 targetPosition;

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
        parent.AddAnimalToHerd(this);
        placementManager = _placementManager;
        spawnPosition = parent.Spawnpoint;
        SpawnEntity(prefab);
        baseMoveSpeed = 2.0f; // DEFAULT ÉRTÉK?!
        SpeedMultiplier = 1.0f; // EZT KELL ÁLLÍTANI
    }

    protected override void CheckState()
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
                break;
            case State.Drinking:
                break;
            case State.Mating:
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
    abstract protected void MoveToFood(); // logika: keres a viewDistancen belül,
                                          // majd ha nem talál megy a legközelebbi eltárolthoz, ha ez sincs akkor megy random
    abstract protected void MoveToWater(); // --||--

    public void Advance()
    {
        MatureAnimal();
        CheckState();
    }

    protected void AnimalDies()
    {
        UnityEngine.Object.Destroy(entityInstance);
        AnimalDied?.Invoke();
    }

    public override void Move()
    {
        if (Vector3.Distance(entityInstance.transform.position, targetPosition) < 0.1f)
        {
            ObjectArrived();
        }
        else
        {
            // Szabadon tudjon mozogni, de vízre ne menjen --> 
            entityInstance.transform.position = Vector3.MoveTowards(entityInstance.transform.position, targetPosition, MoveSpeed * Time.deltaTime);

            Vector3 direction = targetPosition - entityInstance.transform.position;
            if (direction != Vector3.zero)
            {
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
}