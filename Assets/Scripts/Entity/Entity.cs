using UnityEngine;
using System;

public abstract class Entity : IPositionable, ISaveable<EntityData>
{
    public Guid Id { get; protected set; }
    protected Vector3 spawnPosition;
    public GameObject ObjectInstance { get; set; }
    protected PlacementManager placementManager;
    protected SearchInRange discoverEnvironment;
    protected float baseMoveSpeed, baseRotationSpeed;
    protected float MoveSpeed { get { return baseMoveSpeed * SpeedMultiplier; } }
    protected float RotationSpeed { get { return baseRotationSpeed * SpeedMultiplier; } }
    public static float SpeedMultiplier { get; set; }
    public Vector3 Position 
    {
        get { return ObjectInstance.transform.position; }
        set { ObjectInstance.transform.position = value; }
    }
    public abstract void CheckState();
    protected abstract void Move();
    protected void SpawnEntity(GameObject prefab, Transform parent = null) => ObjectInstance = UnityEngine.Object.Instantiate(prefab, spawnPosition, Quaternion.identity, parent);
    
    public Entity(PlacementManager _placementManager)
    {
        Id = Guid.NewGuid();
        placementManager = _placementManager;
    }

    public Entity(EntityData data, PlacementManager placementManager, GameObject prefab, GameObject parent)
    {
        SpawnEntity(prefab, parent.transform);
        LoadData(data, placementManager);
    }

    public void DeleteGameObject()
    {
        if (ObjectInstance != null)
        {
            UnityEngine.Object.Destroy(ObjectInstance);
            ObjectInstance = null;
        }
    }

    public abstract EntityData SaveData();

    public virtual void LoadData(EntityData data, PlacementManager placementManager)
    {
        this.placementManager = placementManager;
        Id = data.Id;
        spawnPosition = data.SpawnPosition;
        Position = data.Position;
        ObjectInstance.transform.rotation = data.Rotation;
        baseMoveSpeed = data.BaseMoveSpeed;
        baseRotationSpeed = data.BaseRotationSpeed;
    }
}
