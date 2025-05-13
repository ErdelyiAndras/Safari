using System;
using UnityEngine;

public class EconomyManager : MonoWinCondition, ISaveable<EconomyManagerData>, ITimeHandler
{
    private readonly int easyGameStartMoney = Constants.EasyGameStartMoney;
    private readonly int normalGameStartMoney = Constants.NormalGameStartMoney;
    private readonly int hardGameStartMoney = Constants.HardGameStartMoney;

    public int money; // TODO: private balance után
    private int admissionFee = Constants.DefaultAdmissionFee;

    public Action<int> moneyChanged;

    private int maintenanceFee = Constants.MaintenanceFee[DifficultySelector.SelectedDifficulty];

    public int Money
    {
        get
        {
            return money;
        }
    }

    public int AdmissionFee
    {
        get
        {
            return admissionFee;
        }
        set
        {
            if (value <= 0)
            {
                return;
            }
            admissionFee = value;
        }
    }

    public Action GoneBankrupt;
    
    public int UnitCostOfNature => Constants.UnitCostOfNature; 
    public int UnitCostOfHerbivore => Constants.UnitCostOfHerbivore;
    public int UnitCostOfCarnivore => Constants.UnitCostOfCarnivore;
    public int UnitCostOfJeep => Constants.UnitCostOfJeep;
    public int UnitCostOfRoad => Constants.UnitCostOfRoad;
    public int UnitCostOfWater => Constants.UnitCostOfWater;
    public int SellAnimalIncome => Constants.SellAnimalIncome;

    private void Awake()
    {
        InitMoney(DifficultySelector.SelectedDifficulty);
    }

    private void InitMoney(Difficulty difficulty)
    {
        switch (difficulty)
        {
            case Difficulty.Easy:
                money = easyGameStartMoney;
                break;
            case Difficulty.Normal:
                money = normalGameStartMoney;
                break;
            case Difficulty.Hard:
                money = hardGameStartMoney;
                break;
        }
    }

    public bool HasEnoughMoney(int amount) => money >= amount;

    public void SpendMoney(int amount)
    {
        money -= amount;
        moneyChanged?.Invoke(money);
        CheckIfGameOver();
    }

    private void DailyMaintenance()
    {
        SpendMoney(maintenanceFee);
    }

    public void EarnMoney(int amount)
    {
        money += amount;
        moneyChanged?.Invoke(money);
    }

    private void CheckIfGameOver()
    {
        if (IsBankrupt())
        {
            OnGoneBankrupt();
        }
    }

    private bool IsBankrupt() => money < 0;

    private void OnGoneBankrupt()
    {
        GoneBankrupt?.Invoke();
    }

    public EconomyManagerData SaveData()
    {
        return new EconomyManagerData(money, admissionFee, GetConditionPassedDays);
    }

    public void LoadData(EconomyManagerData data, PlacementManager placementManager = null)
    {
        money = data.Money;
        admissionFee = data.AdmissionFee;
        GetConditionPassedDays = data.GetConditionPassedDays;
        maintenanceFee = Constants.MaintenanceFee[DifficultySelector.SelectedDifficulty];
    }

    public void ManageTick()
    {
        DailyMaintenance();
        SetConditionPassedDays();
    }

    protected override void SetConditionPassedDays()
    {
        if (Money > Constants.MoneyWinCondition[DifficultySelector.SelectedDifficulty])
        {
            GetConditionPassedDays++;
        }
        else
        {
            GetConditionPassedDays = 0;
        }
    }
}
