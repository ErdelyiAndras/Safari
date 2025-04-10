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
    Herd closestHerd;
    public Herd ClosestHerd { get { return closestHerd; } }
    public Guid PreyGuid { get { return preyGuid; } }

    public CarnivoreSearchInRange(float _visionRange, PlacementManager _placementManager, float _viewExtenderScale, List<Herd> _herds) : base(_visionRange, _placementManager, _viewExtenderScale)
    {
        herds = _herds;
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
        List<Herd> herbivoreHerds = herds.Where(h => h.herdType == HerdType.Herbivore1Herd || h.herdType == HerdType.Herbivore2Herd).ToList();
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
}