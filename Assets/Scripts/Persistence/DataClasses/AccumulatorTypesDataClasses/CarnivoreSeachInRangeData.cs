using System;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class CarnivoreSearchInRangeData : AnimalSearchInRangeData
{
    //[SerializeField]
    //private List<HerdData> herds;
    [SerializeField]
    private string preyGuid;
    //[SerializeField]
    //private HerdData closestHerd;

    public Guid PreyGuid
    {
        get
        {
            return Guid.Parse(preyGuid);
        }
    }

    public CarnivoreSearchInRangeData(
        float visionRange, List<Vector3Int> discoveredDrink, List<Vector3Int> drinkInRange, float viewExtenderScale,
        List<Herd> herds, Guid preyGuid, Herd closestHerd
    ) : base(visionRange, discoveredDrink, drinkInRange, viewExtenderScale)
    {
        //this.herds = new List<HerdData>();
        //foreach (Herd herd in herds)
        //{
        //    this.herds.Add(herd.SaveData());
        //}
        this.preyGuid = preyGuid.ToString();
        //this.closestHerd = closestHerd.SaveData();
    }
}