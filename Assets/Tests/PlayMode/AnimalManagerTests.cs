using System;
using System.Collections;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;

public class AnimalManagerTests
{
    private AnimalManager animalManager;

    [UnitySetUp]
    public IEnumerator Setup()
    {
        yield return null;
    }

    [UnityTearDown]
    public IEnumerator TearDown()
    {
        //Object.Destroy(animalManager.gameObject);
        yield return null;
    }

    private AnimalManager AnimalManagerFactory()
    {
        DifficultySelector.SelectedDifficulty = Difficulty.Normal;
        var go = new GameObject();
        AnimalManager animalManager = go.AddComponent<AnimalManager>();

        var placementManagerGO = new GameObject();
        var placementManager = placementManagerGO.AddComponent<PlacementManager>();
        var prefabManager = placementManagerGO.AddComponent<PrefabManager>();

        prefabManager.Carnivore1Prefab = new GameObject("Carnivore1Prefab");
        prefabManager.Carnivore2Prefab = new GameObject("Carnivore2Prefab");
        prefabManager.Herbivore1Prefab = new GameObject("Herbivore1Prefab");
        prefabManager.Herbivore2Prefab = new GameObject("Herbivore2Prefab");

        placementManager.prefabManager = prefabManager;

        animalManager.placementManager = placementManager;

        return animalManager;
    }
    
    private void DestroyAnimalManager()
    {
        UnityEngine.Object.Destroy(animalManager.gameObject);
    }

    [UnityTest]
    public IEnumerator AnimalManagerGameStartAnimalsSpawnTest()
    {
        var testCases = new[]
        {
            new { Difficulty = Difficulty.Easy },
            new { Difficulty = Difficulty.Normal },
            new { Difficulty = Difficulty.Hard}
        };
        foreach (var testCase in testCases)
        {
            DifficultySelector.SelectedDifficulty = testCase.Difficulty;
            animalManager = AnimalManagerFactory();
            yield return null;
            Assert.AreEqual(Constants.StartAnimalSpwanCount[AnimalType.Carnivore1], animalManager.Carnivore1Count);
            Assert.AreEqual(Constants.StartAnimalSpwanCount[AnimalType.Carnivore2], animalManager.Carnivore2Count);
            Assert.AreEqual(Constants.StartAnimalSpwanCount[AnimalType.Herbivore1], animalManager.Herbivore1Count);
            Assert.AreEqual(Constants.StartAnimalSpwanCount[AnimalType.Herbivore2], animalManager.Herbivore2Count);
            yield return null;
        }
        DestroyAnimalManager();
    }

    [UnityTest]
    public IEnumerator AnimalManagerReproduceTest()
    {
        Constants.StartCarnivoreSpawnDifficultyMultiplier[Difficulty.Normal] = 0;
        Constants.StartHerbivoreSpawnDifficultyMultiplier[Difficulty.Normal] = 0;
        Constants.ReproductionCooldown[AnimalType.Herbivore1] = 45;
        Constants.ReproductionCooldown[AnimalType.Herbivore2] = 45;
        Constants.ReproductionCooldown[AnimalType.Carnivore1] = 45;
        Constants.ReproductionCooldown[AnimalType.Carnivore2] = 45;
        yield return null;
        animalManager = AnimalManagerFactory();
        yield return null;
        Assert.AreEqual(0, animalManager.AllAnimalCount);
        var testCases = new (AnimalType, Action, Func<uint>)[]
        {
            (AnimalType.Herbivore1, animalManager.BuyHerbivore1, () => animalManager.Herbivore1Count),
            (AnimalType.Herbivore2, animalManager.BuyHerbivore2, () => animalManager.Herbivore2Count),
            (AnimalType.Carnivore1, animalManager.BuyCarnivore1, () => animalManager.Carnivore1Count),
            (AnimalType.Carnivore2, animalManager.BuyCarnivore2, () => animalManager.Carnivore2Count)
        };
        foreach (var testCase in testCases) {
            Assert.AreEqual(0, animalManager.AllAnimalCount);
            for(int i = 0; i < 10; ++i)
            {
                testCase.Item2();
            }
            yield return null;
            Assert.AreEqual(10, animalManager.AllAnimalCount);
            for (int i = 0; i <= Constants.MaxLifeTime[testCase.Item1] * Constants.AdultLifetimeThreshold[testCase.Item1]; ++i)
            {
                animalManager.ManageTick();
            }
            yield return null;
            Assert.IsTrue(11 <= animalManager.AllAnimalCount);
            for (int i = 0; i < 200; ++i)
            {
                animalManager.ManageTick();
            }
            yield return null;
        }
        DestroyAnimalManager();
    }

    [UnityTest]
    public IEnumerator AnimalManagerGameOverTest()
    {
        Constants.ReproductionCooldown[AnimalType.Herbivore1] = 45;
        Constants.ReproductionCooldown[AnimalType.Herbivore2] = 45;
        Constants.ReproductionCooldown[AnimalType.Carnivore1] = 45;
        Constants.ReproductionCooldown[AnimalType.Carnivore2] = 45;
        bool extinct = false;
        animalManager = AnimalManagerFactory();
        yield return null;
        animalManager.GameOver += () => extinct = true;
        Assert.AreNotEqual(0, animalManager.AllAnimalCount);
        for (int i = 0; i < 250; ++i)
        {
            animalManager.ManageTick();
        }
        yield return null;
        Assert.IsTrue(extinct);
        yield return null;
        DestroyAnimalManager();
    }
    [UnityTest]
    public IEnumerator AnimalLoadTest()
    {
        animalManager = AnimalManagerFactory();
        yield return null;
        Assert.IsTrue(animalManager.AllAnimalCount != 0);
        uint countBeforeSave = animalManager.AllAnimalCount;
        AnimalManagerData saved = animalManager.SaveData();
        Constants.StartCarnivoreSpawnDifficultyMultiplier[Difficulty.Normal] = 0;
        Constants.StartHerbivoreSpawnDifficultyMultiplier[Difficulty.Normal] = 0;
        animalManager = AnimalManagerFactory();
        yield return null;
        Assert.IsTrue(animalManager.AllAnimalCount == 0);
        animalManager.LoadData(saved, animalManager.placementManager);
        yield return null;
        Assert.IsTrue(animalManager.AllAnimalCount == countBeforeSave);
        Constants.StartCarnivoreSpawnDifficultyMultiplier[Difficulty.Normal] = 2;
        Constants.StartHerbivoreSpawnDifficultyMultiplier[Difficulty.Normal] = 2;
  
    }
}
