using System;
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
        placementManager.RoadRemoved += FixRoadAtPosition;
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
            placementManager.PlaceTemporaryStructure(position, placementManager.prefabManager.DeadEnd, CellType.Road);

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
                placementManager.PlaceTemporaryStructure(temporaryPosition, placementManager.prefabManager.DeadEnd, CellType.Road);
            }
        }
        FixRoadPrefabs();
    }

    public void FixRoadAtPosition(Vector3Int position)
    {
        roadFixer.FixRoadAtPosition(placementManager, position);
    }

    protected override bool CheckPositionBeforePlacement(Vector3Int position)
    {
        return placementManager.CheckIfPositionInBound(position) && placementManager.CheckIfPositionIsFreeFor(position, CellType.Road);
    }

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
            placementManager.AddTemporaryStructuresToStructureDictionary();
            if (temporaryPlacementPositions.Count > 0)
            {
                //AudioPlayer.instance.PlayPlacementSound();
            }
            temporaryPlacementPositions.Clear();
            startPosition = Vector3Int.zero;

            placementManager.HasFullPathProperty = placementManager.HasFullPath(
                new Vector3Int(0, 0, 0),
                new Vector3Int(PlacementManager.width - 1, 0, PlacementManager.height - 1)
            );
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
