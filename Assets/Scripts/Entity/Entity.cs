using UnityEngine;
using System;

public abstract class Entity : IPositionable
{
    public Guid Id { get; protected set; }
    protected Vector3 spawnPosition; 
    protected GameObject entityInstance;
    protected PlacementManager placementManager;
    protected SearchInRange discoverEnvironment;
    protected float baseMoveSpeed, baseRotationSpeed;
    protected float MoveSpeed { get { return baseMoveSpeed * SpeedMultiplier; } }
    protected float RotationSpeed { get { return baseRotationSpeed * SpeedMultiplier; } }
    public static float SpeedMultiplier { get; set; }
    public Vector3 Position 
    {
        get { return entityInstance.transform.position; }
        set { entityInstance.transform.position = value; }
    }
    public abstract void CheckState();
    protected abstract void Move();
    protected void SpawnEntity(GameObject prefab, Transform parent = null) => entityInstance = UnityEngine.Object.Instantiate(prefab, spawnPosition, Quaternion.identity, parent);
}
