using System.Collections;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;

public class PlacementManagerTests
{
    private PlacementManager placementManager;

    [UnitySetUp]
    public IEnumerator Setup()
    {
        yield return null;
    }

    [UnityTearDown]
    public IEnumerator TearDown()
    {
        yield return null;
    }

    private PlacementManager PlacementManagerFactory()
    {
        DifficultySelector.SelectedDifficulty = Difficulty.Normal;
        
        var placementManagerGO = new GameObject();
        var placementManager = placementManagerGO.AddComponent<PlacementManager>();
        var prefabManager = placementManagerGO.AddComponent<PrefabManager>();

        prefabManager.Carnivore1Prefab = new GameObject("Carnivore1Prefab");
        prefabManager.Carnivore2Prefab = new GameObject("Carnivore2Prefab");
        prefabManager.Herbivore1Prefab = new GameObject("Herbivore1Prefab");
        prefabManager.Herbivore2Prefab = new GameObject("Herbivore2Prefab");

        placementManager.prefabManager = prefabManager;

        return placementManager;
    }

    private void DestroyPlacementManager()
    {
        UnityEngine.Object.Destroy(placementManager.gameObject);
    }

    [UnityTest]
    public IEnumerator PlacementManagerIsPositionFreeForTest()
    {
        var testCases = new[]
        {
            new { type = CellType.Road },
            new { type = CellType.Nature},
            new { type = CellType.Water},
            new { type = CellType.Empty},
        };
        
        placementManager = PlacementManagerFactory();
        yield return null;
        Vector3Int placedRoad = new Vector3Int(5,0,5);
        Vector3Int placedNature = new Vector3Int(6,0,6);
        Vector3Int placedWater = new Vector3Int(7,0,7);
        placementManager.PlaceStructure(placedRoad, new GameObject("Road"), CellType.Road);
        placementManager.PlaceStructure(placedNature, new GameObject("Nature"), CellType.Nature);
        placementManager.PlaceStructure(placedWater, new GameObject("Water"), CellType.Water);
        yield return null;
        foreach (var testCase in testCases)
        {
            Assert.AreEqual(false, placementManager.CheckIfPositionIsFreeFor(placedRoad, testCase.type));
            Assert.AreEqual(false, placementManager.CheckIfPositionIsFreeFor(placedWater, testCase.type));
            if (testCase.type != CellType.Empty){
                Assert.AreEqual(true, placementManager.CheckIfPositionIsFreeFor(new Vector3Int(8,0,8), testCase.type));
            }
            if (testCase.type != CellType.Water)
            {
                Assert.AreEqual(false, placementManager.CheckIfPositionIsFreeFor(placedNature, testCase.type));
            }else{
                Assert.AreEqual(true, placementManager.CheckIfPositionIsFreeFor(placedNature, testCase.type));
            }
        }
        DestroyPlacementManager();
    }

    [UnityTest]
    public IEnumerator PlacementManagerRemoveStructureTest(){
        placementManager = PlacementManagerFactory();
        yield return null;
        Vector3Int placedRoad = new Vector3Int(5,0,5);
        Vector3Int placedFixRoad = new Vector3Int(0,0,0);
        Vector3Int placedNature = new Vector3Int(6,0,6);
        Vector3Int placedWater = new Vector3Int(7,0,7);
        placementManager.PlaceStructure(placedRoad, new GameObject("Road"), CellType.Road);
        placementManager.PlaceStructure(placedNature, new GameObject("Nature"), CellType.Nature);
        placementManager.PlaceStructure(placedWater, new GameObject("Water"), CellType.Water);
        placementManager.PlaceStructure(placedFixRoad, new GameObject("FixRoad"), CellType.Road);
        yield return null;
        Assert.AreEqual(CellType.Road, placementManager.GetTypeOfPosition(placedRoad));
        Assert.AreEqual(CellType.Nature, placementManager.GetTypeOfPosition(placedNature));
        Assert.AreEqual(CellType.Water, placementManager.GetTypeOfPosition(placedWater));
        Assert.AreEqual(CellType.Road, placementManager.GetTypeOfPosition(new Vector3Int(0,0,0)));
        placementManager.RemoveStructure(placedRoad);
        placementManager.RemoveStructure(placedNature);
        placementManager.RemoveStructure(placedWater);
        placementManager.RemoveStructure(new Vector3Int(0,0,0));
        yield return null;
        Assert.AreEqual(CellType.Empty, placementManager.GetTypeOfPosition(placedRoad));
        Assert.AreEqual(CellType.Empty, placementManager.GetTypeOfPosition(placedNature));
        Assert.AreEqual(CellType.Empty, placementManager.GetTypeOfPosition(placedWater));
        Assert.AreEqual(CellType.Road, placementManager.GetTypeOfPosition(new Vector3Int(0,0,0)));

        DestroyPlacementManager();
    }

    [UnityTest]
    public IEnumerator PlacementManagerDestroyNatureAtTest(){
        placementManager = PlacementManagerFactory();
        yield return null;
        Vector3Int placedNature = new Vector3Int(6,0,6);
        placementManager.PlaceStructure(placedNature, new GameObject("Nature"), CellType.Nature);
        yield return null;
        Assert.AreEqual(CellType.Nature, placementManager.GetTypeOfPosition(placedNature));
        placementManager.DestroyNatureAt(placedNature);
        yield return null;
        placementManager.PlaceStructure(placedNature, new GameObject("Nature"), CellType.Nature);
        DestroyPlacementManager();
    }

}
