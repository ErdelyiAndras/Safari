using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

public class EconomyManager
{
    private int money;

    public Action<bool> GameOver;

    private readonly int easyGameStartMoney = 700;
    private readonly int mediumGameStartMoney = 500;
    private readonly int hardGameStartMoney = 300;
    public int Money 
    {
        get { return money; }
    }
    public bool SpendMoney(int amount)
    {
        if (money < amount)
        {
            return false;
        }
        money -= amount;
        CheckIfGameOver();
        return true;
    }

    private void CheckIfGameOver()
    {
        if (IsPlayerBankrupt())
        {
            OnGameOver(false);
        }
        //TODO 
    }

    private bool IsPlayerBankrupt() => money > 0;

    private void OnGameOver(bool result)
    {
        GameOver?.Invoke(result);
    }

}

