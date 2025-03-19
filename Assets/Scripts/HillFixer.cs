using UnityEngine;
using SVS;

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

        core.gameObject.SetActive(true);

        //GetRandomVersionOfHill();
        CalculateVisibility();
    }

    private void CalculateVisibility()
    {
        Vector3Int position = Vector3Int.RoundToInt(hillPrefab.transform.position);
        AdjacentCellTypes adjacentCells = placementManager.GetNeighbourTypes(position);

        if (adjacentCells.ToArray(false).Length == 0)
        {
            SetVisibility();
            return;
        }

        if (adjacentCells.Down == CellType.Hill)
        {
            SetVisibility(xStraight1: true);
        }
        if (adjacentCells.Up == CellType.Hill)
        {
            SetVisibility(xStraight2: true);
        }
        if (adjacentCells.Right == CellType.Hill)
        {
            SetVisibility(yStraight1: true);
        }
        if (adjacentCells.Left == CellType.Hill)
        {
            SetVisibility(yStraight2: true);
        }
        if (adjacentCells.LeftUp == CellType.Hill)
        {
            SetVisibility(corner1: true);
        }
        if (adjacentCells.RightUp == CellType.Hill)
        {
            SetVisibility(corner2: true);
        }
        if (adjacentCells.LeftDown == CellType.Hill)
        {
            SetVisibility(corner3: true);
        }
        if (adjacentCells.RightDown == CellType.Hill)
        {
            SetVisibility(corner4: true);
        }
    }

    // TODO: only here to showcase
    private void GetRandomVersionOfHill()
    {
        if (Random.Range(0, 100) < 50)
        {
            core.gameObject.SetActive(false);
        }
        if (Random.Range(0, 100) < 50)
        {
            xStraight1.gameObject.SetActive(false);
        }
        if (Random.Range(0, 100) < 50)
        {
            xStraight2.gameObject.SetActive(false);
        }
        if (Random.Range(0, 100) < 50)
        {
            yStraight1.gameObject.SetActive(false);
        }
        if (Random.Range(0, 100) < 50)
        {
            yStraight2.gameObject.SetActive(false);
        }
        if (Random.Range(0, 100) < 50)
        {
            corner1.gameObject.SetActive(false);
        }
        if (Random.Range(0, 100) < 50)
        {
            corner2.gameObject.SetActive(false);
        }
        if (Random.Range(0, 100) < 50)
        {
            corner3.gameObject.SetActive(false);
        }
        if (Random.Range(0, 100) < 50)
        {
            corner4.gameObject.SetActive(false);
        }
    }

    private void SetVisibility(
        bool xStraight1 = false, 
        bool xStraight2 = false, 
        bool yStraight1 = false, 
        bool yStraight2 = false, 
        bool corner1 = false, 
        bool corner2 = false, 
        bool corner3 = false, 
        bool corner4 = false)
    {
        this.xStraight1.gameObject.SetActive(xStraight1);
        this.xStraight2.gameObject.SetActive(xStraight2);
        this.yStraight1.gameObject.SetActive(yStraight1);
        this.yStraight2.gameObject.SetActive(yStraight2);
        this.corner1.gameObject.SetActive(corner1);
        this.corner2.gameObject.SetActive(corner2);
        this.corner3.gameObject.SetActive(corner3);
        this.corner4.gameObject.SetActive(corner4);
    }
}
