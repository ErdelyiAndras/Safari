using System;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class JeepSearchInRangeData : SearchInRangeData
{
    [SerializeField]
    private List<string> animalsSeen;
    [SerializeField]
    private List<AnimalType> animalTypesSeen;

    public HashSet<Guid> AnimalsSeen
    {
        get
        {
            HashSet<Guid> animalsSeenSet = new HashSet<Guid>();
            foreach (string guid in animalsSeen)
            {
                animalsSeenSet.Add(Guid.Parse(guid));
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

    public JeepSearchInRangeData(float visionRange, HashSet<Guid> animalsSeen, HashSet<AnimalType> animalTypesSeen) : 
        base(visionRange)
    {
        this.animalsSeen = new List<string>();
        foreach (Guid guid in animalsSeen)
        {
            this.animalsSeen.Add(guid.ToString());
        }
        this.animalTypesSeen = new List<AnimalType>();
        foreach (AnimalType type in animalTypesSeen)
        {
            this.animalTypesSeen.Add(type);
        }
    }
}