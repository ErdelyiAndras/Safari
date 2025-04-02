using System.Collections.Generic;
using UnityEngine;
using System;


public class AnimalManager : MonoBehaviour, ITimeHandler
{
    public PlacementManager placementManager;
    public GameObject carnivore1Prefab, carnivore2Prefab, herbivore1Prefab, herbivore2Prefab;
    private List<Herd> herds = new List<Herd>();
    private Dictionary<AnimalType, uint> animalCount;
    public Action<AnimalType> NewAnimal, AnimalDied;
    

    private void Update()
    {
        foreach(var herd in herds)
        {
            herd.CalculateCentroid();
            herd.CheckState();
        }
    }

    public uint GetAnimalCount(AnimalType type)
    {
        if (animalCount.ContainsKey(type))
        {
            return animalCount[type];
        }
        return 0;
    }

    public void ManageTick()
    {
        foreach (var herd in herds)
        {
            herd.AgeAnimals();
        }
    }

    public void SetSpeedMultiplier(float multiplier)
    {
        foreach (var herd in herds)
        {
            herd.SetSpeedMultiplier(multiplier);
        }
    }

    public void BuyCarnivore1()
    {
        Herd _herd = ChooseHerd();
        Animal animal = new Carnivore1(herbivore1Prefab, placementManager, _herd, this);
        InitAnimal(_herd, animal);
    }

    public void BuyCarnivore2()
    {
        Herd _herd = ChooseHerd();
        Animal animal = new Carnivore2(herbivore1Prefab, placementManager, _herd, this);
        InitAnimal(_herd, animal);
    }

    public void BuyHerbivore1()
    {
        Herd _herd = ChooseHerd();
        Animal animal = new Herbivore1(herbivore1Prefab, placementManager, _herd, this);
        InitAnimal(_herd, animal);
    }

    public void BuyHerbivore2()
    {
        Herd _herd = ChooseHerd();
        Animal animal = new Herbivore2(herbivore1Prefab, placementManager, _herd, this);
        InitAnimal(_herd, animal);
    }

    private Herd ChooseHerd()
    {
        if (herds.Count == 0)
        {
            Herd newHerd = new Herd(placementManager);
            herds.Add(newHerd);
            return newHerd;
        }
        int mincount = int.MaxValue;
        Herd choosenHerd = null;
        int random = UnityEngine.Random.Range(0, 10);
        if (random < 8)
        {
            foreach(var herd in herds)
            {
                if (herd.Count < mincount)
                {
                    mincount = herd.Count;
                    choosenHerd = herd;
                }
            }
            return choosenHerd;
            
        }
        else
        {
            Herd newHerd = new Herd(placementManager);
            herds.Add(newHerd);
            return newHerd;
        }
    }

    private void InitAnimal(Herd animalHerd, Animal animal)
    {
        animal.AnimalDied += DeleteAnimalFromHerd;
        animalHerd.AddAnimalToHerd(animal);
        SetAnimalCount(animal.type);
        NewAnimal?.Invoke(animal.type);
    }

    private void SetAnimalCount(AnimalType type)
    {
        if (animalCount.ContainsKey(type))
        {
            animalCount[type]++;
        }
        else
        {
            animalCount.Add(type, 1);
        }
    }

    private void DeleteAnimalFromHerd(Animal animal)
    {
        animal.myHerd.RemoveAnimalFromHerd(animal);
        try
        {
            animalCount[animal.type]--;
        }
        catch
        {
            throw new Exception("Critical failure: Animal count cannot be negative");
        }
        AnimalDied?.Invoke(animal.type);
    }
}
