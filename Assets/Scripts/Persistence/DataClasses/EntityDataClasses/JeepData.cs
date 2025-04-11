using System;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class JeepData : EntityData
{
    [SerializeField]
    private Jeep.State state;
    [SerializeField]
    private Vector3 endPosition;
    [SerializeField]
    private TouristGroupData touristGroup;
    [SerializeField]
    private List<Vector3Int> jeepPath;
    [SerializeField]
    private int currentPathIndex;
    [SerializeField]
    private bool hasFullPath;

    public override SearchInRange DiscoverEnvironment
    {
        get
        {
            return new JeepSearchInRange((JeepSearchInRangeData)discoverEnvironment);
        }
    }

    public Jeep.State State
    {
        get { return state; }
    }

    public Vector3 EndPosition
    {
        get { return endPosition; }
    }

    public TouristGroup TouristGroup
    {
        get { return new TouristGroup(touristGroup); }
    }

    public List<Vector3Int> JeepPath
    {
        get { return jeepPath; }
    }

    public int CurrentPathIndex
    {
        get { return currentPathIndex; }
    }

    public bool HasFullPath
    {
        get { return hasFullPath; }
    }

    public JeepData(
        Guid id, Vector3 spawnPosition, PlacementManager placementManager, JeepSearchInRange discoverEnvironment, Vector3 position, Quaternion rotation,
        Jeep.State state, Vector3 endPosition, TouristGroup touristGroup, List<Vector3Int> jeepPath, int currentPathIndex, bool hasFullPath
    ) : base(id, spawnPosition, placementManager, discoverEnvironment, position, rotation)
    {
        this.state = state;
        this.endPosition = endPosition;
        this.touristGroup = touristGroup.SaveData();
        this.jeepPath = jeepPath;
        this.currentPathIndex = currentPathIndex;
        this.hasFullPath = hasFullPath;
    }
}