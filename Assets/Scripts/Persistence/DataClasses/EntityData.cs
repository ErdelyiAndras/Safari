using System;
using UnityEngine;

[System.Serializable]
public abstract class EntityData
{
    [SerializeField]
    private Guid id;
    [SerializeField]
    private Vector3 spawnPosition;
    [SerializeField]
    private PlacementManagerData placementManager;
    [SerializeField]
    private SearchInRangeData discoverEnvironment;
    [SerializeField]
    private Vector3 position;
    [SerializeField]
    private Quaternion rotation;

    public Guid Id
    {
        get { return id; }
    }

    public Vector3 SpawnPosition
    {
        get { return spawnPosition; }
    }

    public PlacementManager PlacementManager
    {
        get { return new PlacementManager(); }
    }

    public SearchInRange DiscoverEnvironment
    {
        get
        {
            return new HerbivoreSearchInRange(1.0f, PlacementManager, 2.0f);
        }
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
        Guid id, Vector3 spawnPosition, PlacementManager placementManager, SearchInRange discoverEnvironment, Vector3 position, Quaternion rotation
    )
    {
        this.id = id;
        this.spawnPosition = spawnPosition;
        this.placementManager = new PlacementManagerData(1);
        this.discoverEnvironment = new SearchInRangeData(2);
        this.position = position;
        this.rotation = rotation;
    }


}