using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;

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

}
