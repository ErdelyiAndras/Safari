using System.Collections.Generic;

public static class Constants
{
    // DifficultySelector
    public static Difficulty DefaultDifficulty { get; private set; } = Difficulty.Normal;

    // mapgenerator?

    // TimeManager
    public static float BaseTimeSpeed { get; private set; } = 30.0f;
    public static float HourSpeedMultiplier { get; private set; } = 1.0f;
    public static float DaySpeedMultiplier { get; private set; } = 2.0f;
    public static float WeekSpeedMultiplier { get; private set; } = 3.0f;
    public static TimeInterval DefaultTimeInterval { get; private set; } = TimeInterval.HOUR;

    // Animal
    public static Dictionary<AnimalType, float> MaxFood { get; private set; } = new Dictionary<AnimalType, float>
    {
        { AnimalType.Carnivore1, 100.0f },
        { AnimalType.Carnivore2, 100.0f },
        { AnimalType.Herbivore1, 100.0f },
        { AnimalType.Herbivore2, 100.0f }
    };
    public static Dictionary<AnimalType, float> MaxDrink { get; private set; } = new Dictionary<AnimalType, float>
    {
        { AnimalType.Carnivore1, 100.0f },
        { AnimalType.Carnivore2, 100.0f },
        { AnimalType.Herbivore1, 100.0f },
        { AnimalType.Herbivore2, 100.0f }
    };
    public static Dictionary<AnimalType, float> FoodThreshold { get; private set; } = new Dictionary<AnimalType, float>
    {
        { AnimalType.Carnivore1, 48.0f },
        { AnimalType.Carnivore2, 53.0f },
        { AnimalType.Herbivore1, 97.0f },
        { AnimalType.Herbivore2, 65.0f }
    };
    public static Dictionary<AnimalType, float> DrinkThreshold { get; private set; } = new Dictionary<AnimalType, float>
    {
        { AnimalType.Carnivore1, 68.0f },
        { AnimalType.Carnivore2, 57.0f },
        { AnimalType.Herbivore1, 70.0f },
        { AnimalType.Herbivore2, 75.0f }
    };
    public static Dictionary<AnimalType, float> FoodNutrition { get; private set; } = new Dictionary<AnimalType, float>
    {
        { AnimalType.Carnivore1, 40.0f },
        { AnimalType.Carnivore2, 40.0f },
        { AnimalType.Herbivore1, 30.0f },
        { AnimalType.Herbivore2, 30.0f }
    };
    public static Dictionary<AnimalType, float> DrinkNutrition { get; private set; } = new Dictionary<AnimalType, float>
    {
        { AnimalType.Carnivore1, 50.0f },
        { AnimalType.Carnivore2, 40.0f },
        { AnimalType.Herbivore1, 20.0f },
        { AnimalType.Herbivore2, 30.0f }
    };
    public static Dictionary<AnimalType, float> EatingTime { get; private set; } = new Dictionary<AnimalType, float>
    {
        { AnimalType.Carnivore1, 7.0f },
        { AnimalType.Carnivore2, 7.0f },
        { AnimalType.Herbivore1, 7.0f },
        { AnimalType.Herbivore2, 7.0f }
    };
    public static Dictionary<AnimalType, float> DrinkingTime { get; private set; } = new Dictionary<AnimalType, float>
    {
        { AnimalType.Carnivore1, 7.0f },
        { AnimalType.Carnivore2, 7.0f },
        { AnimalType.Herbivore1, 7.0f },
        { AnimalType.Herbivore2, 7.0f }
    };
    public static Dictionary<AnimalType, float> RestTime { get; private set; } = new Dictionary<AnimalType, float>
    {
        { AnimalType.Carnivore1, 7.0f },
        { AnimalType.Carnivore2, 7.0f },
        { AnimalType.Herbivore1, 7.0f },
        { AnimalType.Herbivore2, 7.0f }
    };
    public static Dictionary<AnimalType, float> MaxLifeTime { get; private set; } = new Dictionary<AnimalType, float>
    {
        { AnimalType.Carnivore1, 100.0f },
        { AnimalType.Carnivore2, 100.0f },
        { AnimalType.Herbivore1, 100.0f },
        { AnimalType.Herbivore2, 100.0f }
    };
    public static Dictionary<AnimalType, float> MaxHealth { get; private set; } = new Dictionary<AnimalType, float>
    {
        { AnimalType.Carnivore1, 100.0f },
        { AnimalType.Carnivore2, 100.0f },
        { AnimalType.Herbivore1, 100.0f },
        { AnimalType.Herbivore2, 100.0f }
    };
    public static Dictionary<AnimalType, float> DamageToAnimals { get; private set; } = new Dictionary<AnimalType, float>
    {
        { AnimalType.Carnivore1, 30.0f },
        { AnimalType.Carnivore2, 30.0f },
        { AnimalType.Herbivore1, 0.0f },
        { AnimalType.Herbivore2, 0.0f }
    };
    public static Dictionary<AnimalType, float> AnimalVisionRange { get; private set; } = new Dictionary<AnimalType, float>
    {
        { AnimalType.Carnivore1, 12.0f },
        { AnimalType.Carnivore2, 12.0f },
        { AnimalType.Herbivore1, 6.0f },
        { AnimalType.Herbivore2, 6.0f }
    };
    public static Dictionary<AnimalType, float> AnimalViewExtenderScale { get; private set; } = new Dictionary<AnimalType, float>
    {
        { AnimalType.Carnivore1, 2.0f },
        { AnimalType.Carnivore2, 2.0f },
        { AnimalType.Herbivore1, 2.0f },
        { AnimalType.Herbivore2, 2.0f }
    };
    public static Dictionary<AnimalType, float> AnimalBaseMoveSpeed { get; private set; } = new Dictionary<AnimalType, float>
    {
        { AnimalType.Carnivore1, 3.0f },
        { AnimalType.Carnivore2, 3.0f },
        { AnimalType.Herbivore1, 1.5f },
        { AnimalType.Herbivore2, 1.5f }
    };
    public static Dictionary<AnimalType, float> AnimalBaseRotationSpeed { get; private set; } = new Dictionary<AnimalType, float>
    {
        { AnimalType.Carnivore1, 5.0f },
        { AnimalType.Carnivore2, 5.0f },
        { AnimalType.Herbivore1, 5.0f },
        { AnimalType.Herbivore2, 5.0f }
    };

    // Herd
    public static int CarnivoreHerdDistributionRadius { get; private set; } = 5;
    public static int HerbivoreHerdDistributionRadius { get; private set; } = 2;

    // Jeep
    public static float JeepBaseMoveSpeed { get; private set; } = 1.0f;
    public static float JeepBaseRotationSpeed { get; private set; } = 5.0f;

    // EconomyManager
    public static int EasyGameStartMoney { get; private set; } = 700;
    public static int NormalGameStartMoney { get; private set; } = 500;
    public static int HardGameStartMoney { get; private set; } = 300;
    public static int DefaultAdmissionFee { get; private set; } = 40;
    public static int MaintenanceFee { get; private set; } = 20;
    public static int UnitCostOfNature { get; private set; } = 20;
    public static int UnitCostOfHerbivore { get; private set; } = 50;
    public static int UnitCostOfCarnivore { get; private set; } = 100;
    public static int UnitCostOfJeep { get; private set; } = 200;
    public static int UnitCostOfRoad { get; private set; } = 10;
    public static int UnitCostOfWater { get; private set; } = 100;

    // TouristManager
    public static float DefaultSatisfaction { get; private set; } = 50.0f;

    // CameraMovement
    public static float CameraMovementSpeed { get; private set; } = 5.0f;
}