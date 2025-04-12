using NUnit.Framework;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class AnimalManagerData
{
    [SerializeField]
    private List<HerdData> herds;

    public List<Herd> Herds(PlacementManager placementManager)
    {
        List<Herd> herdList = new List<Herd>();
        foreach (HerdData herdData in herds)
        {
            switch (herdData.AnimalTypesOfHerd)
            {
                case AnimalType.Carnivore1:
                    herdList.Add(new CarnivoreHerd(herdData, placementManager));
                    break;
                case AnimalType.Carnivore2:
                    herdList.Add(new CarnivoreHerd(herdData, placementManager));
                    break;
                case AnimalType.Herbivore1:
                    herdList.Add(new HerbivoreHerd(herdData, placementManager));
                    break;
                case AnimalType.Herbivore2:
                    herdList.Add(new HerbivoreHerd(herdData, placementManager));
                    break;
            }
        }
        return herdList;
    }

    public AnimalManagerData(List<Herd> herds)
    {
        this.herds = new List<HerdData>();
        foreach (Herd herd in herds)
        {
            this.herds.Add(herd.SaveData());
        }
    }
}