using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

public class EconomyManager
{
    public readonly int easyGameStartMoney = 700;
    public readonly int mediumGameStartMoney = 500;
    public readonly int hardGameStartMoney = 300;

    private int money;

    private bool wasInitialized = false;

    public int Money
    {
        get
        {
            return money;
        }
    }

    public Action GoneBankrupt;
    
    // TODO: balance costs
    public int UnitCostOfPlant => 20; 
    public int UnitCostOfHerbivore => 50;
    public int UnitCostOfCarnivore => 100;
    public int UnitCostOfJeep => 200;
    public int UnitCostOfRoad => 10;
    public int UnitCostOfLake => 100;

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
            case Difficulty.Medium:
                money = mediumGameStartMoney;
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

