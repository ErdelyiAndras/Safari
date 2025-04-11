using UnityEngine;

[System.Serializable]
public class TouristGroupData
{
    [SerializeField]
    private int numberOfTourists;

    public int NumberOfTourists
    {
        get
        {
            return numberOfTourists;
        }
    }

    public TouristGroupData(int numberOfTourists)
    {
        this.numberOfTourists = numberOfTourists;
    }
}