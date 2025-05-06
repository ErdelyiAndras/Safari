using System.Collections;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;

public class HillFixerTests
{
    private PlacementManager placementManager;
    private PrefabManager prefabManager;
    private GameObject hillPrefab;

    [UnitySetUp]
    public IEnumerator SetUp()
    {
        hillPrefab = new GameObject("HillPrefab");

        GameObject core = GameObject.CreatePrimitive(PrimitiveType.Sphere);
        core.name = "Core";
        core.transform.SetParent(hillPrefab.transform);
        
        GameObject xStraight1 = GameObject.CreatePrimitive(PrimitiveType.Cube);
        xStraight1.name = "XStraight1";
        xStraight1.transform.SetParent(hillPrefab.transform);

        GameObject xStraight2 = GameObject.CreatePrimitive(PrimitiveType.Plane);
        xStraight2.name = "XStraight2";
        xStraight2.transform.SetParent(hillPrefab.transform);

        GameObject yStraight1 = GameObject.CreatePrimitive(PrimitiveType.Capsule);
        yStraight1.name = "YStraight1";
        yStraight1.transform.SetParent(hillPrefab.transform);

        GameObject yStraight2 = GameObject.CreatePrimitive(PrimitiveType.Cylinder);
        yStraight2.name = "YStraight2";
        yStraight2.transform.SetParent(hillPrefab.transform);

        GameObject corner1 = GameObject.CreatePrimitive(PrimitiveType.Quad);
        corner1.name = "Corner1";
        corner1.transform.SetParent(hillPrefab.transform);

        GameObject corner2 = GameObject.CreatePrimitive(PrimitiveType.Capsule);
        corner2.name = "Corner2";
        corner2.transform.SetParent(hillPrefab.transform);

        GameObject corner3 = GameObject.CreatePrimitive(PrimitiveType.Cube);
        corner3.name = "Corner3";
        corner3.transform.SetParent(hillPrefab.transform);

        GameObject corner4 = GameObject.CreatePrimitive(PrimitiveType.Quad);
        corner4.name = "Corner4";
        corner4.transform.SetParent(hillPrefab.transform);

        HillFixerScript hillFixerScript = hillPrefab.AddComponent<HillFixerScript>();
        hillFixerScript.hillPrefab = hillPrefab;

        GameObject go = new GameObject();
        placementManager = go.AddComponent<PlacementManager>();
        prefabManager = go.AddComponent<PrefabManager>();

        prefabManager.Hill = hillPrefab;

        

        placementManager.prefabManager = prefabManager;

        yield return null;
    }

    [UnityTearDown]
    public IEnumerator TearDown()
    {
        Object.Destroy(placementManager.gameObject);
        Object.Destroy(prefabManager.gameObject);
        Object.Destroy(hillPrefab);
        yield return null;
    }

    [UnityTest]
    public IEnumerator HillFixerNoNeighborTest()
    {
        Vector3Int position = new Vector3Int(0, 0, 0);
        placementManager.PlaceStructure(position, placementManager.prefabManager.Hill, CellType.Hill);
        yield return null;
        Assert.IsTrue(hillPrefab.transform.Find("Core").gameObject.activeSelf);
        yield return null;
    }

    [UnityTest]
    public IEnumerator HillFixerLeftNeighborTest()
    {
        Vector3Int position = new Vector3Int(1, 0, 0);
        placementManager.PlaceStructure(position, placementManager.prefabManager.Hill, CellType.Hill);
        yield return null;
        Vector3Int position2 = new Vector3Int(0, 0, 0);
        placementManager.PlaceStructure(position2, placementManager.prefabManager.Hill, CellType.Hill);
        Assert.IsTrue(hillPrefab.transform.Find("Core").gameObject.activeSelf);
        yield return null;

    }
    
    [UnityTest]
    public IEnumerator HillFixerRigthNeighborTest()
    {
        Vector3Int position = new Vector3Int(0, 0, 0);
        placementManager.PlaceStructure(position, placementManager.prefabManager.Hill, CellType.Hill);
        yield return null;
        Vector3Int position2 = new Vector3Int(1, 0, 0);
        placementManager.PlaceStructure(position2, placementManager.prefabManager.Hill, CellType.Hill);
        Assert.IsTrue(hillPrefab.transform.Find("Core").gameObject.activeSelf);
        yield return null;

    }

    [UnityTest]
    public IEnumerator HillFixerUpNeighborTest()
    {
        Vector3Int position = new Vector3Int(0, 0, 0);
        placementManager.PlaceStructure(position, placementManager.prefabManager.Hill, CellType.Hill);
        yield return null;
        Vector3Int position2 = new Vector3Int(0, 0, 1);
        placementManager.PlaceStructure(position2, placementManager.prefabManager.Hill, CellType.Hill);
        Assert.IsTrue(hillPrefab.transform.Find("Core").gameObject.activeSelf);
        yield return null;
    }

    [UnityTest]
    public IEnumerator HillFixerDownNeighborTest()
    {
        Vector3Int position = new Vector3Int(0, 0, 1);
        placementManager.PlaceStructure(position, placementManager.prefabManager.Hill, CellType.Hill);
        yield return null;
        Vector3Int position2 = new Vector3Int(0, 0, 0);
        placementManager.PlaceStructure(position2, placementManager.prefabManager.Hill, CellType.Hill);
        Assert.IsTrue(hillPrefab.transform.Find("Core").gameObject.activeSelf);
        yield return null;
    }

    [UnityTest]
    public IEnumerator HillFixerRigthUpNeighborTest()
    {
        Vector3Int position = new Vector3Int(0, 0, 1);
        placementManager.PlaceStructure(position, placementManager.prefabManager.Hill, CellType.Hill);
        yield return null;
        Vector3Int position2 = new Vector3Int(1, 0, 0);
        placementManager.PlaceStructure(position2, placementManager.prefabManager.Hill, CellType.Hill);
        yield return null;
        Vector3Int position3 = new Vector3Int(1, 0, 1);
        placementManager.PlaceStructure(position2, placementManager.prefabManager.Hill, CellType.Hill);
        yield return null;
        Vector3Int position4 = new Vector3Int(0, 0, 0);
        placementManager.PlaceStructure(position2, placementManager.prefabManager.Hill, CellType.Hill);
        yield return null;
        Assert.IsTrue(hillPrefab.transform.Find("Core").gameObject.activeSelf);
        yield return null;
    }

    [UnityTest]
    public IEnumerator HillFixerLeftUpNeighborTest()
    {
        Vector3Int position = new Vector3Int(0, 0, 1);
        placementManager.PlaceStructure(position, placementManager.prefabManager.Hill, CellType.Hill);
        yield return null;
        Vector3Int position2 = new Vector3Int(0, 0, 0);
        placementManager.PlaceStructure(position2, placementManager.prefabManager.Hill, CellType.Hill);
        yield return null;
        Vector3Int position3 = new Vector3Int(1, 0, 1);
        placementManager.PlaceStructure(position2, placementManager.prefabManager.Hill, CellType.Hill);
        yield return null;
        Vector3Int position4 = new Vector3Int(1, 0, 0);
        placementManager.PlaceStructure(position2, placementManager.prefabManager.Hill, CellType.Hill);
        yield return null; 
        Assert.IsTrue(hillPrefab.transform.Find("Core").gameObject.activeSelf);
        yield return null;
    }

    [UnityTest]
    public IEnumerator HillFixerRigthDownNeighborTest()
    {
        Vector3Int position = new Vector3Int(0, 0, 0);
        placementManager.PlaceStructure(position, placementManager.prefabManager.Hill, CellType.Hill);
        yield return null;
        Vector3Int position2 = new Vector3Int(1, 0, 0);
        placementManager.PlaceStructure(position2, placementManager.prefabManager.Hill, CellType.Hill);
        yield return null;
        Vector3Int position3 = new Vector3Int(1, 0, 1);
        placementManager.PlaceStructure(position2, placementManager.prefabManager.Hill, CellType.Hill);
        yield return null;
        Vector3Int position4 = new Vector3Int(0, 0, 1);
        placementManager.PlaceStructure(position2, placementManager.prefabManager.Hill, CellType.Hill);
        yield return null;
        Assert.IsTrue(hillPrefab.transform.Find("Core").gameObject.activeSelf);
        yield return null;
    }

    [UnityTest]
    public IEnumerator HillFixerLeftDownNeighborTest()
    {
        Vector3Int position = new Vector3Int(0, 0, 0);
        placementManager.PlaceStructure(position, placementManager.prefabManager.Hill, CellType.Hill);
        yield return null;
        Vector3Int position2 = new Vector3Int(1, 0, 0);
        placementManager.PlaceStructure(position2, placementManager.prefabManager.Hill, CellType.Hill);
        yield return null;
        Vector3Int position3 = new Vector3Int(0, 0, 1);
        placementManager.PlaceStructure(position2, placementManager.prefabManager.Hill, CellType.Hill);
        yield return null;
        Vector3Int position4 = new Vector3Int(1, 0, 1);
        placementManager.PlaceStructure(position2, placementManager.prefabManager.Hill, CellType.Hill);
        yield return null;
        Assert.IsTrue(hillPrefab.transform.Find("Core").gameObject.activeSelf);
        yield return null;
    }
}
