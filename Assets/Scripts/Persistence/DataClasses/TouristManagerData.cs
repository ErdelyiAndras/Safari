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
    private PlacementManagerData placementManagerData;
    [SerializeField]
    private List<JeepData> jeepsData;

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

    public PlacementManager PlacementManager
    {
        get 
        { 
            return new PlacementManager(); 
        }
    }

    public List<Jeep> Jeeps
    {
        get
        {
            return new List<Jeep>();
        }
    }

    public TouristManagerData(
        float satisfaction, int touristCount, int touristsInQueue, PlacementManager placementManager, List<Jeep> jeeps
    )
    {
        this.satisfaction = satisfaction;
        this.touristCount = touristCount;
        this.touristsInQueue = touristsInQueue;
        this.placementManagerData = new PlacementManagerData(3);
        this.jeepsData = new List<JeepData>();
        foreach (Jeep jeep in jeeps)
        {
            jeepsData.Add(jeep.SaveData());
        }
    }
}