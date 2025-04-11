using UnityEngine;
using System.Collections.Generic;
using Unity.VisualScripting;

public class HerbivoreSearchInRange : AnimalSearchInRange
{
    private List<Vector3Int> discoveredFood = new List<Vector3Int>();
    private List<Vector3Int> foodInRange = new List<Vector3Int>();
    public HerbivoreSearchInRange(float _visionRange, PlacementManager _placementManager, float _viewExtenderScale) : base(_visionRange, _placementManager, _viewExtenderScale)
    {
    }

    public HerbivoreSearchInRange(HerbivoreSearchInRangeData data) : base(data)
    {
        LoadData(data);
    }

    public override void SearchInViewDistance(Vector3 position)
    {
        Dictionary<CellType, BFSLists> data = new Dictionary<CellType, BFSLists> 
        {
            { CellType.Water, new BFSLists { permanentList = discoveredDrink, temporaryList = drinkInRange } },
            { CellType.Nature, new BFSLists { permanentList = discoveredFood, temporaryList = foodInRange } }
        };
        GeneralBFS(position, data);
    }

    public Vector3? GetClosestFood(Vector3 Position, Vector3Int herdspawnpoint, int herdradius) => GetClosestTarget(Position, herdspawnpoint, herdradius, foodInRange, discoveredFood);

    public override SearchInRangeData SaveData()
    {
        return new HerbivoreSearchInRangeData(visionRange, placementManager, discoveredDrink, drinkInRange, viewExtenderScale, discoveredFood, foodInRange);
    }

    public override void LoadData(SearchInRangeData data)
    {
        base.LoadData(data);
        discoveredFood = ((HerbivoreSearchInRangeData)data).DiscoveredFood;
        foodInRange = ((HerbivoreSearchInRangeData)data).FoodInRange;
    }
}
