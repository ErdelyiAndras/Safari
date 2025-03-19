using System.Linq;
using UnityEngine;

public class RoadFixer : MonoBehaviour
{
    public GameObject deadEnd, roadStraight, corner, threeWay, fourWay;

    public void FixRoadAtPosition(PlacementManager placementManager, Vector3Int temporaryPosition)
    {
        AdjacentCellTypes result = placementManager.GetNeighbourTypes(temporaryPosition);
        int roadCount = 0; 
        roadCount = result.ToArray(false).Where(x => x == CellType.Road).Count();
        
        if (roadCount == 0 || roadCount == 1)
        {
            CreateDeadEnd(placementManager, result, temporaryPosition);
        }
        else if (roadCount == 2)
        {
            if (CreateStraightRoad(placementManager, result, temporaryPosition))
                return;
            CreateCorner(placementManager, result, temporaryPosition);
        }
        else if (roadCount == 3)
        {
            Create3Way(placementManager, result, temporaryPosition);
        }
        else
        {
            Create4Way(placementManager, result, temporaryPosition);
        }
    }

    private void Create4Way(PlacementManager placementManager, AdjacentCellTypes result, Vector3Int temporaryPosition)
    {
        placementManager.ModifyStructureModel(temporaryPosition, fourWay, Quaternion.identity);
    }

    private void Create3Way(PlacementManager placementManager, AdjacentCellTypes result, Vector3Int temporaryPosition)
    {
        if (result.Up == CellType.Road && result.Right == CellType.Road && result.Down == CellType.Road)
        {
            placementManager.ModifyStructureModel(temporaryPosition, threeWay, Quaternion.identity);
        }
        else if (result.Right == CellType.Road && result.Down == CellType.Road && result.Left == CellType.Road)
        {
            placementManager.ModifyStructureModel(temporaryPosition, threeWay, Quaternion.Euler(0, 90, 0));
        }
        else if (result.Down == CellType.Road && result.Left == CellType.Road && result.Up == CellType.Road)
        {
            placementManager.ModifyStructureModel(temporaryPosition, threeWay, Quaternion.Euler(0, 180, 0));
        }
        else if (result.Left == CellType.Road && result.Up == CellType.Road && result.Right == CellType.Road)
        {
            placementManager.ModifyStructureModel(temporaryPosition, threeWay, Quaternion.Euler(0, 270, 0));
        }

    }

    private void CreateCorner(PlacementManager placementManager, AdjacentCellTypes result, Vector3Int temporaryPosition)
    {
        if (result.Up == CellType.Road && result.Right == CellType.Road)
        {
            placementManager.ModifyStructureModel(temporaryPosition, corner, Quaternion.Euler(0, 90, 0));
        }
        else if (result.Right == CellType.Road && result.Down == CellType.Road)
        {
            placementManager.ModifyStructureModel(temporaryPosition, corner, Quaternion.Euler(0, 180, 0));
        }
        else if (result.Down == CellType.Road && result.Left == CellType.Road)
        {
            placementManager.ModifyStructureModel(temporaryPosition, corner, Quaternion.Euler(0, 270, 0));
        }
        else if (result.Left == CellType.Road && result.Up == CellType.Road)
        {
            placementManager.ModifyStructureModel(temporaryPosition, corner, Quaternion.identity);
        }
    }

    private bool CreateStraightRoad(PlacementManager placementManager, AdjacentCellTypes result, Vector3Int temporaryPosition)
    {
        if (result.Left == CellType.Road && result.Right == CellType.Road)
        {
            placementManager.ModifyStructureModel(temporaryPosition, roadStraight, Quaternion.identity);
            return true;
        }
        else if (result.Up == CellType.Road && result.Down == CellType.Road)
        {
            placementManager.ModifyStructureModel(temporaryPosition, roadStraight, Quaternion.Euler(0, 90, 0));
            return true;
        }
        return false;
    }

    private void CreateDeadEnd(PlacementManager placementManager, AdjacentCellTypes result, Vector3Int temporaryPosition)
    {
        if (result.Up == CellType.Road)
        {
            placementManager.ModifyStructureModel(temporaryPosition, deadEnd, Quaternion.Euler(0, 270, 0));
        }
        else if (result.Right == CellType.Road)
        {
            placementManager.ModifyStructureModel(temporaryPosition, deadEnd, Quaternion.identity);
        }
        else if (result.Down == CellType.Road)
        {
            placementManager.ModifyStructureModel(temporaryPosition, deadEnd, Quaternion.Euler(0, 90, 0));
        }
        else if (result.Left == CellType.Road)
        {
            placementManager.ModifyStructureModel(temporaryPosition, deadEnd, Quaternion.Euler(0, 180, 0));
        }
    }
}
