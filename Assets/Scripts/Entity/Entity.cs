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
    public float SpeedMultiplier { get; set; } = 1.0f;
    public Vector3 Position 
    {
        get { return entityInstance.transform.position; }
        set { entityInstance.transform.position = value; }
    }
    public abstract void CheckState();
    // Need Input --> destructor deletes the object (ex: animal dies and disposes of itself), constructor instantiates the object

    protected abstract void Move();

    protected void SpawnEntity(GameObject prefab, Transform parent = null) => entityInstance = Object.Instantiate(prefab, spawnPosition, Quaternion.identity, parent);
    // jó lenne ha vízbe meg hegybe nem spawnolna animal

}
