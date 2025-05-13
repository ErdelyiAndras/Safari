using System;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class CarnivoreHerdData : HerdData
{
    [SerializeField]
    private List<CarnivoreData> animals;

    public List<Animal> Animals(PlacementManager placementManager, CarnivoreHerd parent)
    {
        List<Animal> animalList = new List<Animal>();
        foreach (CarnivoreData animalData in animals)
        {
            Animal animal;
            switch (animalData.State.type)
            {
                case AnimalType.Carnivore1:
                    animal = new Carnivore1(animalData, placementManager, placementManager.prefabManager.Carnivore1Prefab, parent.ObjectInstance);
                    break;
                case AnimalType.Carnivore2:
                    animal = new Carnivore2(animalData, placementManager, placementManager.prefabManager.Carnivore2Prefab, parent.ObjectInstance);
                    break;
                default:
                    throw new System.Exception($"Unknown animal type {animalData.State.type}");
            }
            animalList.Add(animal);
        }
        return animalList;
    }

    public CarnivoreHerdData(
        Guid guid, AnimalType animalTypesOfHerd, List<Animal> animals, Vector2Int centroid, int distributionRadius, int reproductionCoolDown
    ) : base(guid, animalTypesOfHerd, centroid, distributionRadius, reproductionCoolDown)
    {
        this.animals = new List<CarnivoreData>();
        foreach (Animal animal in animals)
        {
            if (animal is Carnivore1 carnivore1)
            {
                CarnivoreData carnivoreData = (CarnivoreData)carnivore1.SaveData();
                this.animals.Add(carnivoreData);
            }
            else if (animal is Carnivore2 carnivore2)
            {
                CarnivoreData carnivoreData = (CarnivoreData)carnivore2.SaveData();
                this.animals.Add(carnivoreData);
            }
            else
            {
                throw new System.Exception("Unknown animal type");
            }
        }
    }
}