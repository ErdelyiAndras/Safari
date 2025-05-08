using NUnit.Framework;
using System.Collections;
using UnityEngine;
using UnityEngine.TestTools;

public class TimeManagerTests
{
    private TimeManager timeManager;

    [UnitySetUp]
    public IEnumerator Setup()
    {
        var go = new GameObject();
        timeManager = go.AddComponent<TimeManager>();
        yield return null;
    }

    [UnityTearDown]
    public IEnumerator TearDown()
    {
        Object.Destroy(timeManager.gameObject);
        yield return null;
    }

    [UnityTest]
    public IEnumerator TimeManagerAwakeTest()
    {
        Assert.IsFalse(timeManager.IsPaused);
        Assert.AreEqual(System.DateTime.Now.ToString("yyyy-MM-dd"), timeManager.CurrentTime);
        yield return null;
    }

    [UnityTest]
    public IEnumerator TimeManagerSetTimeIntervalTest()
    {
        bool timeIntervalChanged = false;
        timeManager.TimeIntervalChanged += () => timeIntervalChanged = true;
        timeManager.SetTimeInterval(TimeInterval.HOUR);
        Assert.IsTrue(timeIntervalChanged);
        yield return null;
    }

    [UnityTest]
    public IEnumerator TimeManagerTogglePauseTest()
    {
        bool timeIntervalChanged = false;
        timeManager.TimeIntervalChanged += () => timeIntervalChanged = true;
        timeManager.TogglePause();
        Assert.IsTrue(timeIntervalChanged && timeManager.IsPaused);
        yield return null;
    }

    [UnityTest]
    public IEnumerator TimeManagerSaveDataTest()
    {
        TimeManagerData data = timeManager.SaveData();
        Assert.IsTrue(System.DateTime.Now.AddSeconds(1) > data.CurrentTime);
        Assert.IsTrue(1 > data.ElapsedTime);
        Assert.AreEqual(0, data.TickCounter);
        yield return null;
    }

    [UnityTest]
    public IEnumerator TimeManagerLoadDataTest()
    {
        TimeManagerData data = new TimeManagerData(System.DateTime.Now.AddHours(10), 2.3f, 3);
        timeManager.LoadData(data);
        Assert.AreEqual(data.CurrentTime.ToString("yyyy-MM-dd"), timeManager.CurrentTime);
        yield return null;
    }

    [UnityTest]
    public IEnumerator TimeManagerEntitySpeedMultiplierTest()
    {
        timeManager.TogglePause();
        Assert.AreEqual(0.0f, timeManager.EntitySpeedMultiplier);
        yield return null;

        timeManager.TogglePause();
        timeManager.SetTimeInterval(TimeInterval.HOUR);
        Assert.AreEqual(Constants.HourSpeedMultiplier, timeManager.EntitySpeedMultiplier);
        yield return null;

        timeManager.SetTimeInterval(TimeInterval.DAY);
        Assert.AreEqual(Constants.DaySpeedMultiplier, timeManager.EntitySpeedMultiplier);
        yield return null;

        timeManager.SetTimeInterval(TimeInterval.WEEK);
        Assert.AreEqual(Constants.WeekSpeedMultiplier, timeManager.EntitySpeedMultiplier);
        yield return null;
    }

    [UnityTest]
    public IEnumerator TimeManagerUpdateTest()
    {
        timeManager.TogglePause();
        float testElapsedTime = 0.0f;
        while (testElapsedTime < 2.0f)
        {
            testElapsedTime += Time.deltaTime;
            yield return null;
        }
        Assert.AreEqual(System.DateTime.Now.ToString("yyyy-MM-dd"), timeManager.CurrentTime);

        timeManager.TogglePause();
        testElapsedTime = 0.0f;
        while (testElapsedTime < 0.5f)
        {
            testElapsedTime += Time.deltaTime;
            yield return null;
        }
        TimeManagerData data = timeManager.SaveData();
        Assert.IsTrue(0.0f < data.ElapsedTime && data.ElapsedTime < 1.0f && data.TickCounter == 0);

        timeManager.SetTimeInterval(TimeInterval.DAY);
        testElapsedTime = 0.0f;
        while (testElapsedTime < 1.0f)
        {
            testElapsedTime += Time.deltaTime;
            yield return null;
        }
        data = timeManager.SaveData();
        Assert.AreEqual(1, data.TickCounter);

        timeManager.SetTimeInterval(TimeInterval.WEEK);
        testElapsedTime = 0.0f;
        bool elapsed = false;
        timeManager.Elapsed += () => elapsed = true;
        while (testElapsedTime < Constants.BaseTimeSpeed / Constants.WeekSpeedMultiplier)
        {
            testElapsedTime += Time.deltaTime;
            yield return null;
        }
        data = timeManager.SaveData();
        Assert.IsTrue(elapsed);
    }
}