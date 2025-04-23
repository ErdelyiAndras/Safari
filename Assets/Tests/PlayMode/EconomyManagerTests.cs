using System.Collections;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;

public class EconomyManagerTests
{
    private EconomyManager economyManager;

    [UnitySetUp]
    public IEnumerator Setup()
    {
        var go = new GameObject();
        economyManager = go.AddComponent<EconomyManager>();
        yield return null;
    }

    [UnityTearDown]
    public IEnumerator TearDown()
    {
        Object.Destroy(economyManager.gameObject);
        yield return null;
    }

    [UnityTest]
    public IEnumerator EconomyManagerEarnMoneyTest()
    {
        int start = economyManager.Money;

        economyManager.EarnMoney(100);

        Assert.AreEqual(start + 100, economyManager.Money);
        yield return null;
    }

    [UnityTest]
    public IEnumerator EconomyManagerSpendMoneyTest()
    {
        int start = economyManager.Money;

        economyManager.SpendMoney(200);

        Assert.AreEqual(start - 200, economyManager.Money);
        yield return null;
    }

    [UnityTest]
    public IEnumerator EconomyManagerLoadConstructorTest()
    {
        EconomyManagerData data = new EconomyManagerData(10, 20, 0);
        economyManager.LoadData(data);
        Assert.AreEqual(data.Money, economyManager.Money);
        Assert.AreEqual(data.AdmissionFee, economyManager.AdmissionFee);
        yield return null;
    }

    [UnityTest]
    public IEnumerator EconomyManagerSaveDataTest()
    {
        economyManager.EarnMoney(100);
        economyManager.DailyMaintenance();
        EconomyManagerData data = economyManager.SaveData();
        Assert.AreEqual(economyManager.Money, data.Money);
        Assert.AreEqual(economyManager.AdmissionFee, data.AdmissionFee);
        yield return null;
    }

    [UnityTest]
    public IEnumerator EconomyManagerHasEnoughMoneyTest()
    {
        EconomyManagerData data = new EconomyManagerData(10, 20, 0);
        economyManager.LoadData(data);
        Assert.IsTrue(economyManager.HasEnoughMoney(5));
        Assert.IsFalse(economyManager.HasEnoughMoney(15));
        yield return null;
    }

    [UnityTest]
    public IEnumerator EconomyManagerGoneBankruptTest()
    {
        bool isBankrupt = false;
        economyManager.GoneBankrupt += () => isBankrupt = true;
        economyManager.SpendMoney(economyManager.Money + 1);
        //economyManager.SpendMoney(1); // Only needed while money is set to max for debugging
        Assert.IsTrue(isBankrupt);
        yield return null;
    }

    [UnityTest]
    public IEnumerator EconomyManagerAdmissionFeeValidSetTest()
    {
        int start = economyManager.AdmissionFee;
        economyManager.AdmissionFee = 50; ;
        Assert.AreEqual(50, economyManager.AdmissionFee);
        yield return null;
    }

    [UnityTest]
    public IEnumerator EconomyManagerAdmissionFeeInvalidSetTest()
    {
        int start = economyManager.AdmissionFee;
        economyManager.AdmissionFee = -50; ;
        Assert.AreEqual(start, economyManager.AdmissionFee);
        yield return null;
    }
}