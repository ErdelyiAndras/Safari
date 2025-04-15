using System;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public abstract class HerdData
{
    [SerializeField]
    private string guid;
    [SerializeField]
    private AnimalType animalTypesOfHerd;
    [SerializeField]
    private Vector2Int centroid;
    [SerializeField]
    private int distributionRadius;
    [SerializeField]
    private int reproductionCoolDown;

    public Guid Guid
    {
        get
        {
            return Guid.Parse(guid);
        }
    }

    public AnimalType AnimalTypesOfHerd
    {
        get
        {
            return animalTypesOfHerd;
        }
    }

    public Vector2Int Centroid
    {
        get
        {
            return centroid;
        }
    }

    public int DistributionRadius
    {
        get
        {
            return distributionRadius;
        }
    }

    public int ReproductionCoolDown
    {
        get
        {
            return reproductionCoolDown;
        }
    }

    public HerdData(Guid guid, AnimalType animalTypesOfHerd, Vector2Int centroid, int distributionRadius, int reproductionCoolDown)
    {
        this.guid = guid.ToString();
        this.animalTypesOfHerd = animalTypesOfHerd;
        this.centroid = centroid;
        this.distributionRadius = distributionRadius;
        this.reproductionCoolDown = reproductionCoolDown;
    }
}