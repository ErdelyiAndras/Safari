using System;
using UnityEngine;

public class TimeManager : MonoBehaviour
{
    // TODO: csak a unity miatt public
    public TimeInterval timeInterval;

    public Action Elapsed;
    public Action TimeIntervalChanged;

    public float baseSpeed = Constants.BaseTimeSpeed;

    public float hourSpeedMultiplier = Constants.HourSpeedMultiplier;
    public float daySpeedMultiplier = Constants.DaySpeedMultiplier;
    public float weekSpeedMultiplier = Constants.WeekSpeedMultiplier;

    public float EntitySpeedMultiplier
    {
        get
        {
            if (isPaused)
            {
                return 0.0f;
            }

            return timeInterval switch
            {
                TimeInterval.HOUR => hourSpeedMultiplier,
                TimeInterval.DAY => daySpeedMultiplier,
                TimeInterval.WEEK => weekSpeedMultiplier,
                _ => throw new ArgumentOutOfRangeException("Invalid TimeInterval")
            };
        }
    }

    private DateTime currentTime;
    private float elapsedTime;
    private bool isPaused;

    private int tickCounter;

    public bool IsPaused
    {
        get { return isPaused; }
    }

    public string CurrentTime
    {
        get { return currentTime.ToString("yyyy-MM-dd"); }
    }

    private float WaitTime
    {
        get
        {
            return timeInterval switch
            {
                TimeInterval.HOUR => baseSpeed / hourSpeedMultiplier,
                TimeInterval.DAY => baseSpeed / daySpeedMultiplier,
                TimeInterval.WEEK => baseSpeed / weekSpeedMultiplier,
                _ => throw new ArgumentOutOfRangeException("Invalid TimeInterval")
            };
        }
    }

    private void Awake()
    {
        tickCounter = 0;

        elapsedTime = 0.0f;
        isPaused = false;
        timeInterval = Constants.DefaultTimeInterval;
        currentTime = DateTime.Now;
    }

    private void Update()
    {
        if (isPaused)
        {
            return;
        }

        if (elapsedTime < 1.0f)
        {
            elapsedTime += Time.deltaTime;
        }
        else
        {
            tickCounter++;
            // Debug.Log($"Sec: {tickCounter}");
            elapsedTime = 0.0f; 
            if (tickCounter % WaitTime == 0)
            {
                tickCounter = 0;
                currentTime = currentTime.AddDays(1);
                Elapsed?.Invoke();
            }
        }
    }

    public void SetTimeInterval(TimeInterval timeInterval)
    {
        this.timeInterval = timeInterval;
        TimeIntervalChanged?.Invoke();
    }

    public void TogglePause()
    {
        isPaused = !isPaused;
        TimeIntervalChanged?.Invoke();
    }
}
