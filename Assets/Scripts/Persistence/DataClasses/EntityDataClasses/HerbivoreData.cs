using System;
using UnityEngine;

[System.Serializable]
public class HerbivoreData : AnimalData
{
    public override SearchInRange DiscoverEnvironment
    {
        get
        {
            return new HerbivoreSearchInRange((HerbivoreSearchInRangeData)discoverEnvironment);
        }
    }

    public HerbivoreData(
        Guid id, Vector3 spawnPosition, PlacementManager placementManager, HerbivoreSearchInRange discoverEnvironment, Vector3 position, Quaternion rotation,
        Animal.State state, AnimalInternalState internalState, Vector3 targetPosition, Herd herd, bool callOnceFlag, float elapsedTime
    ) : base(id, spawnPosition, placementManager, discoverEnvironment, position, rotation, state, internalState, targetPosition, herd, callOnceFlag, elapsedTime)
    {

    }
}