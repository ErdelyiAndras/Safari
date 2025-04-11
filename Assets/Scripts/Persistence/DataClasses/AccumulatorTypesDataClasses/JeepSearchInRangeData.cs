using System;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class JeepSearchInRangeData : SearchInRangeData
{
    [SerializeField]
    private List<Guid> animalsSeen;
    [SerializeField]
    private List<AnimalType> animalTypesSeen;

    public HashSet<Guid> AnimalsSeen
    {
        get
        {
            HashSet<Guid> animalsSeenSet = new HashSet<Guid>();
            foreach (Guid guid in animalsSeen)
            {
                animalsSeenSet.Add(guid);
            }
            return animalsSeenSet;
        }
    }

    public HashSet<AnimalType> AnimalTypesSeen
    {
        get
        {
            HashSet<AnimalType> animalTypesSeenSet = new HashSet<AnimalType>();
            foreach (AnimalType type in animalTypesSeenSet)
            {
                animalTypesSeenSet.Add(type);
            }
            return animalTypesSeenSet;
        }
    }

    public JeepSearchInRangeData(float visionRange, PlacementManager placementManager, HashSet<Guid> animalsSeen, HashSet<AnimalType> animalTypesSeen) : 
        base(visionRange, placementManager)
    {
        this.animalsSeen = new List<Guid>();
        foreach (Guid guid in animalsSeen)
        {
            this.animalsSeen.Add(guid);
        }
        this.animalTypesSeen = new List<AnimalType>();
        foreach (AnimalType type in animalTypesSeen)
        {
            this.animalTypesSeen.Add(type);
        }
    }
}