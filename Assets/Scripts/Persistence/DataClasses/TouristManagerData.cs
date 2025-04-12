using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class TouristManagerData
{
    [SerializeField]
    private float satisfaction;
    [SerializeField]
    private int touristCount;
    [SerializeField]
    private int touristsInQueue;
    [SerializeField]
    private List<JeepData> jeeps;

    public float Satisfaction
    {
        get { return satisfaction; }
    }
    
    public int TouristCount
    {
        get 
        { 
            return touristCount; 
        }
    }

    public int TouristsInQueue
    {
        get
        { 
            return touristsInQueue; 
        }
    }

    public List<Jeep> Jeeps(PlacementManager placementManager, TouristManager touristManager)
    {
        List<Jeep> jeepsList = new List<Jeep>();
        foreach (JeepData jeepData in jeeps)
        {
            jeepsList.Add(new Jeep(jeepData, placementManager, touristManager));
        }
        return jeepsList;
    }

    public TouristManagerData(
        float satisfaction, int touristCount, int touristsInQueue, List<Jeep> jeeps
    )
    {
        this.satisfaction = satisfaction;
        this.touristCount = touristCount;
        this.touristsInQueue = touristsInQueue;
        this.jeeps = new List<JeepData>();
        foreach (Jeep jeep in jeeps)
        {
            this.jeeps.Add((JeepData)jeep.SaveData());
        }
    }
}