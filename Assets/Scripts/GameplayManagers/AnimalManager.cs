using System.Collections.Generic;
using UnityEngine;
using System;
using System.Linq;

public class AnimalManager : MonoBehaviour, ITimeHandler
{
    public PlacementManager placementManager;
    public GameObject carnivore1Prefab, carnivore2Prefab, herbivore1Prefab, herbivore2Prefab;
    private List<Herd> herds = new List<Herd>();
    public Action<uint> Carnivore1Changed, Carnivore2Changed, Herbivore1Changed, Herbivore2Changed;

    public uint Carnivore1Count => GetAnimalCount(AnimalType.Carnivore1);
    public uint Carnivore2Count => GetAnimalCount(AnimalType.Carnivore2);
    public uint Herbivore1Count => GetAnimalCount(AnimalType.Herbivore1);
    public uint Herbivore2Count => GetAnimalCount(AnimalType.Herbivore2);
    private void Update()
    {
        for (int i = herds.Count - 1; i >= 0; i--)
        {
            if (herds[i].Count == 0)
            {
                Destroy(herds[i].gameObject);
                placementManager.PlacedObjects.DeleteObject(herds[i].Id);
                herds.RemoveAt(i);
                continue;
            }
            herds[i].CalculateCentroid();
            herds[i].CheckState();
        }
    }

    private uint GetAnimalCount(AnimalType type)
    {
        /*if (animalCount.ContainsKey(type))
        {
            return animalCount[type];
        }
        return 0;*/
        return (uint)herds.Where(h => h.AnimalTypesOfHerd == type).Sum(x => x.Count);

    }

    public void ManageTick()
    {
        foreach (var herd in herds)
        {
            herd.AgeAnimals();
        }
    }

    public void BuyCarnivore1()
    {
        Herd _herd = ChooseHerd(AnimalType.Carnivore1);
        Animal animal = new Carnivore1(carnivore1Prefab, placementManager, _herd.Id);
        InitAnimal(_herd, animal);
        placementManager.RegisterObject(animal.Id, ObjectType.Carnivore, animal);
    }

    public void BuyCarnivore2()
    {
        Herd _herd = ChooseHerd(AnimalType.Carnivore2);
        Animal animal = new Carnivore2(carnivore2Prefab, placementManager, _herd.Id);
        InitAnimal(_herd, animal);
        placementManager.RegisterObject(animal.Id, ObjectType.Carnivore, animal);
    }

    public void BuyHerbivore1()
    {
        Herd _herd = ChooseHerd(AnimalType.Herbivore1);
        Animal animal = new Herbivore1(herbivore1Prefab, placementManager, _herd.Id);
        InitAnimal(_herd, animal);
        placementManager.RegisterObject(animal.Id, ObjectType.Herbivore, animal);
    }

    public void BuyHerbivore2()
    {
        Herd _herd = ChooseHerd(AnimalType.Herbivore2);
        Animal animal = new Herbivore2(herbivore2Prefab, placementManager, _herd.Id);
        InitAnimal(_herd, animal);
        placementManager.RegisterObject(animal.Id, ObjectType.Herbivore, animal);

    }

    //TODO: ha van 1 elemű csorda akkor ne jöhesen létre random, hanem abba kerüljön az új állat
    private Herd ChooseHerd(AnimalType type)
    {
        var herdsOfType = herds.Where(h => h.animalTypesOfHerd == type);
        if (herdsOfType.Count() == 0)
        {
            return CreateNewHerd(type);
        }
        int random = UnityEngine.Random.Range(0, 10);
        if (random < 8)
        {
            return herdsOfType.OrderBy(h => h.Count).First();
        }
        else
        {
            return CreateNewHerd(type);
        }
    }

    private Herd CreateNewHerd(AnimalType type)
    {
        Herd newHerd;
        if ( type == AnimalType.Herbivore1 || type == AnimalType.Herbivore2)
        {
            newHerd = new HerbivoreHerd(placementManager, this, type);
            placementManager.RegisterObject(newHerd.Id, ObjectType.HerbivoreHerd, newHerd);
        }
        else
        {
            newHerd = new CarnivoreHerd(placementManager, this, type);
            placementManager.RegisterObject(newHerd.Id, ObjectType.CarnivoreHerd, newHerd);
        }
        herds.Add(newHerd);
        newHerd.Reproduce += ReproduceAnimal;
        return newHerd;
    }

    private void InitAnimal(Herd animalHerd, Animal animal)
    {
        animal.AnimalDied += DeleteAnimalFromHerd;
        animalHerd.AddAnimalToHerd(animal);
        //SetAnimalCount(animal.Type);
        //animalChangedActions[animal.type]?.Invoke(animalCount[animal.type]);
        InvokeAnimalCountChanged(animal.Type);
    }

    /*
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
    */

    private void DeleteAnimalFromHerd(Animal animal)
    {
        placementManager.PlacedObjects.DeleteObject(animal.Id);
        animal.GetMyHerd.RemoveAnimalFromHerd(animal);
        /*if (animalCount[animal.Type] != 0)
        {
            animalCount[animal.Type]--;
            //animalChangedActions[animal.type]?.Invoke(animalCount[animal.type]);
        }*/
        InvokeAnimalCountChanged(animal.Type);
    }

    private void InvokeAnimalCountChanged(AnimalType type)
    {
        switch (type)
        {
            case AnimalType.Carnivore1:
                Carnivore1Changed?.Invoke(Carnivore1Count);
                break;
            case AnimalType.Carnivore2:
                Carnivore2Changed?.Invoke(Carnivore2Count);
                break;
            case AnimalType.Herbivore1:
                Herbivore1Changed?.Invoke(Herbivore1Count);
                break;
            case AnimalType.Herbivore2:
                Herbivore2Changed?.Invoke(Herbivore2Count);
                break;
        }
    }

    private void ReproduceAnimal(Herd _herd)
    {
        Animal children = null;
        switch (_herd.animalTypesOfHerd)
        {
            case AnimalType.Herbivore1:
                children = new Herbivore1(herbivore1Prefab, placementManager, _herd.Id);
                break;
            case AnimalType.Herbivore2:
                children = new Herbivore2(herbivore2Prefab, placementManager, _herd.Id);
                break;
            case AnimalType.Carnivore1:
                children = new Carnivore1(carnivore1Prefab, placementManager, _herd.Id);
                break;
            case AnimalType.Carnivore2:
                children = new Carnivore2(carnivore2Prefab, placementManager, _herd.Id);
                break;
            default:
                break;
        }
        InitAnimal(_herd, children);
        placementManager.RegisterObject(children.Id, ObjectType.Herbivore, children);
    }
}
