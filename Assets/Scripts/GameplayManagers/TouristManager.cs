using System.Collections.Generic;
using UnityEngine;
using System;

public class TouristManager : MonoBehaviour, ITimeHandler
{
    public float Satisfaction { get; private set; } = Constants.DefaultSatisfaction;
    private int touristCount = 0;
    public int TouristsInQueue { get; private set; } = 0;
    public PlacementManager placementManager;
    private List<Jeep> jeeps = new List<Jeep>();
    public GameObject jeepPrefab;
    public Action<float> SatisfactionChanged;
    public Action<int> TouristsInQueueChanged;
    public Action<int> JeepCountChanged;

    private void Start()
    {
        Jeep.JeepWaiting += FillJeep;
        Jeep.JeepArrived += TouristsLeave;
    }
    private void Update()
    {
        foreach (var jeep in jeeps)
        {
            jeep.CheckState();
        }
    }
    
    public int JeepCount => jeeps.Count;

    private void TouristsLeave(Jeep jeep)
    {
        Debug.Log("Tourists leaving jeep " + jeep.tourists.CalculateSatisfaction());
        ModifySatisfaction(jeep.tourists.CalculateSatisfaction());
    }

    private void FillJeep(Jeep jeep)
    {
        if (TouristsInQueue > 0)
        {
            TouristsInQueue--;
            jeep.tourists.AddTourist();
            TouristsInQueueChanged?.Invoke(TouristsInQueue);
        }
    }

    private void ModifySatisfaction(int satisfaction)
    {
        if (touristCount == 0)
        {
            Satisfaction = (Satisfaction + satisfaction * 4.0f) / 2;
        }
        else
        {
            Satisfaction = (Satisfaction + satisfaction * (4.0f / touristCount)) / 2;
        }
        Satisfaction = Mathf.Clamp(Satisfaction, 0.0f, 100.0f);
        SatisfactionChanged?.Invoke(Satisfaction);
    }

    public void AcquireNewJeep()
    {
        Jeep jeep = new Jeep(placementManager, jeepPrefab, this);
        jeeps.Add(jeep);
        placementManager.RegisterObject(jeep.Id, ObjectType.Jeep, jeep);
        JeepCountChanged?.Invoke(JeepCount);
    }

    public void ManageTick()
    {
        int newTourists = 1; // logic to calculate how many tourists arrive
        touristCount += newTourists;
        TouristsInQueue += newTourists;
        TouristsInQueueChanged?.Invoke(TouristsInQueue);
    }
}

