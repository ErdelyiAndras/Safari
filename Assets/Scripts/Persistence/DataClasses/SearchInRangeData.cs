using UnityEngine;

// TODO: this is an entirely dummy class, it should be replaced with a real one

[System.Serializable]
public class SearchInRangeData
{
    [SerializeField]
    private int searchInRange;

    public int SearchInRange
    {
        get
        {
            return searchInRange;
        }
    }

    public SearchInRangeData(int searchInRange)
    {
        this.searchInRange = searchInRange;
    }
}
