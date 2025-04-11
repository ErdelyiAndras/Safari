using System;
using UnityEngine;

[System.Serializable]
public class CarnivoreData : AnimalData
{
    public override SearchInRange DiscoverEnvironment
    {
        get
        {
            return new CarnivoreSearchInRange((CarnivoreSearchInRangeData)discoverEnvironment);
        }
    }

    public CarnivoreData(
        Guid id, Vector3 spawnPosition, PlacementManager placementManager, CarnivoreSearchInRange discoverEnvironment, Vector3 position, Quaternion rotation,
        Animal.State state, AnimalInternalState internalState, Vector3 targetPosition, Herd herd, bool callOnceFlag, float elapsedTime
    ) : base(id, spawnPosition, placementManager, discoverEnvironment, position, rotation, state, internalState, targetPosition, herd, callOnceFlag, elapsedTime)
    {

    }
}