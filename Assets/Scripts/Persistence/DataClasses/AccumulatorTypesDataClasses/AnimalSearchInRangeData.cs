using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public abstract class AnimalSearchInRangeData : SearchInRangeData
{
    [SerializeField]
    private List<Vector3> discoveredDrink;
    [SerializeField]
    private List<Vector3> drinkInRange;
    [SerializeField]
    private float viewExtenderScale;

    public List<Vector3> DiscoveredDrink
    {
        get
        {
            return discoveredDrink;
        }
    }

    public List<Vector3> DrinkInRange
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
        float visionRange, List<Vector3> discoveredDrink, List<Vector3> drinkInRange, float viewExtenderScale
    ) : base(visionRange)
    {
        this.discoveredDrink = discoveredDrink;
        this.drinkInRange = drinkInRange;
        this.viewExtenderScale = viewExtenderScale;
    }
}
