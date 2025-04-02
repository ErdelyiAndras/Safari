using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using System;
using System.Xml;


public class AnimalManager : MonoBehaviour, ITimeHandler
{
    public PlacementManager placementManager;
    public GameObject carnivore1Prefab, carnivore2Prefab, herbivore1Prefab, herbivore2Prefab;
    private List<Herd> herds = new List<Herd>();

    private void Update()
    {
        foreach(var herd in herds)
        {
            herd.CalculateCentroid();
            herd.CheckState();
        }
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
        Herd id = ChooseHerd();
        new Carnivore1(carnivore1Prefab, placementManager, id);
    }
    public void BuyCarnivore2()
    {
        Herd id = ChooseHerd();
        new Carnivore2(carnivore2Prefab, placementManager, id);
    }
    public void BuyHerbivore1()
    {
        Debug.Log("Buying Herbivore 1");
        Herd id = ChooseHerd();
        Animal animal = new Herbivore1(herbivore1Prefab, placementManager, id);
        Debug.Log(animal);
        id.AddAnimalToHerd(animal);
    }
    public void BuyHerbivore2()
    {
        Herd id = ChooseHerd();
        new Herbivore2(herbivore2Prefab, placementManager, id);
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
}
