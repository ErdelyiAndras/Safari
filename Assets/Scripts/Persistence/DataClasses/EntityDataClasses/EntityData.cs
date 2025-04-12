using System;
using UnityEngine;

[System.Serializable]
public abstract class EntityData
{
    [SerializeField]
    private string id;
    [SerializeField]
    private Vector3 spawnPosition;
    [SerializeField]
    private Vector3 position;
    [SerializeField]
    private Quaternion rotation;

    public Guid Id
    {
        get { return Guid.Parse(id); }
    }

    public Vector3 SpawnPosition
    {
        get { return spawnPosition; }
    }

    public PlacementManager PlacementManager
    {
        get { return new PlacementManager(); }
    }

    public Vector3 Position
    {
        get { return position; }
    }

    public Quaternion Rotation
    {
        get { return rotation; }
    }

    public EntityData(
        Guid id, Vector3 spawnPosition, Vector3 position, Quaternion rotation
    )
    {
        this.id = id.ToString();
        this.spawnPosition = spawnPosition;
        this.position = position;
        this.rotation = rotation;
    }
}