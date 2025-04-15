using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class HerbivoreSearchInRangeData : AnimalSearchInRangeData
{
    [SerializeField]
    private List<Vector3> discoveredFood;
    [SerializeField]
    private List<Vector3> foodInRange;

    public List<Vector3> DiscoveredFood
    {
        get
        {
            return discoveredFood;
        }
    }

    public List<Vector3> FoodInRange
    {
        get
        {
            return foodInRange;
        }
    }

    public HerbivoreSearchInRangeData(
        float visionRange, List<Vector3> discoveredDrink, List<Vector3> drinkInRange, float viewExtenderScale,
        List<Vector3> discoveredFood, List<Vector3> foodInRange
    ) : base(visionRange, discoveredDrink, drinkInRange, viewExtenderScale)
    {
        this.discoveredFood = discoveredFood;
        this.foodInRange = foodInRange;
    }
}