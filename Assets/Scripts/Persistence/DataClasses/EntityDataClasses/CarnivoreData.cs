using System;
using UnityEngine;

[System.Serializable]
public class CarnivoreData : AnimalData
{
    [SerializeField]
    private CarnivoreSearchInRangeData discoverEnvironment;

    public CarnivoreSearchInRange DiscoverEnvironment(PlacementManager placementManager)
    {
        return new CarnivoreSearchInRange(discoverEnvironment, placementManager);
    }

    public CarnivoreData(
        Guid id, Vector3 spawnPosition, Vector3 position, Quaternion rotation, float baseMoveSpeed, float baseRotationSpeed,
        CarnivoreSearchInRange discoverEnvironment, Animal.State state, AnimalInternalState internalState, Vector3 targetPosition, Herd herd, bool callOnceFlag, float elapsedTime
    ) : base(id, spawnPosition, position, rotation, baseMoveSpeed, baseRotationSpeed, state, internalState, targetPosition, herd, callOnceFlag, elapsedTime)
    {
        this.discoverEnvironment = (CarnivoreSearchInRangeData)discoverEnvironment.SaveData();
    }
}