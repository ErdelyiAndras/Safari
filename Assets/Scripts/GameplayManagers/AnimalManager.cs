using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;


public class AnimalManager : MonoBehaviour
{
    public PlacementManager placementManager;
    public GameObject carnivore1Prefab, carnivore2Prefab, herbivore1Prefab, herbivore2Prefab;

    private List<Animal> spawnedAnimals = new List<Animal>();

    private void Update()
    {
        foreach(var animal in spawnedAnimals)
        {
            animal.CheckState();
            animal.MoveTowardsTarget();
        }
    }

    public void SetSpeedMultiplier(float multiplier)
    {
        foreach (var animal in spawnedAnimals)
        {
            animal.SpeedMultiplier = multiplier;
        }
    }
    public void BuyCarnivore1()
    {
        spawnedAnimals.Add(new Carnivore1(carnivore1Prefab, placementManager));
    }
    public void BuyCarnivore2()
    {
        spawnedAnimals.Add(new Carnivore2(carnivore2Prefab, placementManager));
    }
    public void BuyHerbivore1()
    {
        spawnedAnimals.Add(new Herbivore1(herbivore1Prefab, placementManager));
    }
    public void BuyHerbivore2()
    {
        spawnedAnimals.Add(new Herbivore2(herbivore2Prefab, placementManager));
    }
}
