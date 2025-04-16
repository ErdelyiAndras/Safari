using System;
using UnityEngine;

public class EconomyManager : MonoBehaviour, ISaveable<EconomyManagerData>
{
    // TODO: balance start money
    public int easyGameStartMoney = Constants.EasyGameStartMoney;
    public int normalGameStartMoney = Constants.NormalGameStartMoney;
    public int hardGameStartMoney = Constants.HardGameStartMoney;

    public int money;
    private int admissionFee = Constants.DefaultAdmissionFee;

    public Action<int> moneyChanged;

    private int maintenanceFee = Constants.MaintenanceFee;

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
    
    // TODO: balance costs
    public int UnitCostOfNature => Constants.UnitCostOfNature; 
    public int UnitCostOfHerbivore => Constants.UnitCostOfHerbivore;
    public int UnitCostOfCarnivore => Constants.UnitCostOfCarnivore;
    public int UnitCostOfJeep => Constants.UnitCostOfJeep;
    public int UnitCostOfRoad => Constants.UnitCostOfRoad;
    public int UnitCostOfWater => Constants.UnitCostOfWater;

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
        money = int.MaxValue; // TODO: delete this line after debugging
    }

    public bool HasEnoughMoney(int amount) => money >= amount;

    public void SpendMoney(int amount)
    {
        money -= amount;
        moneyChanged?.Invoke(money);
        CheckIfGameOver();
    }

    public void DailyMaintenance()
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
        return new EconomyManagerData(money, admissionFee);
    }

    public void LoadData(EconomyManagerData data, PlacementManager placementManager = null)
    {
        money = data.Money;
        admissionFee = data.AmissionFee;
    }
}
