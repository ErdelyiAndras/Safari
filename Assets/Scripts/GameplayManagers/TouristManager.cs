using System.Collections.Generic;
using UnityEngine;
using System;

public class TouristManager : MonoWinCondition, ITimeHandler, ISaveable<TouristManagerData>
{
    private struct MonthlyTourists
    {
        public int days;
        public int tourists;
    }
    public float Satisfaction { get; private set; } = Constants.DefaultSatisfaction;
    private int touristCount = 0;
    private MonthlyTourists monthlyTourists = new MonthlyTourists() { days = 0, tourists = 0};
    public int TouristsInQueue { get; private set; } = 0;
    private int lastDayNewTourists = 0;
    public PlacementManager placementManager;
    private List<Jeep> jeeps = new List<Jeep>();
    private GameObject jeepPrefab;
    public Action<float> SatisfactionChanged;
    public Action<int> TouristsInQueueChanged;
    public Action<int> JeepCountChanged;
    public Action<Jeep> AcquireAdmissionFee;
    private void Start()
    {
        jeepPrefab = placementManager.prefabManager.JeepPrefab;
        Jeep.JeepWaiting += FillJeep;
        Jeep.JeepArrived += TouristsLeave;
        Jeep.AcquireAdmissionFee += jeep => AcquireAdmissionFee?.Invoke(jeep);
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

    private void ModifySatisfaction(float satisfaction)
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
        lastDayNewTourists = (int)(Satisfaction / 10.0f) + 1; // logic to calculate how many tourists arrive
        touristCount += lastDayNewTourists;
        TouristsInQueue += lastDayNewTourists;
        TouristsInQueueChanged?.Invoke(TouristsInQueue);
        SetConditionPassedDays();
    }

    public TouristManagerData SaveData()
    {
        return new TouristManagerData(Satisfaction, touristCount, TouristsInQueue, jeeps);
    }

    public void LoadData(TouristManagerData data, PlacementManager placementManager)
    {
        ResetData();
        Satisfaction = data.Satisfaction;
        touristCount = data.TouristCount;
        TouristsInQueue = data.TouristsInQueue;
        jeeps = data.Jeeps(placementManager, this);
        this.placementManager = placementManager;
        foreach (Jeep jeep in jeeps)
        {
            placementManager.RegisterObject(jeep.Id, ObjectType.Jeep, jeep);
        }
    }

    private void ResetData()
    {
        for (int i = 0; i < jeeps.Count; i++)
        {
            jeeps[i].DeleteGameObject();
        }
    }
    protected override void SetConditionPassedDays()
    {
        if (
            monthlyTourists.days >= 30
            &&
            monthlyTourists.tourists >= Constants.VisitorWinCondition[DifficultySelector.SelectedDifficulty]
           )
        {
            GetConditionPassedDays += 30;
            monthlyTourists.days = 0;
            monthlyTourists.tourists = 0;
        }
        else if (monthlyTourists.days >= 30)
        {
            GetConditionPassedDays = 0;
            monthlyTourists.days = 0;
            monthlyTourists.tourists = 0;
        }
        else
        {
            monthlyTourists.days++;
            monthlyTourists.tourists += lastDayNewTourists;
        }
    }
}

