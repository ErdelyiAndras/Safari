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
        Guid id, Vector3 spawnPosition, Vector3 position, Quaternion rotation, float baseMoveSpeed, float baseRotationSpeed,
        HerbivoreSearchInRange discoverEnvironment, Animal.State state, AnimalInternalState internalState, Vector3 targetPosition, Guid myHerd, bool callOnceFlag, bool targetCorrection, float elapsedTime
    ) : base(id, spawnPosition, position, rotation, baseMoveSpeed, baseRotationSpeed, state, internalState, targetPosition, myHerd, callOnceFlag, targetCorrection, elapsedTime)
    {
        this.discoverEnvironment = (HerbivoreSearchInRangeData)discoverEnvironment.SaveData();
    }
}