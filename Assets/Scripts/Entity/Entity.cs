using UnityEngine;
using System;

public abstract class Entity : IPositionable, ISaveable<EntityData>
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
    
    public Entity(PlacementManager _placementManager, GameObject prefab)
    {
        Id = Guid.NewGuid();
        placementManager = _placementManager;
        SpawnEntity(prefab);
    }

    public Entity(EntityData data, PlacementManager placementManager, GameObject prefab, GameObject parent)
    {
        SpawnEntity(prefab, parent.transform);
        LoadData(data, placementManager);
    }

    public abstract EntityData SaveData();

    public virtual void LoadData(EntityData data, PlacementManager placementManager)
    {
        Id = data.Id;
        spawnPosition = data.SpawnPosition;
        this.placementManager = placementManager;
        //discoverEnvironment = data.DiscoverEnvironment;
        Position = data.Position;
        entityInstance.transform.rotation = data.Rotation;
        baseMoveSpeed = data.BaseMoveSpeed;
        baseRotationSpeed = data.BaseRotationSpeed;
    }
}
