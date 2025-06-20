﻿using System.Collections.Generic;

public class AnimalInternalState : ISaveable<AnimalInternalStateData>
{
    public AnimalType type;

    private float remainingLifetime, hunger, thirst, health;

    public float MaxHealth { get { return Constants.MaxHealth[type]; } }
    public float MaxLifeTime { get { return Constants.MaxLifeTime[type]; } }
    public float MaxFood { get { return Constants.MaxFood[type]; } }
    public float MaxDrink { get { return Constants.MaxDrink[type]; } }
    public float FoodThreshold { get { return Constants.FoodThreshold[type]; } }
    public float DrinkThreshold { get { return Constants.DrinkThreshold[type]; } }
    public float FoodNutrition { get { return Constants.FoodNutrition[type] * remainingLifetime / 100.0f; } }
    public float DrinkNutrition { get { return Constants.DrinkNutrition[type]; } }
    public float EatingTime { get { return Constants.EatingTime[type]; } }
    public float DrinkingTime { get { return Constants.DrinkingTime[type]; } }
    public float RestTime { get { return Constants.RestTime[type]; } }
    public float Damage { get { return Constants.DamageToAnimals[type]; } }

    public float RemainingLifetime { get { return remainingLifetime; } set { remainingLifetime = value; } }
    public ref float Hunger { get { return ref hunger; } }
    public ref float Thirst { get { return ref thirst; } }
    public ref float Health { get { return ref health; } }

    public AnimalInternalState(AnimalType type)
    {
        this.type = type;
        LoadAnimalData();
    }

    public AnimalInternalState(AnimalInternalStateData data)
    {
        LoadData(data);
    }

    private void LoadAnimalData()
    {
        remainingLifetime = MaxLifeTime;
        hunger = MaxFood;
        thirst = MaxDrink;
        health = MaxHealth;
    }

    public AnimalInternalStateData SaveData()
    {
        return new AnimalInternalStateData(type, RemainingLifetime, Hunger, Thirst, Health);
    }

    public void LoadData(AnimalInternalStateData data, PlacementManager placementManager = null)
    {
        type = data.Type;
        RemainingLifetime = data.RemainingLifetime;
        Hunger = data.Hunger;
        Thirst = data.Thirst;
        Health = data.Health;
    }
}

