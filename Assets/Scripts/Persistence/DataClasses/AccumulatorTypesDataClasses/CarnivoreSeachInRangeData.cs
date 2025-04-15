using System;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class CarnivoreSearchInRangeData : AnimalSearchInRangeData
{
    [SerializeField]
    private string preyGuid;
    [SerializeField]
    private string closestHerd;

    public Guid PreyGuid
    {
        get
        {
            return Guid.Parse(preyGuid);
        }
    }

    public Guid ClosestHerd
    {
        get
        {
            return Guid.Parse(closestHerd);
        }
    }

    public CarnivoreSearchInRangeData(
        float visionRange, List<Vector3> discoveredDrink, List<Vector3> drinkInRange, float viewExtenderScale,
        Guid preyGuid, Guid closestHerd
    ) : base(visionRange, discoveredDrink, drinkInRange, viewExtenderScale)
    {
        this.preyGuid = preyGuid.ToString();
        this.closestHerd = closestHerd.ToString();
    }
}