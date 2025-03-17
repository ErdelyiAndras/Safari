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
            return timeInterval switch
            {
                TimeInterval.PAUSED => 0.0f,
                TimeInterval.HOUR => hourInterval / hourInterval,
                TimeInterval.DAY => dayInterval / hourInterval,
                TimeInterval.WEEK => weekInterval / hourInterval,
                _ => throw new ArgumentOutOfRangeException("Invalid TimeInterval")
            };
        }
    }

    private float elapsedTime;

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

    private void Start()
    {
        elapsedTime = 0.0f;
        timeInterval = TimeInterval.PAUSED;
    }

    private void Update()
    {
        if (timeInterval == TimeInterval.PAUSED)
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
        timeInterval = TimeInterval.PAUSED;
    }
}
