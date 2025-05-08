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
    [SerializeField]
    private int lastDayNewTourists;
    [SerializeField]
    private int monthlyTouristsDays;
    [SerializeField]
    private int monthlyTouristsTourists;
    [SerializeField]
    private int getConditionPassedDays;

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

    public int LastDayNewTourists
    {
        get { return lastDayNewTourists; }
    }
    
    public TouristManager.MonthlyTourists MonthlyTourists
    {
        get { return new TouristManager.MonthlyTourists() { days = monthlyTouristsDays, tourists = monthlyTouristsTourists }; }
    }

    public int GetConditionPassedDays
    {
        get { return getConditionPassedDays; }
    }

    public TouristManagerData(
        float satisfaction, int touristCount, int touristsInQueue, List<Jeep> jeeps, int lastDayNewTourists, TouristManager.MonthlyTourists monthlyTourists, int getConditionPassedDays
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
        this.lastDayNewTourists = lastDayNewTourists;
        monthlyTouristsDays = monthlyTourists.days;
        monthlyTouristsTourists = monthlyTourists.tourists;
        this.getConditionPassedDays = getConditionPassedDays;
    }
}