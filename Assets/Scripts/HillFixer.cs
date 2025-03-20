using UnityEngine;
using SVS;
using System.Linq;

public class HillFixerScript : MonoBehaviour
{
    public GameObject hillPrefab;

    private PlacementManager placementManager;

    private Transform core;
    private Transform xStraight1;
    private Transform xStraight2;
    private Transform yStraight1;
    private Transform yStraight2;
    private Transform corner1;
    private Transform corner2;
    private Transform corner3;
    private Transform corner4;

    private void Start()
    {
        core = hillPrefab.transform.Find("Core");
        xStraight1 = hillPrefab.transform.Find("XStraight1");
        xStraight2 = hillPrefab.transform.Find("XStraight2");
        yStraight1 = hillPrefab.transform.Find("YStraight1");
        yStraight2 = hillPrefab.transform.Find("YStraight2");
        corner1 = hillPrefab.transform.Find("Corner1");
        corner2 = hillPrefab.transform.Find("Corner2");
        corner3 = hillPrefab.transform.Find("Corner3");
        corner4 = hillPrefab.transform.Find("Corner4");

        placementManager = FindAnyObjectByType<PlacementManager>();
        if (placementManager == null)
        {
            Debug.LogError("PlacementManager not found!");
            return;
        }

        SetVisibility(core, true);
        SetVisibility(xStraight1, false);
        SetVisibility(xStraight2, false);
        SetVisibility(yStraight1, false);
        SetVisibility(yStraight2, false);
        SetVisibility(corner1, false);
        SetVisibility(corner2, false);
        SetVisibility(corner3, false);
        SetVisibility(corner4, false);

        CalculateVisibility();
    }

    private void CalculateVisibility()
    {
        Vector3Int position = Vector3Int.RoundToInt(hillPrefab.transform.position);
        AdjacentCellTypes adjacentCells = placementManager.GetNeighbourTypes(position);

        if (adjacentCells.ToArray(false).Where(x => x == CellType.Hill).Count() == 0)
        {
            return;
        }

        if (adjacentCells.Left == CellType.Hill)
        {
            SetVisibility(xStraight1, true);
        }
        if (adjacentCells.Right == CellType.Hill)
        {
            SetVisibility(xStraight2, true);
        }
        if (adjacentCells.Down == CellType.Hill)
        {
            SetVisibility(yStraight1, true);
        }
        if (adjacentCells.Up == CellType.Hill)
        {
            SetVisibility(yStraight2, true);
        }
        if (adjacentCells.RightUp == CellType.Hill && adjacentCells.Right == CellType.Hill && adjacentCells.Up == CellType.Hill)
        {
            SetVisibility(corner1, true);
        }
        if (adjacentCells.RightDown == CellType.Hill && adjacentCells.Right == CellType.Hill && adjacentCells.Down == CellType.Hill)
        {
            SetVisibility(corner2, true);
        }
        if (adjacentCells.LeftUp == CellType.Hill && adjacentCells.Left == CellType.Hill && adjacentCells.Up == CellType.Hill)
        {
            SetVisibility(corner3, true);
        }
        if (adjacentCells.LeftDown == CellType.Hill && adjacentCells.Left == CellType.Hill && adjacentCells.Down == CellType.Hill)
        {
            SetVisibility(corner4, true);
        }
    }

    private void SetVisibility(Transform part, bool isVisible)
    {
        part.gameObject.SetActive(isVisible);
    }
}
