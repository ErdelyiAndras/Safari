using System.Collections.Generic;

public class AnimalInternalState
{
    public readonly AnimalType type;

    private static readonly Dictionary<AnimalType, float>  maxLifeTime, maxFood, maxDrink;
    private static readonly Dictionary<AnimalType, float> foodThreshold, drinkThreshold;
    private static readonly Dictionary<AnimalType, float> foodNutrition, drinkNutrition;
    private static readonly Dictionary<AnimalType, float> eatingTime, drinkingTime, restTime;
    private static readonly Dictionary<AnimalType, float> maxHealth, damageToAnimals;
    private float remainingLifetime, hunger, thirst, health;

    public float MaxHealth { get { return maxHealth[type]; } }
    public float MaxLifeTime { get { return maxLifeTime[type]; } }
    public float MaxFood { get { return maxFood[type]; } }
    public float MaxDrink { get { return maxDrink[type]; } }
    public float FoodThreshold { get { return foodThreshold[type]; } }
    public float DrinkThreshold { get { return drinkThreshold[type]; } }
    public float FoodNutrition { get { return foodNutrition[type]; } }
    public float DrinkNutrition { get { return drinkNutrition[type]; } }
    public float EatingTime { get { return eatingTime[type]; } }
    public float DrinkingTime { get { return drinkingTime[type]; } }
    public float RestTime { get { return restTime[type]; } }
    public float Damage { get { return damageToAnimals[type]; } }
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
    static AnimalInternalState()
    {
        maxLifeTime = new Dictionary<AnimalType, float>();
        maxHealth = new Dictionary<AnimalType, float>();
        maxFood = new Dictionary<AnimalType, float>();
        maxDrink = new Dictionary<AnimalType, float>();
        foodThreshold = new Dictionary<AnimalType, float>();
        drinkThreshold = new Dictionary<AnimalType, float>();
        foodNutrition = new Dictionary<AnimalType, float>();
        drinkNutrition = new Dictionary<AnimalType, float>();
        eatingTime = new Dictionary<AnimalType, float>();
        drinkingTime = new Dictionary<AnimalType, float>();
        restTime = new Dictionary<AnimalType, float>();
        damageToAnimals = new Dictionary<AnimalType, float>();
        LoadStaticData();
    }

    private void LoadData()
    {
        RemainingLifetime = maxLifeTime[type];
        Hunger = maxFood[type];
        Thirst = maxDrink[type];
        Health = maxHealth[type];
    }
    private static void LoadStaticData()
    {
        //placeholder
        maxLifeTime.Add(AnimalType.Herbivore1, 100.0f);
        maxLifeTime.Add(AnimalType.Herbivore2, 100.0f);
        maxLifeTime.Add(AnimalType.Carnivore1, 100.0f);
        maxLifeTime.Add(AnimalType.Carnivore2 , 100.0f);

        maxFood.Add(AnimalType.Herbivore1, 100.0f);
        maxFood.Add(AnimalType.Herbivore2, 100.0f);
        maxFood.Add(AnimalType.Carnivore1, 100.0f);
        maxFood.Add(AnimalType.Carnivore2, 100.0f);

        maxDrink.Add(AnimalType.Herbivore1, 100.0f);
        maxDrink.Add(AnimalType.Herbivore2, 100.0f);
        maxDrink.Add(AnimalType.Carnivore1, 100.0f);
        maxDrink.Add(AnimalType.Carnivore2, 100.0f);

        foodThreshold.Add(AnimalType.Herbivore1, 95.0f);
        foodThreshold.Add(AnimalType.Herbivore2, 65.0f);
        foodThreshold.Add(AnimalType.Carnivore1, 48.0f);
        foodThreshold.Add(AnimalType.Carnivore2, 53.0f);

        drinkThreshold.Add(AnimalType.Herbivore1, 92.0f);
        drinkThreshold.Add(AnimalType.Herbivore2, 75.0f);
        drinkThreshold.Add(AnimalType.Carnivore1, 68.0f);
        drinkThreshold.Add(AnimalType.Carnivore2, 57.0f);

        foodNutrition.Add(AnimalType.Herbivore1, 30.0f);
        foodNutrition.Add(AnimalType.Herbivore2, 30.0f);
        foodNutrition.Add(AnimalType.Carnivore1, 40.0f);
        foodNutrition.Add(AnimalType.Carnivore2, 40.0f);

        drinkNutrition.Add(AnimalType.Herbivore1, 20.0f);
        drinkNutrition.Add(AnimalType.Herbivore2, 30.0f);
        drinkNutrition.Add(AnimalType.Carnivore1, 50.0f);
        drinkNutrition.Add(AnimalType.Carnivore2, 40.0f);

        eatingTime.Add(AnimalType.Herbivore1, 7.0f);
        eatingTime.Add(AnimalType.Herbivore2, 7.0f);
        eatingTime.Add(AnimalType.Carnivore1, 7.0f);
        eatingTime.Add(AnimalType.Carnivore2, 7.0f);

        drinkingTime.Add(AnimalType.Herbivore1, 7.0f);
        drinkingTime.Add(AnimalType.Herbivore2, 7.0f);
        drinkingTime.Add(AnimalType.Carnivore1, 7.0f);
        drinkingTime.Add(AnimalType.Carnivore2, 7.0f);

        restTime.Add(AnimalType.Herbivore1, 7.0f);
        restTime.Add(AnimalType.Herbivore2, 7.0f);
        restTime.Add(AnimalType.Carnivore1, 7.0f);
        restTime.Add(AnimalType.Carnivore2, 7.0f);

        maxHealth.Add(AnimalType.Herbivore1, 100.0f);
        maxHealth.Add(AnimalType.Herbivore2, 100.0f);
        maxHealth.Add(AnimalType.Carnivore1, 100.0f);
        maxHealth.Add(AnimalType.Carnivore2, 100.0f);

        damageToAnimals.Add(AnimalType.Herbivore1, 0.0f);
        damageToAnimals.Add(AnimalType.Herbivore2, 0.0f);
        damageToAnimals.Add(AnimalType.Carnivore1, 30.0f);
        damageToAnimals.Add(AnimalType.Carnivore2, 30.0f);

        // Load data from a data source (e.g., a file or database)
    }
}

