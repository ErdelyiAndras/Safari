﻿using System;

public class TouristGroup : ISaveable<TouristGroupData>
{
    public Action readyToGo;
    private int numberOfTourists;

    public TouristGroup()
    {
        numberOfTourists = 0;
    }

    public TouristGroup(TouristGroupData data)
    {
        LoadData(data);
    }
    
    public void AddTourist()
    {
        numberOfTourists++;
        if (IsTouristGroupFull())
        {
            readyToGo?.Invoke();
        }
    }

    public bool IsTouristGroupFull() => numberOfTourists == 4;


    public void SetDefault()
    {
        numberOfTourists = 0;
    }

    public TouristGroupData SaveData()
    {
        return new TouristGroupData(numberOfTourists);
    }
    
    public void LoadData(TouristGroupData data, PlacementManager placementManager = null)
    {
        numberOfTourists = data.NumberOfTourists;
    }
}
