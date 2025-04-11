using UnityEngine;

[System.Serializable]
public class EconomyManagerData
{
    [SerializeField]
    private int money;
    [SerializeField]
    private int admissionFee;

    public int Money
    {
        get
        {
            return money;
        }
    }

    public int AmissionFee
    {
        get
        {
            return admissionFee;
        }
    }

    public EconomyManagerData(int money, int admissionFee)
    {
        this.money = money;
        this.admissionFee = admissionFee;
    }
}