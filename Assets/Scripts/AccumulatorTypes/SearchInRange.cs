using UnityEngine;

public abstract class SearchInRange
{
    protected float visionRange;
    protected PlacementManager placementManager;
    public abstract void SearchInViewDistance(Vector3 position);

    public SearchInRange(float _visionRange, PlacementManager _placementManager)
    {
        visionRange = _visionRange;
        placementManager = _placementManager;
    }
}

