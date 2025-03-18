using System;
using UnityEngine;

public class TimeManager : MonoBehaviour
{
    // TODO: csak a unity miatt public
    public TimeInterval timeInterval;

    public Action Elapsed;

    public float baseSpeed = 30.0f;

    public float hourSpeedMultiplier = 1.0f;
    public float daySpeedMultiplier = 2.0f;
    public float weekSpeedMultiplier = 3.0f;

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

    private float elapsedTime;
    private bool isPaused = true;

    public bool IsPaused
    {
        get { return isPaused; }
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
        baseSpeed = 30.0f;
        elapsedTime = 0.0f;
        isPaused = true;
        timeInterval = TimeInterval.HOUR;
    }

    private void Update()
    {
        if (isPaused)
        {
            return;
        }

        if (elapsedTime < WaitTime)
        {
            elapsedTime += Time.deltaTime;
        }
        else
        {
            elapsedTime = 0.0f;
            Elapsed?.Invoke();
        }
    }

    public void SetTimeInterval(TimeInterval timeInterval)
    {
        this.timeInterval = timeInterval;
    }

    public void Pause()
    {
        isPaused = true;
    }

    public void Resume()
    {
        isPaused = false;
    }
}
