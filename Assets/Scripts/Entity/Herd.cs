using System;
using System.Collections.Generic;
using UnityEngine;

public abstract class Herd : IPositionable, ISaveable<HerdData>
{
    public Guid Id { get; }
    public AnimalType animalTypesOfHerd; // if mixed herds are allowed this can be a set
    public AnimalType AnimalTypesOfHerd => animalTypesOfHerd;
    private List<Animal> animals;
    private Vector2Int centroid;
    private PlacementManager placementManager;
    public int Count {  get { return animals.Count; } }
    public Vector3 Position { get { return animals.Count == 0 ? GetRandomPosition() : new Vector3(centroid.x, 0, centroid.y); } }
    public int DistributionRadius { get; protected set;}
    public GameObject gameObject = new GameObject();
    public List<Animal> Animals{ get { return animals; }}
    public Action<Herd> Reproduce;
    private int reproductionCoolDown;
    

    public Herd(PlacementManager placementManager, AnimalManager parent, AnimalType type)
    {
        Id = Guid.NewGuid();
        animals = new List<Animal>();
        this.placementManager = placementManager;
        animalTypesOfHerd = type;
        gameObject.transform.SetParent(parent.transform);
        reproductionCoolDown = 8;
    }

    public Herd(HerdData data, PlacementManager placementManager)
    {
        LoadData(data, placementManager);
        AnimalManager parent = UnityEngine.Object.FindFirstObjectByType<AnimalManager>();
        gameObject.transform.SetParent(parent.transform);
    }

    public void CalculateCentroid()
    {
        if (animals.Count == 0)
        {
            return;
        }
        Vector2Int sum = Vector2Int.zero;
        foreach (Animal animal in animals)
        {
            sum += new Vector2Int((int)animal.Position.x, (int)animal.Position.z);
        }
        centroid = sum / animals.Count;
    }

    public void AddAnimalToHerd(Animal animal)
    {
        animals.Add(animal);
    }
    public void RemoveAnimalFromHerd(Animal animal)
    {
        animals.Remove(animal);
    }

    public void CheckState()
    {
        for (int i = animals.Count - 1; i >= 0; i--)
        {
            animals[i].CheckState();
        }
    }
    public void AgeAnimals()
    {
        for (int i = animals.Count - 1; i >= 0; i--)
        {
            animals[i].MatureAnimal();
        }
        reproductionCoolDown--;
        if (reproductionCoolDown <= 0 && Count >=2) // TODO és felnőttek is legyenek az egyedek
        {
            Reproduce?.Invoke(this);
            reproductionCoolDown = 8;
        }
    }

    private Vector3 GetRandomPosition()
    {
        int randomX = 0, randomZ = 0;
        do{
            randomX = UnityEngine.Random.Range(0, placementManager.width);
            randomZ = UnityEngine.Random.Range(0, placementManager.height);
        }
        while (!placementManager.IsPositionWalkable(new Vector3Int(randomX, 0, randomZ)));

        return new Vector3(randomX, 0, randomZ);
    }

    public HerdData SaveData()
    {
        return new HerdData(animalTypesOfHerd, animals, centroid, DistributionRadius);
    }

    public void LoadData(HerdData data, PlacementManager placementManager)
    {
        animalTypesOfHerd = data.AnimalTypesOfHerd;
        animals = data.Animals(placementManager, animalManager, this);
        centroid = data.Centroid;
        this.placementManager = placementManager;
        DistributionRadius = data.DistributionRadius;
        foreach (Animal animal in animals)
        {
            animal.myHerd = this;
        }
    }
}

