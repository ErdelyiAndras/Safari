using System;
using UnityEngine;

[System.Serializable]
public class HerbivoreData : AnimalData
{
    [SerializeField]
    private HerbivoreSearchInRangeData discoverEnvironment;

    public HerbivoreSearchInRange DiscoverEnvironment(PlacementManager placementManager)
    {
        return new HerbivoreSearchInRange(discoverEnvironment, placementManager);
    }

    public HerbivoreData(
        Guid id, Vector3 spawnPosition, Vector3 position, Quaternion rotation,
        HerbivoreSearchInRange discoverEnvironment, Animal.State state, AnimalInternalState internalState, Vector3 targetPosition, Herd herd, bool callOnceFlag, float elapsedTime
    ) : base(id, spawnPosition, position, rotation, state, internalState, targetPosition, herd, callOnceFlag, elapsedTime)
    {
        this.discoverEnvironment = (HerbivoreSearchInRangeData)discoverEnvironment.SaveData();
    }
}