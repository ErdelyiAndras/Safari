using NUnit;
using System;
using System.Collections.Generic;
using UnityEngine;

public class PlacementManager : MonoBehaviour, ISaveable<PlacementManagerData>
{
    public static int Width { get; private set; } = Constants.MapWidth;
    public static int Height { get; private set; } = Constants.MapHeight;
    public Grid placementGrid { get; private set; }
    public PrefabManager prefabManager;
    public Action<Vector3Int> RoadRemoved;

    private Dictionary<Vector3Int, StructureModel> temporaryRoadobjects = new Dictionary<Vector3Int, StructureModel>();
    private Dictionary<Vector3Int, StructureModel> structureDictionary = new Dictionary<Vector3Int, StructureModel>();
    public PlacedObjects PlacedObjects { get; private set; } = new PlacedObjects();
    public static readonly Vector3Int startPosition = new Vector3Int(0, 0, 0), endPosition = new Vector3Int(Width - 1, 0, Height -1);

    public bool HasFullPathProperty { get; set; }

    private void Awake()
    {
        placementGrid = new Grid(Width, Height);
    }

    public void RegisterObject(Guid id, ObjectType type, IPositionable entity)
    {
        SafariObject obj = new SafariObject
        {
            guid = id,
            type = type,
            entity = entity
        };
        PlacedObjects.AddObject(obj);
    }

    public bool CheckIfPositionInBound(Vector3Int position) => position.x >= 0 && position.x < Width && position.z >= 0 && position.z < Height;

    public bool IsPositionWalkable(Vector3Int position) => placementGrid[position.x, position.z] != CellType.Water;

    public bool CheckIfPositionIsFreeFor(Vector3Int position, CellType type) 
    {
        if (type == CellType.Road)
        {
            return CheckIfPositionIsOfType(position, CellType.Empty);
        }
        else if (type == CellType.Nature)
        {
            return CheckIfPositionIsOfType(position, CellType.Empty);
        }
        else if (type == CellType.Water)
        {
            return CheckIfPositionIsOfType(position, CellType.Empty) ||  CheckIfPositionIsOfType(position, CellType.Nature);
        }
        return false;
    } 

    public CellType GetTypeOfPosition(Vector3Int position) => placementGrid[position.x, position.z];

    public bool CheckIfPositionIsOfType(Vector3Int position, CellType type) => placementGrid[position.x, position.z] == type;

    public void PlaceTemporaryStructure(Vector3Int position, GameObject structurePrefab, CellType type)
    {
        placementGrid[position.x, position.z] = type;
        StructureModel structure = CreateANewStructureModel(position, structurePrefab, type);
        temporaryRoadobjects.Add(position, structure);
    }

    public void RemoveStructure(Vector3Int position)
    {

        if (!CheckIfPositionInBound(position) || CheckIfPositionIsUnremovable(position))
            return;
        if (structureDictionary.ContainsKey(position))
        {
            if (GetTypeOfPosition(position) != CellType.Hill && GetTypeOfPosition(position) != CellType.Empty)
            {
                Destroy(structureDictionary[position].gameObject);
                structureDictionary.Remove(position);
                if (GetTypeOfPosition(position) == CellType.Road)
                {
                    placementGrid[position.x, position.z] = CellType.Empty;
                    foreach (var roadNeighbour in GetNeighboursOfType(position, CellType.Road))
                    {
                        RoadRemoved?.Invoke(roadNeighbour);
                    }
                    HasFullPathProperty = HasFullPath(
                        new Vector3Int(0, 0, 0),
                        new Vector3Int(Width - 1, 0, Height - 1)
                    );
                }
                else
                {
                    placementGrid[position.x, position.z] = CellType.Empty;

                }
            }
        }
    }

    public bool CheckIfPositionIsUnremovable(Vector3Int position)
    {
        return  (position.x == 0 && position.z == 0) ||
                (position.x == Width - 1 && position.z == Width - 1) || 
                placementGrid[position.x, position.z] == CellType.Hill;
    }

    public void PlaceStructure(Vector3Int position, GameObject structurePrefab, CellType type)
    {
        placementGrid[position.x, position.z] = type;
        StructureModel structure = CreateANewStructureModel(position, structurePrefab, type);
        DestroyNatureAt(position);
        structureDictionary.Add(position, structure);
        if (type == CellType.Road)
        {
            HasFullPathProperty = HasFullPath(new Vector3Int(0, 0, 0), new Vector3Int(Width - 1, 0, Height - 1));
        }
    }

    private StructureModel CreateANewStructureModel(Vector3Int position, GameObject structurePrefab, CellType type)
    {
        GameObject structure = new GameObject(type.ToString());
        structure.transform.SetParent(transform);
        structure.transform.localPosition = position;
        var structureModel = structure.AddComponent<StructureModel>();
        structureModel.CreateModel(structurePrefab);
        return structureModel;
    }

    public void ModifyStructureModel(Vector3Int position, GameObject newModel, Quaternion rotation)
    {
        if (temporaryRoadobjects.ContainsKey(position))
            temporaryRoadobjects[position].SwapModel(newModel, rotation);
        else if (structureDictionary.ContainsKey(position))
            structureDictionary[position].SwapModel(newModel, rotation);
    }

    public void DestroyNatureAt(Vector3Int position)
    {
        /*RaycastHit[] hits = Physics.BoxCastAll(position + new Vector3(0, 0.5f, 0), new Vector3(0.5f, 0.5f, 0.5f), transform.up, Quaternion.identity, 1f, 1 << LayerMask.NameToLayer("Nature"));
        foreach (var item in hits)
        {
            Destroy(item.collider.gameObject);
        }*/
        if (structureDictionary.ContainsKey(position))
        {
            Destroy(structureDictionary[position].gameObject);
            structureDictionary.Remove(position);
        }
    }

    public AdjacentCellTypes GetNeighbourTypes(Vector3Int position)
    {
        return placementGrid.GetAllAdjacentCellTypes(position.x, position.z);
    }

    public List<Vector3Int> GetNeighboursOfType(Vector3Int position, CellType type)
    {
        var neighbourVertices = placementGrid.GetAdjacentCellsOfType(position.x, position.z, type);
        List<Vector3Int> neighbours = new List<Vector3Int>();
        foreach (var point in neighbourVertices)
        {
            neighbours.Add(new Vector3Int(point.X, 0, point.Y));
        }
        return neighbours;
    }

    public List<Vector3Int> GetPathBetween(Vector3Int startPosition, Vector3Int endPosition, bool isAgent = false)
    {
        var resultPath = GridSearch.AStarSearch(placementGrid, new Point(startPosition.x, startPosition.z), new Point(endPosition.x, endPosition.z));
        List<Vector3Int> path = new List<Vector3Int>();
        foreach (Point point in resultPath)
        {
            path.Add(new Vector3Int(point.X, 0, point.Y));
        }
        return path;
    }

    public Vector3Int RoundPosition(Vector3 position) => new Vector3Int((int)position.x, (int)position.y, (int)position.z);

    public bool HasFullPath(Vector3Int start, Vector3Int end)
    {
        return TryFindRoadPath(start, end, out _);
    }


    public List<Vector3Int> PickShortestPath(Vector3Int start, Vector3Int end)
    {
        return TryFindRoadPath(start, end, out var path) ? path : null;
    }


    private bool TryFindRoadPath(Vector3Int start, Vector3Int end, out List<Vector3Int> path)
    {
        path = null;

        Dictionary<Vector3Int, Vector3Int> cameFrom = new Dictionary<Vector3Int, Vector3Int>();
        Queue<Vector3Int> queue = new Queue<Vector3Int>();
        HashSet<Vector3Int> visited = new HashSet<Vector3Int>();

        queue.Enqueue(start);
        visited.Add(start);
        cameFrom[start] = start;

        while (queue.Count > 0)
        {
            Vector3Int current = queue.Dequeue();

            if (current == end)
            {
                path = new List<Vector3Int>();
                while (current != start)
                {
                    path.Add(current);
                    current = cameFrom[current];
                }
                path.Add(start);
                path.Reverse();
                return true;
            }

            List<Point> neighbors = placementGrid.GetAdjacentCellsOfType(current.x, current.z, CellType.Road);
            Shuffle(neighbors); // véletlenszerű sorrend

            foreach (Point p in neighbors)
            {
                Vector3Int neighbor = new Vector3Int(p.X, 0, p.Y);

                if (!visited.Contains(neighbor))
                {
                    visited.Add(neighbor);
                    queue.Enqueue(neighbor);
                    cameFrom[neighbor] = current;
                }
            }
        }

        return false;
    }


    private void Shuffle<T>(List<T> list)
    {
        for (int i = 0; i < list.Count; i++)
        {
            int j = UnityEngine.Random.Range(i, list.Count);
            T temp = list[i];
            list[i] = list[j];
            list[j] = temp;
        }
    }

    public List<Vector3Int> PickRandomRoadPath(Vector3Int start, Vector3Int end)
    {
        List<List<Vector3Int>> allPaths = new List<List<Vector3Int>>();
        FindAllRoadPaths(start, end, new HashSet<Vector3Int>(), new List<Vector3Int>(), allPaths);

        if (allPaths.Count == 0)
            return null;

        int index = UnityEngine.Random.Range(0, allPaths.Count);
        return allPaths[index];
    }

    private void FindAllRoadPaths(Vector3Int current, Vector3Int end, HashSet<Vector3Int> visited, List<Vector3Int> path, List<List<Vector3Int>> allPaths)
    {
        visited.Add(current);
        path.Add(current);

        if (current == end)
        {
            allPaths.Add(new List<Vector3Int>(path));
        }
        else
        {
            List<Point> neighbors = placementGrid.GetAdjacentCellsOfType(current.x, current.z, CellType.Road);
            Shuffle(neighbors); // véletlen sorrend

            foreach (Point p in neighbors)
            {
                Vector3Int neighbor = new Vector3Int(p.X, 0, p.Y);
                if (!visited.Contains(neighbor))
                {
                    FindAllRoadPaths(neighbor, end, visited, path, allPaths);
                }
            }
        }

        visited.Remove(current);
        path.RemoveAt(path.Count - 1);
    }

    public void RemoveAllTemporaryStructures()
    {
        foreach (var structure in temporaryRoadobjects.Values)
        {
            var position = Vector3Int.RoundToInt(structure.transform.position);
            placementGrid[position.x, position.z] = CellType.Empty;
            Destroy(structure.gameObject);
        }
        temporaryRoadobjects.Clear();
    }

    public void AddTemporaryStructuresToStructureDictionary()
    {
        foreach (var structure in temporaryRoadobjects)
        {
            DestroyNatureAt(structure.Key);
            structureDictionary.Add(structure.Key, structure.Value);
        }
        temporaryRoadobjects.Clear();
    }

    private void RemoveAnyStructure(Vector3Int position)
    {
        if (!CheckIfPositionInBound(position))
            return;
        if (structureDictionary.ContainsKey(position))
        {
            if (GetTypeOfPosition(position) != CellType.Empty)
            {
                Destroy(structureDictionary[position].gameObject);
                structureDictionary.Remove(position);
                if (GetTypeOfPosition(position) == CellType.Road)
                {
                    placementGrid[position.x, position.z] = CellType.Empty;
                    foreach (var roadNeighbour in GetNeighboursOfType(position, CellType.Road))
                    {
                        RoadRemoved?.Invoke(roadNeighbour);
                    }
                    HasFullPathProperty = HasFullPath(
                        new Vector3Int(0, 0, 0),
                        new Vector3Int(Width - 1, 0, Height - 1)
                    );
                }
                else
                {
                    placementGrid[position.x, position.z] = CellType.Empty;

                }
            }
        }
    }

    public PlacementManagerData SaveData()
    {
        return new PlacementManagerData(placementGrid, structureDictionary, HasFullPathProperty);
    }

    // StructureDictionary has to be loaded from GameManager using Road, Nature and Water managersS
    public void LoadData(PlacementManagerData data, PlacementManager placementManager = null)
    {
        ResetData();
        placementGrid = data.PlacementGrid;
        Width = placementGrid.Width;
        Height = placementGrid.Height;
        HasFullPathProperty = data.HasFullPath;
    }

    private void ResetData()
    {
        for (int i = 0; i < Width; ++i)
        {
            for (int j = 0; j < Height; ++j)
            {
                RemoveAnyStructure(new Vector3Int(i, 0, j));
            }
        }
        PlacedObjects = new PlacedObjects();
    }
}
