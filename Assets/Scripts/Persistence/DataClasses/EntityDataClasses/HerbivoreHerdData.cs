using System;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class HerbivoreHerdData : HerdData
{
    [SerializeField]
    private List<HerbivoreData> animals;

    public List<Animal> Animals(PlacementManager placementManager, HerbivoreHerd parent)
    {
        List<Animal> animalList = new List<Animal>();
        foreach (HerbivoreData animalData in animals)
        {
            Animal animal;
            switch (animalData.State.type)
            {
                case AnimalType.Herbivore1:
                    animal = new Herbivore1(animalData, placementManager, placementManager.prefabManager.Herbivore1Prefab, parent.gameObject);
                    break;
                case AnimalType.Herbivore2:
                    animal = new Herbivore2(animalData, placementManager, placementManager.prefabManager.Herbivore2Prefab, parent.gameObject);
                    break;
                /*case AnimalType.Carnivore1:
                    animal = new Carnivore1((CarnivoreData)animalData, placementManager, animalManager.carnivore1Prefab, parent.gameObject);
                    break;
                case AnimalType.Carnivore2:
                    animal = new Carnivore2((CarnivoreData)animalData, placementManager, animalManager.carnivore2Prefab, parent.gameObject);
                    break;*/
                default:
                    throw new System.Exception("Unknown animal type");
            }
            animalList.Add(animal);
        }
        return animalList;
    }

    public HerbivoreHerdData(
        Guid guid, AnimalType animalTypesOfHerd, List<Animal> animals, Vector2Int centroid, int distributionRadius, int reproductionCoolDown
    ) : base(guid, animalTypesOfHerd, centroid, distributionRadius, reproductionCoolDown)
    {
        this.animals = new List<HerbivoreData>();
        foreach (Animal animal in animals)
        {
            if (animal is Herbivore1 herbivore)
            {
                HerbivoreData herbivoreData = (HerbivoreData)herbivore.SaveData();
                this.animals.Add(herbivoreData);
            }
            else if (animal is Herbivore2 herbivore2)
            {
                HerbivoreData herbivoreData = (HerbivoreData)herbivore2.SaveData();
                this.animals.Add(herbivoreData);
            }
            else
            {
                throw new System.Exception("Unknown animal type");
            }
        }
    }
}