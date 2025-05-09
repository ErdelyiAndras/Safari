using System.Collections;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;

public class AnimalTests
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

        for(int i = 0; i < 50; ++i){
            Vector3Int temporatyPosition;
            do{
                int randomX = UnityEngine.Random.Range(0, placementManager.Width);
                int randomZ = UnityEngine.Random.Range(0, placementManager.Height);
                temporatyPosition = new Vector3Int(randomX, 0, randomZ);
            
            }while (!placementManager.CheckIfPositionIsOfType(temporatyPosition, CellType.Empty));
            placementManager.PlaceStructure(temporatyPosition, new GameObject("Water"), CellType.Water);
        }
        for(int i = 0; i < 20; ++i){
            Vector3Int temporatyPosition;
            do{
                int randomX = UnityEngine.Random.Range(0, placementManager.Width);
                int randomZ = UnityEngine.Random.Range(0, placementManager.Height);
                temporatyPosition = new Vector3Int(randomX, 0, randomZ);
            
            }while (!placementManager.CheckIfPositionIsOfType(temporatyPosition, CellType.Empty));
            placementManager.PlaceStructure(temporatyPosition, new GameObject("Nature"), CellType.Nature);
        }

        animalManager.placementManager = placementManager;

        return animalManager;
    }
    
    private void DestroyAnimalManager()
    {
        UnityEngine.Object.Destroy(animalManager.gameObject);
    }

    
   [UnityTest]
    public IEnumerator AnimalEatTest()
    {
        Constants.StartCarnivoreSpawnDifficultyMultiplier[Difficulty.Normal] = 10;
        Constants.StartHerbivoreSpawnDifficultyMultiplier[Difficulty.Normal] = 10;
        Constants.FoodThreshold[AnimalType.Herbivore1] = 0.99f;
        Constants.FoodThreshold[AnimalType.Herbivore2] = 0.99f;
        Constants.FoodThreshold[AnimalType.Carnivore1] = 0.99f;
        Constants.FoodThreshold[AnimalType.Carnivore2] = 0.99f;
        Constants.ReproductionCooldown[AnimalType.Herbivore1] = 500;
        Constants.ReproductionCooldown[AnimalType.Herbivore2] = 500;
        Constants.ReproductionCooldown[AnimalType.Carnivore1] = 500;
        Constants.ReproductionCooldown[AnimalType.Carnivore2] = 500;
        yield return null;
        animalManager = AnimalManagerFactory();
        yield return null;
        //animalManager.BuyHerbivore1();
        //yield return null;
        uint animalCount = animalManager.AllAnimalCount;
        Debug.Log("Animal Count: " + animalCount);
        Assert.AreNotEqual(0, animalCount);
        for(int i = 0; i < 5 ; ++i){
            animalManager.ManageTick();
        }    
        for(int i = 0; i < 100 ; ++i){
            yield return null;
        }
        Debug.Log("Animal Count After: " + animalManager.AllAnimalCount);
        Assert.IsTrue(animalManager.AllAnimalCount == animalCount);
    }
    
    [UnityTest]
    public IEnumerator AnimalDrinkTest()
    {
        Constants.StartCarnivoreSpawnDifficultyMultiplier[Difficulty.Normal] = 5;
        Constants.StartHerbivoreSpawnDifficultyMultiplier[Difficulty.Normal] = 5;
        Constants.MaxDrink[AnimalType.Herbivore1] = 100.0f;
        Constants.MaxDrink[AnimalType.Herbivore2] = 100.0f;
        Constants.MaxDrink[AnimalType.Carnivore1] = 100.0f;
        Constants.MaxDrink[AnimalType.Carnivore2] = 100.0f;
        Constants.AnimalBaseMoveSpeed[AnimalType.Herbivore1] = 10.0f;
        Constants.AnimalBaseMoveSpeed[AnimalType.Herbivore2] = 10.0f;
        Constants.AnimalBaseMoveSpeed[AnimalType.Carnivore1] = 10.0f;
        Constants.AnimalBaseMoveSpeed[AnimalType.Carnivore2] = 10.0f;
        Constants.DrinkThreshold[AnimalType.Herbivore1] = 0.98f;
        Constants.DrinkThreshold[AnimalType.Herbivore2] = 0.98f;
        Constants.DrinkThreshold[AnimalType.Carnivore1] = 0.98f;
        Constants.DrinkThreshold[AnimalType.Carnivore2] = 0.98f;
        Constants.FoodThreshold[AnimalType.Herbivore1] = 0.1f;
        Constants.FoodThreshold[AnimalType.Herbivore2] = 0.1f;
        Constants.FoodThreshold[AnimalType.Carnivore1] = 0.1f;
        Constants.FoodThreshold[AnimalType.Carnivore2] = 0.1f;
        Constants.ReproductionCooldown[AnimalType.Herbivore1] = 500;
        Constants.ReproductionCooldown[AnimalType.Herbivore2] = 500;
        Constants.ReproductionCooldown[AnimalType.Carnivore1] = 500;
        Constants.ReproductionCooldown[AnimalType.Carnivore2] = 500;
        yield return null;
        animalManager = AnimalManagerFactory();
        yield return null;
        uint animalCount = animalManager.AllAnimalCount;
        Assert.AreNotEqual(0, animalCount);
        for(int i = 0; i < 10 ; ++i){
            animalManager.ManageTick();
            for(int j = 0; j < 20; ++j){
                yield return null;
            }
        }
        Debug.Log("Animal Count After: " + animalManager.AllAnimalCount);
        Assert.IsTrue(animalManager.AllAnimalCount == animalCount);
    }
    [UnityTest]
    public IEnumerator AnimalLoadTest()
    {
        Constants.StartCarnivoreSpawnDifficultyMultiplier[Difficulty.Normal] = 5;
        Constants.StartHerbivoreSpawnDifficultyMultiplier[Difficulty.Normal] = 5;
        yield return null;
        animalManager = AnimalManagerFactory();
        yield return null;
        var data = animalManager.SaveData();
        var herds = data.Herds(animalManager.placementManager, animalManager);
        yield return null;
        Carnivore1 carnivore1 = new Carnivore1(new GameObject("carnivore1"), animalManager.placementManager, herds[0].Id);
        Carnivore2 carnivore2 = new Carnivore2(new GameObject("carnivore2"), animalManager.placementManager, herds[0].Id);
        Herbivore1 herbivore1 = new Herbivore1(new GameObject("carnivore1"), animalManager.placementManager, herds[0].Id);
        Herbivore2 herbivore2 = new Herbivore2(new GameObject("carnivore1"), animalManager.placementManager, herds[0].Id);
        
        yield return null;
        for (int i = 0; i < 10; ++i){
            animalManager.ManageTick();
            yield return null;
        }
        yield return new WaitForSecondsRealtime(5.0f);
        yield return null;
        EntityData a = carnivore1.SaveData();
        EntityData b = carnivore2.SaveData();
        EntityData c = herbivore1.SaveData();
        EntityData d = herbivore2.SaveData();
        carnivore1 = new Carnivore1(new GameObject("carnivore1"), animalManager.placementManager, herds[0].Id);
        carnivore2 = new Carnivore2(new GameObject("carnivore2"), animalManager.placementManager, herds[0].Id);
        herbivore1 = new Herbivore1(new GameObject("carnivore1"), animalManager.placementManager, herds[0].Id);
        herbivore2 = new Herbivore2(new GameObject("carnivore1"), animalManager.placementManager, herds[0].Id);
        EntityData aa = carnivore1.SaveData();
        EntityData bb = carnivore2.SaveData();
        EntityData cc = herbivore1.SaveData();
        EntityData dd = herbivore2.SaveData();
        Assert.AreNotEqual(a, aa);
        Assert.AreNotEqual(b, bb);
        carnivore1.LoadData(a, animalManager.placementManager);
        carnivore2.LoadData(b, animalManager.placementManager);
        herbivore1.LoadData(c, animalManager.placementManager);
        herbivore2.LoadData(d, animalManager.placementManager);
    }

}
