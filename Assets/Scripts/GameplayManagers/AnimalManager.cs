using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using System;
using System.Xml;


public class AnimalManager : MonoBehaviour
{
    public PlacementManager placementManager;
    public GameObject carnivore1Prefab, carnivore2Prefab, herbivore1Prefab, herbivore2Prefab;
    private List<Herd> herds = new List<Herd>();

    private void Update()
    {
        foreach(var herd in herds)
        {
            herd.ManageAnimals();
            herd.CalculateCentroid();
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
        Herd id = ChooseHerd();
        new Herbivore1(herbivore1Prefab, placementManager, id);
    }
    public void BuyHerbivore2()
    {
        Herd id = ChooseHerd();
        new Herbivore2(herbivore2Prefab, placementManager, id);
    }

    private Herd ChooseHerd()
    {
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
            Herd newHerd = new Herd();
            herds.Add(newHerd);
            return newHerd;
        }
    }
}
