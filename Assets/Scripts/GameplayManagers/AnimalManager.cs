using System.Collections.Generic;
using UnityEngine;
using System;
using System.Linq;
using Unity.VisualScripting;


public class AnimalManager : MonoBehaviour, ITimeHandler
{
    public PlacementManager placementManager;
    public GameObject carnivore1Prefab, carnivore2Prefab, herbivore1Prefab, herbivore2Prefab;
    private List<Herd> herds = new List<Herd>();
    private Dictionary<AnimalType, uint> animalCount = new Dictionary<AnimalType, uint>();
    public Action<uint> Carnivore1Changed, Carnivore2Changed, Herbivore1Changed, Herbivore2Changed;
    private Dictionary<AnimalType, Action<uint>> animalChangedActions;

    private void Start()
    {
        animalChangedActions = new Dictionary<AnimalType, Action<uint>>
        {
            { AnimalType.Carnivore1, Carnivore1Changed },
            { AnimalType.Carnivore2, Carnivore2Changed },
            { AnimalType.Herbivore1, Herbivore1Changed },
            { AnimalType.Herbivore2, Herbivore2Changed },
        };
    }
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

    public void BuyCarnivore1()
    {
        Herd _herd = ChooseHerd(HerdType.Carnivore1Herd);
        Animal animal = new Carnivore1(carnivore1Prefab, placementManager, _herd);
        InitAnimal(_herd, animal);
    }

    public void BuyCarnivore2()
    {
        Herd _herd = ChooseHerd(HerdType.Carnivore2Herd);
        Animal animal = new Carnivore2(carnivore2Prefab, placementManager, _herd);
        InitAnimal(_herd, animal);
    }

    public void BuyHerbivore1()
    {
        Herd _herd = ChooseHerd(HerdType.Herbivore1Herd);
        Animal animal = new Herbivore1(herbivore1Prefab, placementManager, _herd);
        InitAnimal(_herd, animal);
    }

    public void BuyHerbivore2()
    {
        Herd _herd = ChooseHerd(HerdType.Herbivore2Herd);
        Animal animal = new Herbivore2(herbivore2Prefab, placementManager, _herd);
        InitAnimal(_herd, animal);
    }

    //TODO: ha van 1 elemű csorda akkor ne jöhesen létre random, hanem abba kerüljön az új állat
    private Herd ChooseHerd(HerdType type)
    {
        var herdsOfType = herds.Where(h => h.herdType == type);
        if (herdsOfType.Count() == 0)
        {
            Herd newHerd =  type == HerdType.Herbivore1Herd || 
                            type == HerdType.Herbivore2Herd ? 
                            new HerbivoreHerd(placementManager, this, type) : new CarnivoreHerd(placementManager, this, type);
            herds.Add(newHerd);
            return newHerd;
        }
        int random = UnityEngine.Random.Range(0, 10);
        if (random < 8)
        {
            return herdsOfType.OrderBy(h => h.Count).First();
        }
        else
        {
            Herd newHerd =  type == HerdType.Herbivore1Herd || 
                            type == HerdType.Herbivore2Herd ? 
                            new HerbivoreHerd(placementManager, this, type) : new CarnivoreHerd(placementManager, this, type);
            herds.Add(newHerd);
            return newHerd;
        }
    }

    private void InitAnimal(Herd animalHerd, Animal animal)
    {
        animal.AnimalDied += DeleteAnimalFromHerd;
        animalHerd.AddAnimalToHerd(animal);
        SetAnimalCount(animal.type);
        animalChangedActions[animal.type]?.Invoke(animalCount[animal.type]);
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
        if (animalCount[animal.type] != 0)
        {
            animalCount[animal.type]--;
            animalChangedActions[animal.type]?.Invoke(animalCount[animal.type]);
        }
    }
}
