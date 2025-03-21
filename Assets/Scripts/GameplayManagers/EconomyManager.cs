using System;
using UnityEngine;

public class EconomyManager : MonoBehaviour
{
    // TODO: balance start money
    public int easyGameStartMoney = 700;
    public int normalGameStartMoney = 500;
    public int hardGameStartMoney = 300;

    public int money;
    private int admissionFee = 40;

    private bool wasInitialized = false;

    private int maintenanceFee = 20;

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
    public int UnitCostOfNature => 20; 
    public int UnitCostOfHerbivore => 50;
    public int UnitCostOfCarnivore => 100;
    public int UnitCostOfJeep => 200;
    public int UnitCostOfRoad => 10;
    public int UnitCostOfWater => 100;

    public void InitMoney(Difficulty difficulty)
    {
        if (wasInitialized)
        {
            throw new InvalidOperationException("EconomyManager was already initialized");
        }
        wasInitialized = true;
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
        CheckIfGameOver();
    }

    public void DailyMaintenance()
    {
        SpendMoney(maintenanceFee);
    }

    public void EarnMoney(int amount)
    {
        money += amount;
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

}

