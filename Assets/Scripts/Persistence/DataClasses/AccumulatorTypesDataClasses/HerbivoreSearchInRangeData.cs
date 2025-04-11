using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class HerbivoreSearchInRangeData : AnimalSearchInRangeData
{
    [SerializeField]
    private List<Vector3Int> discoveredFood;
    [SerializeField]
    private List<Vector3Int> foodInRange;

    public List<Vector3Int> DiscoveredFood
    {
        get
        {
            return discoveredFood;
        }
    }

    public List<Vector3Int> FoodInRange
    {
        get
        {
            return foodInRange;
        }
    }

    public HerbivoreSearchInRangeData(
        float visionRange, PlacementManager placementManager, List<Vector3Int> discoveredDrink, List<Vector3Int> drinkInRange, float viewExtenderScale,
        List<Vector3Int> discoveredFood, List<Vector3Int> foodInRange
    ) : base(visionRange, placementManager, discoveredDrink, drinkInRange, viewExtenderScale)
    {
        this.discoveredFood = discoveredFood;
        this.foodInRange = foodInRange;
    }
}