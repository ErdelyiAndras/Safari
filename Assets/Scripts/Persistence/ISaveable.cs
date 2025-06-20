﻿public interface ISaveable<T>
{
    T SaveData();
    void LoadData(T data, PlacementManager placementManager = null);
}