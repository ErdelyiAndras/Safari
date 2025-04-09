using System;
using System.Collections.Generic;
using UnityEngine;

public enum HerdType
{
    Herbivore1Herd,
    Herbivore2Herd,
    Carnivore1Herd,
    Carnivore2Herd
}
public class Herd
{
    public readonly HerdType herdType;
    private List<Animal> animals;
    public int Count {  get { return animals.Count; } }
    private Vector2Int centroid;
    public Vector3Int Spawnpoint { get { return animals.Count == 0 ? GetRandomPosition() : new Vector3Int(centroid.x, 0, centroid.y); } }
    public int DistributionRadius { get; protected set;}
    private PlacementManager placementManager;
    public GameObject gameObject = new GameObject();
    

    public Herd(PlacementManager placementManager, AnimalManager parent, HerdType type)
    {
        animals = new List<Animal>();
        this.placementManager = placementManager;
        herdType = type;
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
        foreach(Animal animal in animals)
        {
            animal.CheckState();
        }
    }
    public void AgeAnimals()
    {
        for (int i = animals.Count - 1; i >= 0; i--)
        {
            animals[i].MatureAnimal();
        }
    }

    private Vector3Int GetRandomPosition()
    {
        int randomX = 0, randomZ = 0;
        do{
            randomX = UnityEngine.Random.Range(0, placementManager.width);
            randomZ = UnityEngine.Random.Range(0, placementManager.height);
        }
        while (!placementManager.IsPositionWalkable(new Vector3Int(randomX, 0, randomZ)));

        return new Vector3Int(randomX, 0, randomZ);
    }

}

