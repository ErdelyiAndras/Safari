using UnityEngine;

public abstract class Entity
{
    protected Vector3 spawnPosition; 
    protected GameObject entityInstance;
    protected PlacementManager placementManager;
    protected int visionRange;
    protected float baseMoveSpeed, baseRotationSpeed;
    protected float MoveSpeed { get { return baseMoveSpeed * SpeedMultiplier; } }
    protected float RotationSpeed { get { return baseRotationSpeed * SpeedMultiplier; } }
    public static float SpeedMultiplier { get; set; } = 1.0f;
    public Vector3 Position 
    {
        get { return entityInstance.transform.position; }
        set { entityInstance.transform.position = value; }
    }
    public abstract void CheckState();
    protected abstract void Move();
    protected void SpawnEntity(GameObject prefab, Transform parent = null) => entityInstance = Object.Instantiate(prefab, spawnPosition, Quaternion.identity, parent);
}
