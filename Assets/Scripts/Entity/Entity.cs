using UnityEngine;

public abstract class Entity
{
    protected Vector3 spawnPosition; 
    protected GameObject entityInstance;
    protected PlacementManager placementManager;
    protected int visionRange;
    protected float baseMoveSpeed;
    protected float MoveSpeed { get { return baseMoveSpeed * SpeedMultiplier; } }
    public float SpeedMultiplier { get; set; }
    public Vector3 Position 
    {
        get { return entityInstance.transform.position; }
    }
    public abstract void CheckState();
    // Need Input --> destructor deletes the object (ex: animal dies and disposes of itself), constructor instantiates the object
    public abstract void Move(Vector3 targetPosition);
    protected void SpawnEntity(GameObject prefab) => entityInstance = Object.Instantiate(prefab, spawnPosition, Quaternion.identity);
    // jó lenne ha vízbe meg hegybe nem spawnolna animal




}
