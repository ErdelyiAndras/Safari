using System.Collections.Generic;
using UnityEngine;
using System;

public class JeepSearchInRange : SearchInRange
{
    private HashSet<Guid> animalsSeen;
    private HashSet<AnimalType> animalTypesSeen;

    public JeepSearchInRange(float _visionRange, PlacementManager placementManager) : base(_visionRange, placementManager)
    {
        SetDefault();
    }

    public int AnimalsSeenCount => animalsSeen.Count;
    public int AnimalTypesSeenCount => animalTypesSeen.Count;

    private void AddSeenAnimal(Animal animal)
    {
        animalsSeen.Add(animal.Id);
        animalTypesSeen.Add(animal.Type);
    }

    public override void SearchInViewDistance(Vector3 Position)
    {
        foreach (var animal in placementManager.PlacedObjects.AnimalObjects)
        {
            if (animal.Distance(Position) < visionRange)
            {
                AddSeenAnimal((Animal)animal.entity);
            }
        }
    }

    public void SetDefault()
    {
        animalsSeen = new HashSet<Guid>();
        animalTypesSeen = new HashSet<AnimalType>();
    }
}

