using System.Collections.Generic;
using System.Linq;
using System;
using UnityEngine;
using TMPro;
using UnityEngine.UIElements;

public class CarnivoreSearchInRange : AnimalSearchInRange
{
    private List<Herd> herds;
    private Guid preyGuid = Guid.Empty;
    private Herd closestHerd;
    public Herd ClosestHerd { get { return closestHerd; } }
    public Guid PreyGuid { get { return preyGuid; } }

    

    public CarnivoreSearchInRange(float _visionRange, PlacementManager _placementManager, float _viewExtenderScale, List<Herd> _herds) : base(_visionRange, _placementManager, _viewExtenderScale)
    {
        herds = _herds;
    }

    public CarnivoreSearchInRange(CarnivoreSearchInRangeData data) : base(data)
    {
        LoadData(data);
    }

    public override void SearchInViewDistance(Vector3 position)
    {
        Dictionary<CellType, BFSLists> data = new Dictionary<CellType, BFSLists>
        {
            { CellType.Water, new BFSLists { permanentList = discoveredDrink, temporaryList = drinkInRange } },
        };
        GeneralBFS(position, data);
    }

    public Vector3? GetClosestFood(Vector3 Position)
    {
        Vector3? targetPosition = null;
        List<Herd> herbivoreHerds = herds.Where(h => h.AnimalTypesOfHerd == AnimalType.Herbivore1 || h.AnimalTypesOfHerd == AnimalType.Herbivore2).ToList();
        if (herbivoreHerds.Count != 0)
        {
            closestHerd = herbivoreHerds.OrderBy(h => Vector3Int.Distance(h.Spawnpoint, Vector3Int.RoundToInt(Position))).FirstOrDefault();
            closestHerd.Animals.Sort((a, b) => Vector3.Distance(Position, a.Position).CompareTo(Vector3.Distance(Position, b.Position)));
            try
            {
                if (Vector3.Distance(closestHerd.Animals[0].Position, Position) <= GetViewDistance(Position))
                {
                    preyGuid = closestHerd.Animals[0].Id;
                    targetPosition = closestHerd.Animals[0].Position;
                }
            }
            catch { }
        }
        return targetPosition;
    }

    public override SearchInRangeData SaveData()
    {
        return new CarnivoreSearchInRangeData(visionRange, placementManager, discoveredDrink, drinkInRange, viewExtenderScale, herds, preyGuid, closestHerd);
    }

    public override void LoadData(SearchInRangeData data)
    {
        base.LoadData(data);
        herds = ((CarnivoreSearchInRangeData)data).Herds;
        preyGuid = ((CarnivoreSearchInRangeData)data).PreyGuid;
        closestHerd = ((CarnivoreSearchInRangeData)data).ClosestHerd;
    }
}