using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

public class WaterManager : BuildManagerBase
{
    private GameObject waterPrefab;

    private void Start()
    {
        waterPrefab = placementManager.prefabManager.Water;
    }

    public override void PlaceObject(Vector3Int position)
    {
        if (!CheckPositionBeforePlacement(position))
            return;
        temporaryPlacementPositions.Add(position);
    }
    public override void FinalizeObject(bool result, GameObject type = null)
    {
        if (result)
        {
            foreach (var item in temporaryPlacementPositions)
            {
                placementManager.PlaceStructure(item, waterPrefab, CellType.Water);
            }
        }
        temporaryPlacementPositions.Clear();
    }

    protected override bool CheckPositionBeforePlacement(Vector3Int position)
    {
        return placementManager.CheckIfPositionInBound(position) && placementManager.CheckIfPositionIsFreeFor(position, CellType.Water);
    }
}

