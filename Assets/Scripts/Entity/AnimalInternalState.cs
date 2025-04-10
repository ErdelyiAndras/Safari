using System.Collections.Generic;

public class AnimalInternalState
{
    public readonly AnimalType type;

    private float remainingLifetime, hunger, thirst, health;

    public float MaxHealth { get { return Constants.MaxHealth[type]; } }
    public float MaxLifeTime { get { return Constants.MaxLifeTime[type]; } }
    public float MaxFood { get { return Constants.MaxFood[type]; } }
    public float MaxDrink { get { return Constants.MaxDrink[type]; } }
    public float FoodThreshold { get { return Constants.FoodThreshold[type]; } }
    public float DrinkThreshold { get { return Constants.DrinkThreshold[type]; } }
    public float FoodNutrition { get { return Constants.FoodNutrition[type]; } }
    public float DrinkNutrition { get { return Constants.DrinkNutrition[type]; } }
    public float EatingTime { get { return Constants.EatingTime[type]; } }
    public float DrinkingTime { get { return Constants.DrinkingTime[type]; } }
    public float RestTime { get { return Constants.RestTime[type]; } }
    public float Damage { get { return Constants.DamageToAnimals[type]; } }
    public ref float RemainingLifetime { get { return ref remainingLifetime; }}
    public ref float Hunger { get { return ref hunger; } }
    public ref float Thirst { get { return ref thirst; } }

    public ref float Health { get { return ref health; } }

    public readonly float viewExtenderScale = 2.0f;

    public AnimalInternalState(AnimalType type)
    {
        this.type = type;
        LoadData();
    }

    private void LoadData()
    {
        RemainingLifetime = MaxLifeTime;
        Hunger = MaxFood;
        Thirst = MaxDrink;
        Health = MaxHealth;

    }
}

