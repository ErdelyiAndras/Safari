using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class AnimalManagerData
{
    [SerializeField]
    private List<CarnivoreHerdData> carnivoreHerds;
    [SerializeField]
    private List<HerbivoreHerdData> herbivoreHerds;

    public List<Herd> Herds(PlacementManager placementManager, AnimalManager parent)
    {
        List<Herd> herdList = new List<Herd>();
        foreach (CarnivoreHerdData carnivoreHerdData in carnivoreHerds)
        {
            switch (carnivoreHerdData.AnimalTypesOfHerd)
            {
                case AnimalType.Carnivore1:
                    herdList.Add(new CarnivoreHerd(carnivoreHerdData, placementManager, parent));
                    break;
                case AnimalType.Carnivore2:
                    herdList.Add(new CarnivoreHerd(carnivoreHerdData, placementManager, parent));
                    break;
                default:
                    throw new System.Exception($"Unknown animal type in carnivoreherds {carnivoreHerdData.AnimalTypesOfHerd}");
            }
        }
        foreach (HerbivoreHerdData herbivoreHerdData in herbivoreHerds)
        {
            switch (herbivoreHerdData.AnimalTypesOfHerd)
            {
                case AnimalType.Herbivore1:
                    herdList.Add(new HerbivoreHerd(herbivoreHerdData, placementManager, parent));
                    break;
                case AnimalType.Herbivore2:
                    herdList.Add(new HerbivoreHerd(herbivoreHerdData, placementManager, parent));
                    break;
                default:
                    throw new System.Exception("Unknown animal type in herbivoreherds");
            }
        }
        return herdList;
    }

    public AnimalManagerData(List<Herd> herds)
    {
        carnivoreHerds = new List<CarnivoreHerdData>();
        herbivoreHerds = new List<HerbivoreHerdData>();
        foreach (Herd herd in herds)
        {
            if (herd is CarnivoreHerd carnivoreHerd)
            {
                carnivoreHerds.Add((CarnivoreHerdData)carnivoreHerd.SaveData());
            }
            else if (herd is HerbivoreHerd herbivoreHerd)
            {
                herbivoreHerds.Add((HerbivoreHerdData)herbivoreHerd.SaveData());
            }
            else
            {
                throw new System.Exception("Unknown herd type");
            }
        }
    }
}