using System.Collections.Generic;
using UnityEngine;

public class RoadManager : BuildManagerBase
{
    public List<Vector3Int> roadPositionsToRecheck = new List<Vector3Int>();

    private Vector3Int startPosition;
    private bool placementMode = false;

    public RoadFixer roadFixer;

    // public override int Count => temporaryPlacementPositions.Count * 30; // TODO: to be balanced
    private void Start()
    {
        roadFixer = GetComponent<RoadFixer>();
    }

    public override void PlaceObject(Vector3Int position)
    {
        if (!CheckPositionBeforePlacement(position))
            return;
        if (placementMode == false)
        {
            temporaryPlacementPositions.Clear();
            roadPositionsToRecheck.Clear();

            placementMode = true;
            startPosition = position;

            temporaryPlacementPositions.Add(position);
            placementManager.PlaceTemporaryStructure(position, roadFixer.deadEnd, CellType.Road);

        }
        else
        {
            placementManager.RemoveAllTemporaryStructures();
            temporaryPlacementPositions.Clear();

            foreach (var positionsToFix in roadPositionsToRecheck)
            {
                roadFixer.FixRoadAtPosition(placementManager, positionsToFix);
            }

            roadPositionsToRecheck.Clear();

            temporaryPlacementPositions = placementManager.GetPathBetween(startPosition, position);

            foreach (var temporaryPosition in temporaryPlacementPositions)
            {
                if (placementManager.CheckIfPositionIsFreeFor(temporaryPosition, CellType.Road) == false)
                {
                    roadPositionsToRecheck.Add(temporaryPosition);
                    continue;
                }
                placementManager.PlaceTemporaryStructure(temporaryPosition, roadFixer.deadEnd, CellType.Road);
            }
        }

        FixRoadPrefabs();

    }

    protected override bool CheckPositionBeforePlacement(Vector3Int position)
    {
        return placementManager.CheckIfPositionInBound(position) && placementManager.CheckIfPositionIsFreeFor(position, CellType.Road);
    }

    // Class specific functions
    private void FixRoadPrefabs()
    {
        foreach (var temporaryPosition in temporaryPlacementPositions)
        {
            roadFixer.FixRoadAtPosition(placementManager, temporaryPosition);
            var neighbours = placementManager.GetNeighboursOfType(temporaryPosition, CellType.Road);
            foreach (var roadposition in neighbours)
            {
                if (roadPositionsToRecheck.Contains(roadposition) == false)
                {
                    roadPositionsToRecheck.Add(roadposition);
                }
            }
        }
        foreach (var positionToFix in roadPositionsToRecheck)
        {
            roadFixer.FixRoadAtPosition(placementManager, positionToFix);
        }
    }

    public override void FinalizeObject(bool result, GameObject type = null)
    {
        if (result)
        {
            placementMode = false;
            placementManager.AddtemporaryStructuresToStructureDictionary();
            if (temporaryPlacementPositions.Count > 0)
            {
                //AudioPlayer.instance.PlayPlacementSound();
            }
            temporaryPlacementPositions.Clear();
            startPosition = Vector3Int.zero;
        }
        else
        {
            placementMode = false;
            placementManager.RemoveAllTemporaryStructures();
            temporaryPlacementPositions.Clear();
            startPosition = Vector3Int.zero;
        }
        
    }
}
