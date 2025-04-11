using UnityEngine;

// TODO: this is an entirely dummy class, it should be replaced with a real one

[System.Serializable]
public class PlacementManagerData
{
    [SerializeField]
    private int placementManager;

    public int PlacementManager
    {
        get
        {
            return placementManager;
        }
    }

    public PlacementManagerData(int placementManager)
    {
        this.placementManager = placementManager;
    }
}
