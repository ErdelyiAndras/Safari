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
    private PlacementManagerData placementManager;
    [SerializeField]
    private int distributionRadius;

    public AnimalType AnimalType
    {
        get
        {
            return animalTypesOfHerd;
        }
    }

    public List<Animal> Animals
    {
        get
        {
            return new List<Animal>();
        }
    }

    public Vector2Int Centroid
    {
        get
        {
            return centroid;
        }
    }

    public PlacementManager PlacementManager
    {
        get
        {
            return new PlacementManager();
        }
    }

    public int DistributionRadius
    {
        get
        {
            return distributionRadius;
        }
    }

    public HerdData(AnimalType animalTypesOfHerd, List<Animal> animals, Vector2Int centroid, PlacementManager placementManager, int distributionRadius)
    {
        this.animalTypesOfHerd = animalTypesOfHerd;
        this.animals = new List<AnimalData>();
        foreach (Animal animal in animals)
        {
            this.animals.Add((AnimalData)animal.SaveData());
        }
        this.centroid = centroid;
        this.placementManager = new PlacementManagerData(4);
        this.distributionRadius = distributionRadius;
    }
}