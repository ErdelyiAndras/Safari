using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public abstract class AnimalSearchInRangeData : SearchInRangeData
{
    [SerializeField]
    private List<Vector3Int> discoveredDrink;
    [SerializeField]
    private List<Vector3Int> drinkInRange;
    [SerializeField]
    private float viewExtenderScale;

    public List<Vector3Int> DiscoveredDrink
    {
        get
        {
            return discoveredDrink;
        }
    }

    public List<Vector3Int> DrinkInRange
    {
        get
        {
            return drinkInRange;
        }
    }

    public float ViewExtenderScale
    {
        get
        {
            return viewExtenderScale;
        }
    }

    public AnimalSearchInRangeData(
        float visionRange, List<Vector3Int> discoveredDrink, List<Vector3Int> drinkInRange, float viewExtenderScale
    ) : base(visionRange)
    {
        this.discoveredDrink = discoveredDrink;
        this.drinkInRange = drinkInRange;
        this.viewExtenderScale = viewExtenderScale;
    }
}
