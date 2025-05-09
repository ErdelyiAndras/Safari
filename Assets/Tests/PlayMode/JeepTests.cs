using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;

public class JeepPlayModeTests
{
    private PlacementManager placementManager;
    private TouristManager touristManager;
    private GameObject jeepPrefab;

    [UnitySetUp]
    public IEnumerator SetUp()
    {
        var placementGO = new GameObject("PlacementManager");
        placementManager = placementGO.AddComponent<PlacementManager>();

        var prefabManagerGO = new GameObject("PrefabManager");
        var prefabManager = prefabManagerGO.AddComponent<PrefabManager>();

        jeepPrefab = GameObject.CreatePrimitive(PrimitiveType.Cube);
        prefabManager.JeepPrefab = jeepPrefab;

        placementManager.prefabManager = prefabManager;

        var touristGO = new GameObject("TouristManager");
        touristManager = touristGO.AddComponent<TouristManager>();
        touristManager.placementManager = placementManager;

        yield return null;
    }

    [UnityTest]
    public IEnumerator JeepCreatesObjectInstance()
    {
        var jeep = new Jeep(placementManager, jeepPrefab, touristManager);

        Assert.IsNotNull(jeep.ObjectInstance, "Jeep ObjectInstance nem jött létre!");

        yield return null;
    }

    [UnityTest]
    public IEnumerator JeepObjectInstancePositionCanChange()
    {
        var jeep = new Jeep(placementManager, jeepPrefab, touristManager);

        Vector3 startPos = jeep.ObjectInstance.transform.position;

        jeep.ObjectInstance.transform.position = new Vector3(5f, 0f, 0f);
        yield return null;

        Vector3 newPos = jeep.ObjectInstance.transform.position;

        Assert.AreNotEqual(startPos, newPos, "A pozíció nem változott meg");
    }

    [UnityTest]
    public IEnumerator JeepMovesWithManualPath()
    {
        var jeep = new Jeep(placementManager, jeepPrefab, touristManager);

        jeep.ObjectInstance.transform.position = new Vector3(0.1f, 0f, 0f);
        Entity.SpeedMultiplier = 1f;

        var manualPath = new List<Vector3Int>
        {
            new Vector3Int(0, 0, 0),
            new Vector3Int(1, 0, 0),
            new Vector3Int(2, 0, 0)
        };

        var jeepType = typeof(Jeep);
        var pathField = jeepType.GetField("jeepPath", BindingFlags.NonPublic | BindingFlags.Instance);
        var indexField = jeepType.GetField("currentPathIndex", BindingFlags.NonPublic | BindingFlags.Instance);
        var moveMethod = jeepType.GetMethod("Move", BindingFlags.NonPublic | BindingFlags.Instance);

        pathField.SetValue(jeep, manualPath);
        indexField.SetValue(jeep, 1);

        Vector3 startPos = jeep.Position;

        for (int i = 0; i < 30; i++)
        {
            moveMethod.Invoke(jeep, null);
            yield return new WaitForSecondsRealtime(0.05f);
        }

        Vector3 endPos = jeep.Position;
        Assert.Greater(endPos.x, startPos.x + 0.1f, "Jeep nem mozdult el");
    }

    [UnityTest]
    public IEnumerator JeepMovesInMovingState()
    {
        var jeep = new Jeep(placementManager, jeepPrefab, touristManager);
        Entity.SpeedMultiplier = 1f;

        placementManager.startPosition = new Vector3Int(0, 0, 0);
        placementManager.endPosition = new Vector3Int(5, 0, 0);

        for (int x = 0; x <= 5; x++) placementManager.PlaceStructure(new Vector3Int(x, 0, 0), jeepPrefab, CellType.Road);

        placementManager.HasFullPathProperty = true;

        for (int i = 0; i < 4; i++) jeep.tourists.AddTourist();

        yield return null;

        jeep.CheckState();
        Assert.AreEqual(Jeep.State.Moving, jeep.MyState, "Jeep nem váltott át Moving állapotra");

        Vector3 startPos = jeep.Position;

        for (int i = 0; i < 50; i++)
        {
            jeep.CheckState();
            yield return new WaitForSecondsRealtime(0.05f);
        }

        Vector3 endPos = jeep.Position;

        Assert.Greater(endPos.x, startPos.x + 0.1f, "Jeep nem mozdult el a Moving állapotban");
    }

    [UnityTest]
    public IEnumerator CheckStateMoving1()
    {
        var jeep = new Jeep(placementManager, jeepPrefab, touristManager);
        Entity.SpeedMultiplier = 1f;

        placementManager.startPosition = new Vector3Int(0, 0, 0);
        placementManager.endPosition = new Vector3Int(1, 0, 0);

        for (int x = 0; x <= 1; x++) placementManager.PlaceStructure(new Vector3Int(x, 0, 0), jeepPrefab, CellType.Road);

        placementManager.HasFullPathProperty = true;

        for (int i = 0; i < 4; i++) jeep.tourists.AddTourist();

        yield return null;

        jeep.CheckState();

        for (int i = 0; i < 100; i++)
        {
            jeep.CheckState();
            if (jeep.MyState == Jeep.State.Leaving)
            {
                break;
            }
            yield return new WaitForSecondsRealtime(0.05f);
        }

        Assert.AreEqual(Jeep.State.Leaving, jeep.MyState, "Jeep nem váltott át Leaving állapotra, amikor elérte az útvonal végét");
    }

    [UnityTest]
    public IEnumerator CheckStateMoving2()
    {
        var jeep = new Jeep(placementManager, jeepPrefab, touristManager);
        Entity.SpeedMultiplier = 1f;

        placementManager.startPosition = new Vector3Int(0, 0, 0);
        placementManager.endPosition = new Vector3Int(5, 0, 0);

        for (int x = 0; x <= 5; x++) placementManager.PlaceStructure(new Vector3Int(x, 0, 0), jeepPrefab, CellType.Road);

        placementManager.HasFullPathProperty = true;

        for (int i = 0; i < 4; i++) jeep.tourists.AddTourist();

        yield return null;

        jeep.CheckState();
        Assert.AreEqual(Jeep.State.Moving, jeep.MyState);

        var pathField = typeof(Jeep).GetField("jeepPath", BindingFlags.NonPublic | BindingFlags.Instance);
        pathField.SetValue(jeep, null);

        Vector3 startPos = jeep.Position;

        for (int i = 0; i < 30; i++)
        {
            jeep.CheckState();
            yield return new WaitForSecondsRealtime(0.05f);
        }

        Vector3 endPos = jeep.Position;

        Assert.Greater(endPos.x, startPos.x + 0.1f, "Jeep nem mozdult el miután path == null és CheckState újra meghívódott");
    }

    [UnityTest]
    public IEnumerator CheckStateMoving3()
    {
        var jeep = new Jeep(placementManager, jeepPrefab, touristManager);
        Entity.SpeedMultiplier = 1f;

        placementManager.startPosition = new Vector3Int(0, 0, 0);
        placementManager.endPosition = new Vector3Int(5, 0, 0);

        for (int x = 0; x <= 5; x++) placementManager.PlaceStructure(new Vector3Int(x, 0, 0), jeepPrefab, CellType.Road);

        placementManager.HasFullPathProperty = true;

        for (int i = 0; i < 4; i++) jeep.tourists.AddTourist();

        yield return null;

        jeep.CheckState();
        Assert.AreEqual(Jeep.State.Moving, jeep.MyState);

        yield return new WaitForSecondsRealtime(0.1f);

        var pathField = typeof(Jeep).GetField("jeepPath", BindingFlags.NonPublic | BindingFlags.Instance);
        var path = pathField.GetValue(jeep) as List<Vector3Int>;

        Assert.IsNotNull(path, "Path null, pedig már létrejött");
        Assert.Greater(path.Count, 1, "Path túl rövid");

        Vector3 startPos = jeep.Position;

        for (int i = 0; i < 30; i++)
        {
            jeep.CheckState();
            yield return new WaitForSecondsRealtime(0.05f);
        }

        Vector3 endPos = jeep.Position;

        Assert.Greater(endPos.x, startPos.x + 0.1f, "Jeep nem mozdult tovább, pedig volt útvonala és Moving állapotban volt");
    }

    [UnityTest]
    public IEnumerator CheckStateWaiting()
    {
        var jeep = new Jeep(placementManager, jeepPrefab, touristManager);

        for (int i = 0; i < 3; i++) jeep.tourists.AddTourist();

        jeep.CheckState();

        Assert.AreEqual(Jeep.State.Waiting, jeep.MyState, "Jeep elindult kevesebb mint 4 turistával");
        yield return null;
    }

    [UnityTest]
    public IEnumerator CheckStateLeaving()
    {
        var jeep = new Jeep(placementManager, jeepPrefab, touristManager);
        Entity.SpeedMultiplier = 1f;

        for (int i = 0; i < 4; i++) jeep.tourists.AddTourist();
        Assert.IsTrue(jeep.tourists.IsTouristGroupFull(), "Turisták száma nem elég a teszthez");

        typeof(Jeep).GetProperty("MyState").SetValue(jeep, Jeep.State.Leaving);

        jeep.CheckState();
        yield return null;

        Assert.AreEqual(Jeep.State.Returning, jeep.MyState, "Jeep nem váltott át Returning állapotra");
        Assert.IsFalse(jeep.tourists.IsTouristGroupFull(), "Turisták nem lettek alaphelyzetbe állítva");
    }

    [UnityTest]
    public IEnumerator CheckStateReturning1()
    {
        var jeep = new Jeep(placementManager, jeepPrefab, touristManager);
        Entity.SpeedMultiplier = 1f;

        placementManager.startPosition = new Vector3Int(0, 0, 0);
        placementManager.endPosition = new Vector3Int(1, 0, 0);

        for (int x = 0; x <= 1; x++) placementManager.PlaceStructure(new Vector3Int(x, 0, 0), jeepPrefab, CellType.Road);

        placementManager.HasFullPathProperty = true;

        for (int i = 0; i < 4; i++) jeep.tourists.AddTourist();

        yield return null;

        jeep.CheckState();

        for (int i = 0; i < 100; i++)
        {
            jeep.CheckState();
            if (jeep.MyState == Jeep.State.Leaving)
                break;
            yield return new WaitForSecondsRealtime(0.05f);
        }

        jeep.CheckState();

        for (int i = 0; i < 100; i++)
        {
            jeep.CheckState();
            if (jeep.MyState == Jeep.State.Waiting)
                break;
            yield return new WaitForSecondsRealtime(0.05f);
        }

        Assert.AreEqual(Jeep.State.Waiting, jeep.MyState, "Jeep nem váltott vissza Waiting állapotra");
    }

    [UnityTest]
    public IEnumerator CheckStateReturning2()
    {
        var jeep = new Jeep(placementManager, jeepPrefab, touristManager);
        Entity.SpeedMultiplier = 1f;

        placementManager.startPosition = new Vector3Int(0, 0, 0);
        placementManager.endPosition = new Vector3Int(5, 0, 0);

        for (int x = 0; x <= 5; x++) placementManager.PlaceStructure(new Vector3Int(x, 0, 0), jeepPrefab, CellType.Road);

        placementManager.HasFullPathProperty = true;

        for (int i = 0; i < 4; i++) jeep.tourists.AddTourist();

        yield return null;

        jeep.CheckState();

        for (int i = 0; i < 100; i++)
        {
            jeep.CheckState();
            if (jeep.MyState == Jeep.State.Leaving)
                break;
            yield return new WaitForSecondsRealtime(0.05f);
        }

        jeep.CheckState();

        typeof(Jeep).GetField("jeepPath", BindingFlags.NonPublic | BindingFlags.Instance).SetValue(jeep, null);

        Vector3 startPos = jeep.Position;

        for (int i = 0; i < 30; i++)
        {
            jeep.CheckState();
            yield return new WaitForSecondsRealtime(0.05f);
        }

        Vector3 endPos = jeep.Position;

        Assert.Greater(Mathf.Abs(endPos.x - startPos.x), 0.1f, "Jeep nem mozdult el miután path == null Returning állapotban");
    }

    [UnityTest]
    public IEnumerator CheckStateReturning3()
    {
        var jeep = new Jeep(placementManager, jeepPrefab, touristManager);
        Entity.SpeedMultiplier = 1f;

        placementManager.startPosition = new Vector3Int(0, 0, 0);
        placementManager.endPosition = new Vector3Int(5, 0, 0);

        for (int x = 0; x <= 5; x++) placementManager.PlaceStructure(new Vector3Int(x, 0, 0), jeepPrefab, CellType.Road);

        placementManager.HasFullPathProperty = true;

        for (int i = 0; i < 4; i++) jeep.tourists.AddTourist();

        yield return null;

        jeep.CheckState();

        for (int i = 0; i < 100; i++)
        {
            jeep.CheckState();
            if (jeep.MyState == Jeep.State.Leaving)
                break;
            yield return new WaitForSecondsRealtime(0.05f);
        }

        jeep.CheckState();

        yield return new WaitForSecondsRealtime(0.1f);

        var pathField = typeof(Jeep).GetField("jeepPath", BindingFlags.NonPublic | BindingFlags.Instance);
        var path = pathField.GetValue(jeep) as List<Vector3Int>;

        Assert.IsNotNull(path, "Path null, pedig már létrejött Returning állapotban");
        Assert.Greater(path.Count, 1, "Path túl rövid Returning állapotban");

        Vector3 startPos = jeep.Position;

        for (int i = 0; i < 30; i++)
        {
            jeep.CheckState();
            yield return new WaitForSecondsRealtime(0.05f);
        }

        Vector3 endPos = jeep.Position;

        Assert.Greater(Mathf.Abs(endPos.x - startPos.x), 0.1f, "Jeep nem mozdult Returning állapotban, pedig volt útvonala");
    }
}

