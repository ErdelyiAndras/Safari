using UnityEngine;

[System.Serializable]
public abstract class SearchInRangeData
{
    [SerializeField]
    private float visionRange;
    [SerializeField]
    private PlacementManagerData placementManager;

    public float VisionRange
    {
        get
        {
            return visionRange;
        }
    }

    public PlacementManager PlacementManager
    {
        get
        {
            return new PlacementManager();
        }
    }

    public SearchInRangeData(float visionRange, PlacementManager placementManager)
    {
        this.visionRange = visionRange;
        this.placementManager = new PlacementManagerData(6);
    }
}
