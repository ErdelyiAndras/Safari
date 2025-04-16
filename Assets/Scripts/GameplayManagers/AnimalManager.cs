using System.Collections.Generic;
using UnityEngine;
using System;
using System.Linq;

public class AnimalManager : MonoBehaviour, ITimeHandler, ISaveable<AnimalManagerData>
{
    public PlacementManager placementManager;
    private GameObject carnivore1Prefab, carnivore2Prefab, herbivore1Prefab, herbivore2Prefab;
    private List<Herd> herds = new List<Herd>();
    public Action<uint> Carnivore1Changed, Carnivore2Changed, Herbivore1Changed, Herbivore2Changed;

    public uint Carnivore1Count => GetAnimalCount(AnimalType.Carnivore1);
    public uint Carnivore2Count => GetAnimalCount(AnimalType.Carnivore2);
    public uint Herbivore1Count => GetAnimalCount(AnimalType.Herbivore1);
    public uint Herbivore2Count => GetAnimalCount(AnimalType.Herbivore2);

    private void Start()
    {
        carnivore1Prefab = placementManager.prefabManager.Carnivore1Prefab;
        carnivore2Prefab = placementManager.prefabManager.Carnivore2Prefab;
        herbivore1Prefab = placementManager.prefabManager.Herbivore1Prefab;
        herbivore2Prefab = placementManager.prefabManager.Herbivore2Prefab;
    }

    private void Update()
    {
        for (int i = herds.Count - 1; i >= 0; i--)
        {
            if (herds[i].Count == 0)
            {
                Destroy(herds[i].ObjectInstance);
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
        IEnumerable<Herd> herdsOfType = herds.Where(h => h.AnimalTypesOfHerd == type);
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
        newHerd.animalRemovedFromHerd += h => InvokeAnimalCountChanged(h.AnimalTypesOfHerd);
        return newHerd;
    }

    private void InitAnimal(Herd animalHerd, Animal animal)
    {
        animalHerd.AddAnimalToHerd(animal);
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
        switch (_herd.AnimalTypesOfHerd)
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
        if (_herd.AnimalTypesOfHerd == AnimalType.Herbivore1 || _herd.AnimalTypesOfHerd == AnimalType.Herbivore2)
        {
            placementManager.RegisterObject(children.Id, ObjectType.Herbivore, children);
        }
        else if (_herd.AnimalTypesOfHerd == AnimalType.Carnivore1 || _herd.AnimalTypesOfHerd == AnimalType.Carnivore2)
        {
            placementManager.RegisterObject(children.Id, ObjectType.Carnivore, children);
        }
        else
        {
            throw new Exception("Invalid animal type");
        }
    }

    public AnimalManagerData SaveData()
    {
        return new AnimalManagerData(herds);
    }

    public void LoadData(AnimalManagerData data, PlacementManager placementManager)
    {
        ResetData();
        this.placementManager = placementManager;
        herds = data.Herds(placementManager, this);
        foreach (Herd herd in herds)
        {
            herd.Reproduce += ReproduceAnimal;
            herd.animalRemovedFromHerd += h => InvokeAnimalCountChanged(h.AnimalTypesOfHerd);
            if (herd.AnimalTypesOfHerd == AnimalType.Herbivore1 || herd.AnimalTypesOfHerd == AnimalType.Herbivore2)
            {
                placementManager.RegisterObject(herd.Id, ObjectType.HerbivoreHerd, (HerbivoreHerd)herd);
            }
            else if (herd.AnimalTypesOfHerd == AnimalType.Carnivore1 || herd.AnimalTypesOfHerd == AnimalType.Carnivore2)
            {
                placementManager.RegisterObject(herd.Id, ObjectType.CarnivoreHerd, (CarnivoreHerd)herd);
            }
            else
            {
                throw new Exception("Unknown animal type in herds");
            }
        }
    }

    private void ResetData()
    {
        foreach (Herd herd in herds)
        {
            herd.ResetData();
        }
    }

    public void SellAnimal(GameObject animal) => ((Animal)placementManager.PlacedObjects.GetGameObjectWrapper(animal)).AnimalDies();

}
