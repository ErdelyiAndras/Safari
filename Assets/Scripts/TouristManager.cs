using NUnit.Framework;
using Unity.VisualScripting;
using System.Collections.Generic;
using UnityEngine;

public class TouristManager : MonoBehaviour
{
    public float Satisfaction { get; private set; }
    private int touristCount;
    private int touristsInQueue;
    public PlacementManager placementManager; // better solution for this? 
    private List<Jeep> jeeps = new List<Jeep>();

    private void Start()
    {
        Jeep.JeepWaiting += FillJeep;
        Jeep.JeepArrived += TouristsLeave;
    }

    private void TouristsArrive()
    {
        touristsInQueue += 1;
        // logic to calculate how many tourists arrive
    }
    private void TouristsLeave(Jeep jeep)
    {
        ModifySatisfaction(jeep.tourists.CalculateSatisfaction());
        jeep.Return();
    }

    public void HandleTImeElapsed() // EZZEL KELL FELIRATKOZNI A TICK-re
    {
        TouristsArrive();
    }
    private void FillJeep(Jeep jeep)
    {
        if (touristsInQueue > 0)
        {
            touristsInQueue--;
            jeep.tourists.AddTourist();
        }
    }

    private void ModifySatisfaction(int satisfaction)
    {
        Satisfaction = (Satisfaction + satisfaction) / 2;
    }
    public void AcquireNewJeep()
    {
        jeeps.Add(new Jeep(placementManager));
    }
}

