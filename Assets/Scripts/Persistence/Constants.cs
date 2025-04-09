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

    // TODO: assign these values
    // OPT: different animals have different values
    // Animal
    public static float MaxFood { get; private set; } = 100.0f;
    public static float MaxDrink { get; private set; } = 100.0f;
    public static float FoodThreshold { get; private set; } = 0.92f;
    public static float DrinkThreshold { get; private set; } = 0.98f;
    public static float FoodNutrition { get; private set; } = 6.0f;
    public static float DrinkNutrition { get; private set; } = 3.0f;
    public static float EatingTime { get; private set; } = 10.0f;
    public static float DrinkingTime { get; private set; } = 10.0f;
    public static float RestTime { get; private set; } = 7.0f;
    public static float AnimalBasicViewDistance { get; private set; } = 10.0f; // TODO: possible rename to match
    public static float AnimalViewExtendScale { get; private set; } = 2.0f; // TODO: possible rename to match
    public static float AnimalBaseMoveSpeed { get; private set; } = 2.0f;
    public static float AnimalBaseRotationSpeed { get; private set; } = 5.0f;

    // Herd
    public static float CarnivoreHerdDistributionRadius { get; private set; } = 5.0f;
    public static float HerbivoreHerdDistributionRadius { get; private set; } = 2.0f;

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

    // CameraMovement
    public static float CameraMovementSpeed { get; private set; } = 5.0f;
}