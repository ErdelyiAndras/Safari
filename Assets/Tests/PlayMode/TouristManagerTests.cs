using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;
using static TouristManager;

public class TouristManagerTests
{
    private TouristManager touristManager;

    [UnitySetUp]
    public IEnumerator SetUp()
    {
        DifficultySelector.SelectedDifficulty = Difficulty.Normal;

        typeof(Constants)
            .GetProperty("WinConditionLength")
            .SetValue(null, new Dictionary<Difficulty, int>
            {
                { Difficulty.Easy, 5 },
                { Difficulty.Normal, 10 },
                { Difficulty.Hard, 15 }
            });

        typeof(Constants)
            .GetProperty("VisitorWinCondition")
            .SetValue(null, new Dictionary<Difficulty, int>
            {
                { Difficulty.Easy, 20 },
                { Difficulty.Normal, 30 },
                { Difficulty.Hard, 40 }
            });

        var placementGO = new GameObject("PlacementManager");
        var placementManager = placementGO.AddComponent<PlacementManager>();

        var prefabGO = new GameObject("PrefabManager");
        var prefabManager = prefabGO.AddComponent<PrefabManager>();
        prefabManager.JeepPrefab = GameObject.CreatePrimitive(PrimitiveType.Cube);

        placementManager.prefabManager = prefabManager;

        var touristGO = new GameObject("TouristManager");
        touristGO.SetActive(false);

        touristManager = touristGO.AddComponent<TouristManager>();
        touristManager.placementManager = placementManager;

        touristGO.SetActive(true);

        yield return null;
    }

    [UnityTest]
    public IEnumerator ModifySatisfaction1()
    {
        typeof(TouristManager)
            .GetProperty("Satisfaction")
            .SetValue(touristManager, 50f);

        typeof(TouristManager)
            .GetField("touristCount", BindingFlags.NonPublic | BindingFlags.Instance)
            .SetValue(touristManager, 0);

        bool eventFired = false;
        float newSatisfaction = -1f;

        touristManager.SatisfactionChanged += (value) =>
        {
            eventFired = true;
            newSatisfaction = value;
        };

        MethodInfo modifyMethod = typeof(TouristManager)
            .GetMethod("ModifySatisfaction", BindingFlags.NonPublic | BindingFlags.Instance);

        modifyMethod.Invoke(touristManager, new object[] { 80f });

        yield return null;

        Assert.IsTrue(eventFired, "SatisfactionChanged nem hívódott meg");
        Assert.AreEqual(100f, newSatisfaction, "Nem várt satisfaction érték touristCount == 0 esetén");
    }

    [UnityTest]
    public IEnumerator ModifySatisfaction2()
    {
        typeof(TouristManager)
            .GetProperty("Satisfaction")
            .SetValue(touristManager, 60f);

        typeof(TouristManager)
            .GetField("touristCount", BindingFlags.NonPublic | BindingFlags.Instance)
            .SetValue(touristManager, 8);

        bool eventFired = false;
        float newSatisfaction = -1f;

        touristManager.SatisfactionChanged += (value) =>
        {
            eventFired = true;
            newSatisfaction = value;
        };

        MethodInfo modifyMethod = typeof(TouristManager)
            .GetMethod("ModifySatisfaction", BindingFlags.NonPublic | BindingFlags.Instance);

        modifyMethod.Invoke(touristManager, new object[] { 80f });

        yield return null;

        Assert.IsTrue(eventFired, "SatisfactionChanged nem hívódott meg");
        Assert.AreEqual(50f, newSatisfaction, 0.01f, "Nem várt satisfaction érték touristCount != 0 esetén");
    }

    [UnityTest]
    public IEnumerator AcquireNewJeep_AddsJeepAndFiresEvent()
    {
        int eventValue = -1;
        touristManager.JeepCountChanged += (count) => eventValue = count;

        MethodInfo acquireMethod = typeof(TouristManager)
            .GetMethod("AcquireNewJeep", BindingFlags.Public | BindingFlags.Instance);

        acquireMethod.Invoke(touristManager, null);

        yield return null;

        var jeepsField = typeof(TouristManager)
            .GetField("jeeps", BindingFlags.NonPublic | BindingFlags.Instance);
        var jeepsList = jeepsField.GetValue(touristManager) as IList;

        Assert.IsNotNull(jeepsList, "A jeeps lista null");
        Assert.AreEqual(1, jeepsList.Count, "Nem lett hozzáadva jeep");

        var firstJeep = jeepsList[0] as Jeep;
        Assert.IsNotNull(firstJeep, "A jeeps lista elsõ eleme nem Jeep");

        Assert.AreEqual(1, eventValue, "JeepCountChanged nem hívódott meg, vagy rossz értéket adott");
    }

    [UnityTest]
    public IEnumerator SetConditionPassedDays_WinConditionMet()
    {
        var data = new MonthlyTourists { days = Constants.MonthLength, tourists = Constants.VisitorWinCondition[Difficulty.Normal] };
        typeof(TouristManager).GetField("monthlyTourists", BindingFlags.NonPublic | BindingFlags.Instance).SetValue(touristManager, data);
        typeof(MonoWinCondition)
            .GetProperty("GetConditionPassedDays", BindingFlags.NonPublic | BindingFlags.Instance)
            .SetValue(touristManager, 0);

        MethodInfo method = typeof(TouristManager).GetMethod("SetConditionPassedDays", BindingFlags.NonPublic | BindingFlags.Instance);
        method.Invoke(touristManager, null);

        yield return null;

        int passedDays = (int)typeof(MonoWinCondition)
            .GetProperty("GetConditionPassedDays", BindingFlags.Instance | BindingFlags.NonPublic)
            .GetValue(touristManager);
        var currentStats = (MonthlyTourists)typeof(TouristManager).GetField("monthlyTourists", BindingFlags.NonPublic | BindingFlags.Instance).GetValue(touristManager);

        Assert.AreEqual(Constants.MonthLength, passedDays, "Nem nõtt meg a GetConditionPassedDays értéke");
        Assert.AreEqual(0, currentStats.days, "A hónap napjai nem nullázódtak");
        Assert.AreEqual(0, currentStats.tourists, "A turisták száma nem nullázódott");
    }

    [UnityTest]
    public IEnumerator SetConditionPassedDays_MonthOver_NotEnoughTourists()
    {
        var data = new MonthlyTourists { days = Constants.MonthLength, tourists = Constants.VisitorWinCondition[Difficulty.Normal] - 1 };
        typeof(TouristManager).GetField("monthlyTourists", BindingFlags.NonPublic | BindingFlags.Instance).SetValue(touristManager, data);
        typeof(MonoWinCondition)
            .GetProperty("GetConditionPassedDays", BindingFlags.NonPublic | BindingFlags.Instance)
            .SetValue(touristManager, 10);

        MethodInfo method = typeof(TouristManager).GetMethod("SetConditionPassedDays", BindingFlags.NonPublic | BindingFlags.Instance);
        method.Invoke(touristManager, null);

        yield return null;

        int passedDays = (int)typeof(MonoWinCondition)
            .GetProperty("GetConditionPassedDays", BindingFlags.Instance | BindingFlags.NonPublic)
            .GetValue(touristManager);
        var currentStats = (MonthlyTourists)typeof(TouristManager).GetField("monthlyTourists", BindingFlags.NonPublic | BindingFlags.Instance).GetValue(touristManager);

        Assert.AreEqual(0, passedDays, "GetConditionPassedDays nem nullázódott");
        Assert.AreEqual(0, currentStats.days, "Napok nem nullázódtak");
        Assert.AreEqual(0, currentStats.tourists, "Turisták nem nullázódtak");
    }

    [UnityTest]
    public IEnumerator SetConditionPassedDays_StillInMonth_AddsDayAndTourists()
    {
        var data = new MonthlyTourists { days = 3, tourists = 20 };
        typeof(TouristManager).GetField("monthlyTourists", BindingFlags.NonPublic | BindingFlags.Instance).SetValue(touristManager, data);
        typeof(TouristManager).GetField("lastDayNewTourists", BindingFlags.NonPublic | BindingFlags.Instance).SetValue(touristManager, 7);

        MethodInfo method = typeof(TouristManager).GetMethod("SetConditionPassedDays", BindingFlags.NonPublic | BindingFlags.Instance);
        method.Invoke(touristManager, null);

        yield return null;

        var currentStats = (MonthlyTourists)typeof(TouristManager).GetField("monthlyTourists", BindingFlags.NonPublic | BindingFlags.Instance).GetValue(touristManager);

        Assert.AreEqual(4, currentStats.days, "Napok nem nõttek 1-gyel");
        Assert.AreEqual(27, currentStats.tourists, "Turisták nem adódtak hozzá helyesen");
    }

    [UnityTest]
    public IEnumerator ResetData_DeletesAllJeepGameObjects()
    {
        MethodInfo acquire = typeof(TouristManager)
            .GetMethod("AcquireNewJeep", BindingFlags.Public | BindingFlags.Instance);

        acquire.Invoke(touristManager, null);
        acquire.Invoke(touristManager, null);
        acquire.Invoke(touristManager, null);

        yield return null;

        var jeepsField = typeof(TouristManager)
            .GetField("jeeps", BindingFlags.NonPublic | BindingFlags.Instance);
        var jeeps = jeepsField.GetValue(touristManager) as IList;

        Assert.AreEqual(3, jeeps.Count, "Nem lett létrehozva 3 jeep");

        MethodInfo reset = typeof(TouristManager)
            .GetMethod("ResetData", BindingFlags.NonPublic | BindingFlags.Instance);
        reset.Invoke(touristManager, null);

        yield return null;

        for (int i = 0; i < jeeps.Count; i++)
        {
            var jeep = jeeps[i] as Jeep;
            Assert.IsTrue(jeep.ObjectInstance == null || jeep.ObjectInstance.Equals(null), $"Jeep #{i} GameObject nem lett törölve");
        }
    }
}
