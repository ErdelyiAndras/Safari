using System;
using System.Collections.Generic;
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
    protected float maxFood, maxDrink, foodThreshold, drinkThreshold;
    protected int remainingLifetime, food, drink;
    public int Health { get => (food * drink + remainingLifetime); } // TODO : balance health
    public State MyState { get; private set; }
    protected bool IsAnimalDead() => remainingLifetime <= 0 || Health <= 0 || food <= 0 || drink <= 0;

    public Animal(GameObject prefab, PlacementManager _placementManager)
    {
        placementManager = _placementManager;
        spawnPosition = GetRandomSpawnPosition();
        SpawnEntity(prefab);
        baseMoveSpeed = 2.0f; // DEFAULT ÉRTÉK?!
        SpeedMultiplier = 1.0f; // EZT KELL ÁLLÍTANI
    }

    public override void CheckState()
    {
        switch(MyState)
        {
          // TODO
        }
    }

    public virtual void MatureAnimal()
    {
        remainingLifetime--;
        food--;
        drink--;
        if (IsAnimalDead())
        {
            AnimalDies();
        }
        if (food < maxFood * foodThreshold)
        {
            Eat();
        }
        if (drink < maxDrink * drinkThreshold)
        {
            Drink();
        }
    }
    abstract protected void Eat();
    abstract protected void Drink();

    protected void AnimalDies()
    {
        UnityEngine.Object.Destroy(entityInstance);
        AnimalDied?.Invoke();
    }
    private Vector3 GetRandomSpawnPosition()
    {
        float x = UnityEngine.Random.Range(0, placementManager.width);
        float z = UnityEngine.Random.Range(0, placementManager.height);
        return new Vector3(x, 0, z);
    }

    //István
    public float rotationSpeed = 5.0f;
    private Vector3 targetPosition;

    public override void Move()
    {
        if (Vector3.Distance(Position, targetPosition) < 0.1f)
        {
            SetRandomTargetPosition();
        }

        Position = Vector3.MoveTowards(Position, targetPosition, MoveSpeed * Time.deltaTime);

        Vector3 direction = targetPosition - Position;
        if (direction != Vector3.zero)
        {
            Quaternion targetRotation = Quaternion.LookRotation(direction);
            entityInstance.transform.rotation = Quaternion.Slerp(entityInstance.transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);
        }
    }


    private void SetRandomTargetPosition()
    {
        float randomX = UnityEngine.Random.Range(0, 50);
        float randomZ = UnityEngine.Random.Range(0, 50);
        targetPosition = new Vector3(randomX, 0, randomZ);
    }
}