using NUnit.Framework;

public class TouristGroupTests
{
    [Test]
    public void TouristGroupConstructorTest()
    {
        TouristGroup group = new TouristGroup();
        Assert.IsFalse(group.IsTouristGroupFull());
    }

    [Test]
    public void TouristGroupLoadConstructorTest()
    {
        TouristGroupData data = new TouristGroupData(2);
        TouristGroup group = new TouristGroup(data);
        Assert.IsFalse(group.IsTouristGroupFull());
    }

    [Test]
    public void TouristGroupAddTouristTest()
    {
        TouristGroup group = new TouristGroup();
        group.AddTourist();
        Assert.IsFalse(group.IsTouristGroupFull());
    }

    [Test]
    public void TouristGroupIsTouristGroupFullTest()
    {
        TouristGroup group = new TouristGroup();
        for (int i = 0; i < 4; i++)
        {
            group.AddTourist();
        }
        Assert.IsTrue(group.IsTouristGroupFull());
    }

    [Test]
    public void TouristGroupReadyToGoTest()
    {
        TouristGroup group = new TouristGroup();
        bool isReadyToGo = false;
        group.readyToGo += () => isReadyToGo = true;
        for (int i = 0; i < 3; i++)
        {
            group.AddTourist();
            Assert.IsFalse(isReadyToGo);
        }
        group.AddTourist();
        Assert.IsTrue(isReadyToGo);
    }

    [Test]
    public void TouristGroupSetDefaultFromEmptyTest()
    {
        TouristGroup group = new TouristGroup();
        group.SetDefault();
        Assert.IsFalse(group.IsTouristGroupFull());
    }

    [Test]
    public void TouristGroupSetDefaultFromNotFullTest()
    {
        TouristGroup group = new TouristGroup();
        group.AddTourist();
        group.AddTourist();
        group.SetDefault();
        Assert.IsFalse(group.IsTouristGroupFull());
    }

    [Test]
    public void TouristGroupSetDefaultFromFullTest()
    {
        TouristGroup group = new TouristGroup();
        for (int i = 0; i < 4; i++)
        {
            group.AddTourist();
        }
        group.SetDefault();
        Assert.IsFalse(group.IsTouristGroupFull());
    }

    [Test]
    public void TouristGroupSaveDataTest()
    {
        TouristGroup group = new TouristGroup();
        for (int i = 0; i < 3; i++)
        {
            group.AddTourist();
        }
        TouristGroupData data = group.SaveData();
        Assert.AreEqual(3, data.NumberOfTourists);
    }

    [Test]
    public void TouristGroupLoadDataTest()
    {
        TouristGroup group = new TouristGroup();
        TouristGroupData data = new TouristGroupData(2);
        group.LoadData(data);
        Assert.IsFalse(group.IsTouristGroupFull());
    }
}