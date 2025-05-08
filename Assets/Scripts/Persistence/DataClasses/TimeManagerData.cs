using System;
using UnityEngine;

[System.Serializable]
public class TimeManagerData
{
    [SerializeField]
    private string currentTime;
    [SerializeField]
    private float elapsedTime;
    [SerializeField]
    private int tickCounter;

    public DateTime CurrentTime
    {
        get { return DateTime.Parse(currentTime); }
    }

    public float ElapsedTime
    {
        get { return elapsedTime; }
    }

    public int TickCounter
    {
        get { return tickCounter; }
    }

    public TimeManagerData(DateTime currentTime, float elapsedTime, int tickCounter)
    {
        this.currentTime = currentTime.ToString("o");
        this.elapsedTime = elapsedTime;
        this.tickCounter = tickCounter;
    }
}