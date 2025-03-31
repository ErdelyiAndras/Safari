using System;
using System.Collections.Generic;
using UnityEngine;
public class Herd
{
    private List<Animal> animals;
    public int Count {  get { return animals.Count; } }
    private Vector2Int centroid;
    public Vector3Int Spawnpoint { get { return new Vector3Int(centroid.x, 0, centroid.y); } }
    public int DistributionRadius { get; }

    public Herd()
    {
        animals = new List<Animal>();
    }
    public void CalculateCentroid()
    {
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
    
    public void ManageAnimals()
    {
        foreach (Animal animal in animals)
        {
            animal.Advance();
        }
    }

    public void SetSpeedMultiplier(float multiplier)
    {
        foreach (Animal animal in animals)
        {
            animal.SpeedMultiplier = multiplier;
        }
    }

}

