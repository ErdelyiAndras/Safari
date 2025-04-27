using UnityEngine;

[System.Serializable]
public class EconomyManagerData
{
    [SerializeField]
    private int money;
    [SerializeField]
    private int admissionFee;
    [SerializeField]
    private int getConditionPassedDays;

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
    }

    public int GetConditionPassedDays
    {
        get
        {
            return getConditionPassedDays;
        }
    }

    public EconomyManagerData(int money, int admissionFee, int getConditionPassedDays)
    {
        this.money = money;
        this.admissionFee = admissionFee;
        this.getConditionPassedDays = getConditionPassedDays;
    }
}