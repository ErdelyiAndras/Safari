using System;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class CarnivoreSearchInRangeData : AnimalSearchInRangeData
{
    [SerializeField]
    private List<HerdData> herds;
    [SerializeField]
    private Guid preyGuid;
    [SerializeField]
    private HerdData closestHerd;

    public List<Herd> Herds
    {
        get
        {
            return new List<Herd>();
        }
    }

    public Guid PreyGuid
    {
        get
        {
            return preyGuid;
        }
    }

    public Herd ClosestHerd
    {
        get
        {
            return new CarnivoreHerd(PlacementManager, new AnimalManager(), AnimalType.Carnivore2);
        }
    }

    public CarnivoreSearchInRangeData(
        float visionRange, PlacementManager placementManager, List<Vector3Int> discoveredDrink, List<Vector3Int> drinkInRange, float viewExtenderScale,
        List<Herd> herds, Guid preyGuid, Herd closestHerd
    ) : base(visionRange, placementManager, discoveredDrink, drinkInRange, viewExtenderScale)
    {
        this.herds = new List<HerdData>();
        foreach (Herd herd in herds)
        {
            this.herds.Add(herd.SaveData());
        }
        this.preyGuid = preyGuid;
        this.closestHerd = closestHerd.SaveData();
    }
}