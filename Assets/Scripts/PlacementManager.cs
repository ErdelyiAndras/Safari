using System;
using System.Collections;
using System.Collections.Generic;
using JetBrains.Annotations;
using Unity.Collections;
using UnityEngine;

public class PlacementManager : MonoBehaviour
{
    public int width, height;
    public Grid placementGrid { get; private set; }
    internal Action<Vector3Int> RoadRemoved;

    private Dictionary<Vector3Int, StructureModel> temporaryRoadobjects = new Dictionary<Vector3Int, StructureModel>();
    private Dictionary<Vector3Int, StructureModel> structureDictionary = new Dictionary<Vector3Int, StructureModel>();

    private void Awake()
    {
        placementGrid = new Grid(width, height);
    }

    internal bool CheckIfPositionInBound(Vector3Int position) => position.x >= 0 && position.x < width && position.z >= 0 && position.z < height;

    internal bool IsPositionWalkable(Vector3Int position) => placementGrid[position.x, position.z] != CellType.Water;

    internal bool CheckIfPositionIsFreeFor(Vector3Int position, CellType type) 
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

    internal CellType GetTypeOfPosition(Vector3Int position) => placementGrid[position.x, position.z];

    internal bool CheckIfPositionIsOfType(Vector3Int position, CellType type) => placementGrid[position.x, position.z] == type;

    internal void PlaceTemporaryStructure(Vector3Int position, GameObject structurePrefab, CellType type)
    {
        placementGrid[position.x, position.z] = type;
        StructureModel structure = CreateANewStructureModel(position, structurePrefab, type);
        temporaryRoadobjects.Add(position, structure);
    }

    internal void RemoveStructure(Vector3Int position)
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
                }
                else
                {
                    placementGrid[position.x, position.z] = CellType.Empty;

                }
            }
        }
    }

    internal bool CheckIfPositionIsUnremovable(Vector3Int position) => (position.x == 0 && position.z == 0) || (position.x == width - 1 && position.z == width - 1);

    internal void PlaceStructure(Vector3Int position, GameObject structurePrefab, CellType type)
    {
        placementGrid[position.x, position.z] = type;
        StructureModel structure = CreateANewStructureModel(position, structurePrefab, type);
        DestroyNatureAt(position);
        structureDictionary.Add(position, structure);
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

    internal void ModifyStructureModel(Vector3Int position, GameObject newModel, Quaternion rotation)
    {
        if (temporaryRoadobjects.ContainsKey(position))
            temporaryRoadobjects[position].SwapModel(newModel, rotation);
        else if (structureDictionary.ContainsKey(position))
            structureDictionary[position].SwapModel(newModel, rotation);
    }

    internal void DestroyNatureAt(Vector3Int position)
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

    //[right, up, left, down]
    internal CellType[] GetNeighbourTypes(Vector3Int position)
    {
        Debug.Log($"X: {position.x}, Z: {position.z}");
        CellType[] n = placementGrid.GetAllAdjacentCellTypes(position.x, position.z);
        Debug.Log(n[0]);
        Debug.Log(n[1]);
        Debug.Log(n[2]);
        Debug.Log(n[3]);
        return n;
    }

    // TODO: refactor CellType[] to struct
    internal CellType[] GetExtendedNeighbourTypes(Vector3Int position)
    {
        return placementGrid.GetExtendedAdjacentCellTypes(position.x, position.z);
    }

    internal List<Vector3Int> GetNeighboursOfType(Vector3Int position, CellType type)
    {
        var neighbourVertices = placementGrid.GetAdjacentCellsOfType(position.x, position.z, type);
        List<Vector3Int> neighbours = new List<Vector3Int>();
        foreach (var point in neighbourVertices)
        {
            neighbours.Add(new Vector3Int(point.X, 0, point.Y));
        }
        return neighbours;
    }

    internal List<Vector3Int> GetPathBetween(Vector3Int startPosition, Vector3Int endPosition)
    {
        var resultPath = GridSearch.AStarSearch(placementGrid, new Point(startPosition.x, startPosition.z), new Point(endPosition.x, endPosition.z));
        List<Vector3Int> path = new List<Vector3Int>();
        foreach (Point point in resultPath)
        {
            path.Add(new Vector3Int(point.X, 0, point.Y));
        }
        return path;
    }

    internal void RemoveAllTemporaryStructures()
    {
        foreach (var structure in temporaryRoadobjects.Values)
        {
            var position = Vector3Int.RoundToInt(structure.transform.position);
            placementGrid[position.x, position.z] = CellType.Empty;
            Destroy(structure.gameObject);
        }
        temporaryRoadobjects.Clear();
    }

    internal void AddTemporaryStructuresToStructureDictionary()
    {
        foreach (var structure in temporaryRoadobjects)
        {
            DestroyNatureAt(structure.Key);
            structureDictionary.Add(structure.Key, structure.Value);
        }
        temporaryRoadobjects.Clear();
    }

}
