using UnityEngine;

[System.Serializable]
public abstract class SearchInRangeData
{
    [SerializeField]
    private float visionRange;

    public float VisionRange
    {
        get
        {
            return visionRange;
        }
    }

    public SearchInRangeData(float visionRange)
    {
        this.visionRange = visionRange;
    }
}
