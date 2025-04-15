using System.Linq;
using UnityEngine;

public class RoadFixer : MonoBehaviour
{
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
        placementManager.ModifyStructureModel(temporaryPosition, placementManager.prefabManager.FourWay, Quaternion.identity);
    }

    private void Create3Way(PlacementManager placementManager, AdjacentCellTypes result, Vector3Int temporaryPosition)
    {
        if (result.Up == CellType.Road && result.Right == CellType.Road && result.Down == CellType.Road)
        {
            placementManager.ModifyStructureModel(temporaryPosition, placementManager.prefabManager.ThreeWay, Quaternion.identity);
        }
        else if (result.Right == CellType.Road && result.Down == CellType.Road && result.Left == CellType.Road)
        {
            placementManager.ModifyStructureModel(temporaryPosition, placementManager.prefabManager.ThreeWay, Quaternion.Euler(0, 90, 0));
        }
        else if (result.Down == CellType.Road && result.Left == CellType.Road && result.Up == CellType.Road)
        {
            placementManager.ModifyStructureModel(temporaryPosition, placementManager.prefabManager.ThreeWay, Quaternion.Euler(0, 180, 0));
        }
        else if (result.Left == CellType.Road && result.Up == CellType.Road && result.Right == CellType.Road)
        {
            placementManager.ModifyStructureModel(temporaryPosition, placementManager.prefabManager.ThreeWay, Quaternion.Euler(0, 270, 0));
        }

    }

    private void CreateCorner(PlacementManager placementManager, AdjacentCellTypes result, Vector3Int temporaryPosition)
    {
        if (result.Up == CellType.Road && result.Right == CellType.Road)
        {
            placementManager.ModifyStructureModel(temporaryPosition, placementManager.prefabManager.Corner, Quaternion.Euler(0, 90, 0));
        }
        else if (result.Right == CellType.Road && result.Down == CellType.Road)
        {
            placementManager.ModifyStructureModel(temporaryPosition, placementManager.prefabManager.Corner, Quaternion.Euler(0, 180, 0));
        }
        else if (result.Down == CellType.Road && result.Left == CellType.Road)
        {
            placementManager.ModifyStructureModel(temporaryPosition, placementManager.prefabManager.Corner, Quaternion.Euler(0, 270, 0));
        }
        else if (result.Left == CellType.Road && result.Up == CellType.Road)
        {
            placementManager.ModifyStructureModel(temporaryPosition, placementManager.prefabManager.Corner, Quaternion.identity);
        }
    }

    private bool CreateStraightRoad(PlacementManager placementManager, AdjacentCellTypes result, Vector3Int temporaryPosition)
    {
        if (result.Left == CellType.Road && result.Right == CellType.Road)
        {
            placementManager.ModifyStructureModel(temporaryPosition, placementManager.prefabManager.RoadStraigth, Quaternion.identity);
            return true;
        }
        else if (result.Up == CellType.Road && result.Down == CellType.Road)
        {
            placementManager.ModifyStructureModel(temporaryPosition, placementManager.prefabManager.RoadStraigth, Quaternion.Euler(0, 90, 0));
            return true;
        }
        return false;
    }

    private void CreateDeadEnd(PlacementManager placementManager, AdjacentCellTypes result, Vector3Int temporaryPosition)
    {
        if (result.Up == CellType.Road)
        {
            placementManager.ModifyStructureModel(temporaryPosition, placementManager.prefabManager.DeadEnd, Quaternion.Euler(0, 270, 0));
        }
        else if (result.Right == CellType.Road)
        {
            placementManager.ModifyStructureModel(temporaryPosition, placementManager.prefabManager.DeadEnd, Quaternion.identity);
        }
        else if (result.Down == CellType.Road)
        {
            placementManager.ModifyStructureModel(temporaryPosition, placementManager.prefabManager.DeadEnd, Quaternion.Euler(0, 90, 0));
        }
        else if (result.Left == CellType.Road)
        {
            placementManager.ModifyStructureModel(temporaryPosition, placementManager.prefabManager.DeadEnd, Quaternion.Euler(0, 180, 0));
        }
    }
}
