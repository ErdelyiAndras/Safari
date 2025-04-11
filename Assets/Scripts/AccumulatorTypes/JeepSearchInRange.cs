using System.Collections.Generic;
using UnityEngine;
using System;

public class JeepSearchInRange : SearchInRange
{
    private HashSet<Guid> animalsSeen = new HashSet<Guid>();
    private HashSet<AnimalType> animalTypesSeen = new HashSet<AnimalType>();

    public JeepSearchInRange(float _visionRange, PlacementManager placementManager) : base(_visionRange, placementManager)
    {
        SetDefault();
    }

    public JeepSearchInRange(JeepSearchInRangeData data) : base(data)
    {
        LoadData(data);
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

    public override SearchInRangeData SaveData()
    {
        return new JeepSearchInRangeData(visionRange, placementManager, animalsSeen, animalTypesSeen);
    }

    public override void LoadData(SearchInRangeData data)
    {
        base.LoadData(data);
        animalsSeen = ((JeepSearchInRangeData)data).AnimalsSeen;
        animalTypesSeen = ((JeepSearchInRangeData)data).AnimalTypesSeen;
    }
}

