using System;
using UnityEngine;

public class TimeManager : MonoBehaviour
{
    // TODO: csak a unity miatt public
    public TimeInterval timeInterval;

    public Action Elapsed;

    public float hourInterval = 10.0f;
    public float dayInterval = 20.0f;
    public float weekInterval = 30.0f;

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

                TimeInterval.HOUR => hourInterval / hourInterval,
                TimeInterval.DAY => dayInterval / hourInterval,
                TimeInterval.WEEK => weekInterval / hourInterval,
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
                TimeInterval.HOUR => hourInterval,
                TimeInterval.DAY => dayInterval,
                TimeInterval.WEEK => weekInterval,
                _ => throw new ArgumentOutOfRangeException("Invalid TimeInterval")
            };
        }
    }

    private void Awake()
    {
        elapsedTime = 0.0f;
        isPaused = true;
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
