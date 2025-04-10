using System.Collections.Generic;
using UnityEngine;
using System;

public class TouristManager : MonoBehaviour, ITimeHandler
{
    public float Satisfaction { get; private set; }
    private int touristCount;
    public int TouristsInQueue { get; private set; }
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
        ModifySatisfaction(jeep.CalculateSatisfaction());
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
        Satisfaction = (Satisfaction + satisfaction * (4 / 1)) / 2;
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
        TouristsInQueue += 1;
        TouristsInQueueChanged?.Invoke(TouristsInQueue);
        // logic to calculate how many tourists arrive
    }
}

