using System;
using System.Collections.Generic;
using UnityEngine;
public class Herd
{
    private List<Animal> animals;
    public int Count {  get { return animals.Count; } }
    private Vector2Int centroid;
    public Vector3Int Spawnpoint { get { return animals.Count == 0 ? GetRandomPosition() : new Vector3Int(centroid.x, 0, centroid.y); } }
    public int DistributionRadius { get; } = 5; // TO BE BALANCED
    private PlacementManager placementManager;

    public Herd(PlacementManager placementManager)
    {
        animals = new List<Animal>();
        this.placementManager = placementManager;
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
        Debug.Log("Adding constructed animal to herd");
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
        foreach (Animal animal in animals)
        {
            animal.AgeAnimal();
        }
    }

    public void SetSpeedMultiplier(float multiplier)
    {
        foreach (Animal animal in animals)
        {
            animal.SpeedMultiplier = multiplier;
        }
    }

    private Vector3Int GetRandomPosition()
    {
        int randomX = 0, randomZ = 0;
        do
        {
                randomX = UnityEngine.Random.Range(0, placementManager.width);
                randomZ = UnityEngine.Random.Range(0, placementManager.height);
        } while (!placementManager.IsPositionWalkable(new Vector3Int(randomX, 0, randomZ)));
        return new Vector3Int(randomX, 0, randomZ);
    }

}

