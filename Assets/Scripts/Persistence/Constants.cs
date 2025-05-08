using System.Collections.Generic;

public static class Constants
{
    // DifficultySelector
    public static Difficulty DefaultDifficulty { get; private set; } = Difficulty.Normal;

    public static int MonthLength { get; private set; } = 30;
    public static Dictionary<Difficulty, int> WinConditionLength { get; private set; } = new Dictionary<Difficulty, int>
    {
        { Difficulty.Easy, 3 * MonthLength },
        { Difficulty.Normal, 6 * MonthLength },
        { Difficulty.Hard, 12 * MonthLength }
    };
    public static Dictionary<Difficulty, uint> AnimalWinCondition { get; private set; } = new Dictionary<Difficulty, uint>
    {
        { Difficulty.Easy, 20 },
        { Difficulty.Normal, 30 },
        { Difficulty.Hard, 40 }
    };
    public static Dictionary<Difficulty, int> MoneyWinCondition { get; private set; } = new Dictionary<Difficulty, int>
    {
        { Difficulty.Easy, 1000 },
        { Difficulty.Normal, 2500 },
        { Difficulty.Hard, 5000 }
    };
    public static Dictionary<Difficulty, int> VisitorWinCondition { get; private set; } = new Dictionary<Difficulty, int>
    {
        { Difficulty.Easy, 50 },
        { Difficulty.Normal, 100 },
        { Difficulty.Hard, 200 }
    };

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
        { AnimalType.Carnivore1, 0.97f },
        { AnimalType.Carnivore2, 0.53f },
        { AnimalType.Herbivore1, 0.60f },
        { AnimalType.Herbivore2, 0.65f }
    };
    public static Dictionary<AnimalType, float> DrinkThreshold { get; private set; } = new Dictionary<AnimalType, float>
    {
        { AnimalType.Carnivore1, 0.68f },
        { AnimalType.Carnivore2, 0.57f },
        { AnimalType.Herbivore1, 0.97f },
        { AnimalType.Herbivore2, 0.75f }
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

    public static Dictionary<AnimalType, float> AdultLifetimeThreshold { get; private set; } = new Dictionary<AnimalType, float>
    {
        { AnimalType.Carnivore1, 0.5f },
        { AnimalType.Carnivore2, 0.5f },
        { AnimalType.Herbivore1, 0.5f },
        { AnimalType.Herbivore2, 0.5f }
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
        { AnimalType.Carnivore1, 2.0f },
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

    public static Dictionary<AnimalType, int> ReproductionCooldown { get; private set; } = new Dictionary<AnimalType, int>
    {
        { AnimalType.Carnivore1, 8 },
        { AnimalType.Carnivore2, 8 },
        { AnimalType.Herbivore1, 8 },
        { AnimalType.Herbivore2, 8 }
    };

    private static Dictionary<Difficulty, uint> StartHerbivoreSpawnDifficultyMultiplier { get; set; } = new Dictionary<Difficulty, uint>
    {
        {Difficulty.Easy, 3 },
        {Difficulty.Normal, 2 },
        {Difficulty.Hard, 1 }
    };
    private static Dictionary<Difficulty, uint> StartCarnivoreSpawnDifficultyMultiplier { get; set; } = new Dictionary<Difficulty, uint>
    {
        {Difficulty.Easy, 1 },
        {Difficulty.Normal, 2 },
        {Difficulty.Hard, 3 }
    };
    public static Dictionary<AnimalType, uint> StartAnimalSpwanCount
    {
        get
        {
            return new Dictionary<AnimalType, uint>
            {
                { AnimalType.Carnivore1, 2 * StartCarnivoreSpawnDifficultyMultiplier[DifficultySelector.SelectedDifficulty] },
                { AnimalType.Carnivore2, 2 * StartCarnivoreSpawnDifficultyMultiplier[DifficultySelector.SelectedDifficulty] },
                { AnimalType.Herbivore1, 3 * StartHerbivoreSpawnDifficultyMultiplier[DifficultySelector.SelectedDifficulty] },
                { AnimalType.Herbivore2, 3 * StartHerbivoreSpawnDifficultyMultiplier[DifficultySelector.SelectedDifficulty] }
            };
        }
    }

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
    public static Dictionary<Difficulty, int> MaintenanceFee { get; private set; } = new Dictionary<Difficulty, int>
    {
        { Difficulty.Easy , 20 },
        { Difficulty.Normal, 40 },
        { Difficulty.Hard, 100 }
    };
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

    // PlacementManager
    public static int MapWidth { get; private set; } = 51;
    public static int MapHeight { get; private set; } = 51;
}