using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class HerdData
{
    [SerializeField]
    private AnimalType animalTypesOfHerd;
    [SerializeField]
    private List<AnimalData> animals;
    [SerializeField]
    private Vector2Int centroid;
    [SerializeField]
    private int distributionRadius;

    public AnimalType AnimalTypesOfHerd
    {
        get
        {
            return animalTypesOfHerd;
        }
    }

    public List<Animal> Animals(PlacementManager placementManager)
    {
        List<Animal> animalList = new List<Animal>();
        foreach (AnimalData animalData in animals)
        {
            Animal animal;
            switch (animalData.State.type)
            {
                case AnimalType.Herbivore1:
                    animal = new Herbivore1((HerbivoreData)animalData, placementManager);
                    break;
                case AnimalType.Herbivore2:
                    animal = new Herbivore2((HerbivoreData)animalData, placementManager);
                    break;
                case AnimalType.Carnivore1:
                    animal = new Carnivore1((CarnivoreData)animalData, placementManager);
                    break;
                case AnimalType.Carnivore2:
                    animal = new Carnivore2((CarnivoreData)animalData, placementManager);
                    break;
                default:
                    throw new System.Exception("Unknown animal type");
            }
            animalList.Add(animal);
        }
        return animalList;
    }

    public Vector2Int Centroid
    {
        get
        {
            return centroid;
        }
    }

    public int DistributionRadius
    {
        get
        {
            return distributionRadius;
        }
    }

    public HerdData(AnimalType animalTypesOfHerd, List<Animal> animals, Vector2Int centroid, int distributionRadius)
    {
        this.animalTypesOfHerd = animalTypesOfHerd;
        this.animals = new List<AnimalData>();
        foreach (Animal animal in animals)
        {
            AnimalData animalData = (AnimalData)animal.SaveData();
            this.animals.Add(animalData);
        }
        this.centroid = centroid;
        this.distributionRadius = distributionRadius;
    }
}