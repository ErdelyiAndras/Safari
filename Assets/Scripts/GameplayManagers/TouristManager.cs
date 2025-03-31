using NUnit.Framework;
using Unity.VisualScripting;
using System.Collections.Generic;
using UnityEngine;
using System;

public class TouristManager : MonoBehaviour
{
    public float Satisfaction { get; private set; }
    private int touristCount;
    public int TouristsInQueue { get; private set; }
    public PlacementManager placementManager;
    private List<Jeep> jeeps = new List<Jeep>();
    public GameObject jeepPrefab;
    public Action<float> SatisfactionChanged; // TODO feliratkozni rá a UI változásához
    public Action<int> TouristCountChanged; // TODO feliratkozni rá a UI változásához

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

    public void TouristsArrive()
    {
        TouristsInQueue += 1;
        TouristCountChanged?.Invoke(touristCount);
        // logic to calculate how many tourists arrive
    }
    private void TouristsLeave(Jeep jeep)
    {
        ModifySatisfaction(jeep.tourists.CalculateSatisfaction());
    }

    public void SetSpeedMultiplier(float multiplier)
    {
        foreach (var jeep in jeeps)
        {
            jeep.SpeedMultiplier = multiplier;
        }
    }

    private void FillJeep(Jeep jeep)
    {
        if (TouristsInQueue > 0)
        {
            TouristsInQueue--;
            jeep.tourists.AddTourist();
        }
    }

    private void ModifySatisfaction(int satisfaction)
    {
        Satisfaction = (Satisfaction + satisfaction) / 2;
        SatisfactionChanged?.Invoke(Satisfaction);
    }
    public void AcquireNewJeep()
    {
        jeeps.Add(new Jeep(placementManager, jeepPrefab, this));
    }
}

