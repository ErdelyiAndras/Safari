using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UIElements;


public class NatureManager : BuildManagerBase
{
    // public override int Count => 50; //TODO: to be balanced
    public override void PlaceObject(Vector3Int position)
    {
        if (!CheckPositionBeforePlacement(position))
            return;
        temporaryPlacementPositions.Add(position);
    }
    public override void FinalizeObject(bool result, GameObject type)
    {
        if (result)
        {
            foreach (var item in temporaryPlacementPositions)
            {
                placementManager.PlaceStructure(item, type, CellType.Nature);
            }
        }
        temporaryPlacementPositions.Clear();
    }

    protected override bool CheckPositionBeforePlacement(Vector3Int position)
    {
        return placementManager.CheckIfPositionInBound(position) && placementManager.CheckIfPositionIsFreeFor(position, CellType.Nature);
    }


}

