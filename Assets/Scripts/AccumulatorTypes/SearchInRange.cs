using UnityEngine;

public abstract class SearchInRange : ISaveable<SearchInRangeData>
{
    protected float visionRange;
    protected PlacementManager placementManager;

    public abstract void SearchInViewDistance(Vector3 position);

    public SearchInRange(float _visionRange, PlacementManager _placementManager)
    {
        visionRange = _visionRange;
        placementManager = _placementManager;
    }

    public SearchInRange(SearchInRangeData data)
    {
        LoadData(data);
    }

    public abstract SearchInRangeData SaveData();
    public virtual void LoadData(SearchInRangeData data)
    {
        visionRange = data.VisionRange;
        placementManager = data.PlacementManager;
    }
}

