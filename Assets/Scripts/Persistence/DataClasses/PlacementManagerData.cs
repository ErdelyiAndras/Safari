using System.Collections.Generic;
using UnityEngine;

public enum NatureType
{
    Bush1,
    Tree2,
    Tree4
}

[System.Serializable]
public class PlacementManagerData
{
    [SerializeField]
    private GridData placementGrid;
    [SerializeField]
    private List<Vector3Int> structureDictionaryNatureKeys;
    [SerializeField]
    private List<NatureType> structureDictionaryNatureValues;
    [SerializeField]
    private bool hasFullPath;

    public Grid PlacementGrid
    {
        get
        {
            return new Grid(placementGrid);
        }
    }

    public Dictionary<Vector3Int, NatureType> StructureDictionaryNature
    {
        get
        {
            Dictionary<Vector3Int, NatureType> structureDictionary = new Dictionary<Vector3Int, NatureType>();
            for (int i = 0; i < structureDictionaryNatureKeys.Count; i++)
            {
                structureDictionary.Add(structureDictionaryNatureKeys[i], structureDictionaryNatureValues[i]);
            }
            return structureDictionary;
        }
    }

    public bool HasFullPath
    {
        get { return hasFullPath; }
    }

    public PlacementManagerData(Grid placementGrid, Dictionary<Vector3Int, StructureModel> structureDictionary, bool hasFullPath)
    {
        this.placementGrid = placementGrid.SaveData();
        structureDictionaryNatureKeys = new List<Vector3Int>();
        structureDictionaryNatureValues = new List<NatureType>();
        foreach (var item in structureDictionary)
        {
            if (placementGrid[item.Key.x, item.Key.z] == CellType.Nature)
            {
                structureDictionaryNatureKeys.Add(item.Key);
                switch (item.Value.gameObject.transform.GetChild(0).name)
                {
                    case "Bush1(Clone)":
                        structureDictionaryNatureValues.Add(NatureType.Bush1);
                        break;
                    case "Tree2(Clone)":
                        structureDictionaryNatureValues.Add(NatureType.Tree2);
                        break;
                    case "Tree4(Clone)":
                        structureDictionaryNatureValues.Add(NatureType.Tree4);
                        break;
                    default:
                        throw new System.Exception("Unknown structure type: " + item.Value.gameObject.name);
                }
            }
        }
        this.hasFullPath = hasFullPath;
    }
}
