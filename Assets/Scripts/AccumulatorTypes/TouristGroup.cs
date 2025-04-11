using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public class TouristGroup : ISaveable<TouristGroupData>
{
    public Action readyToGo;
    private int numberOfTourists;

    
    public void AddTourist()
    {
        numberOfTourists++;
        if (IsTouristGroupFull())
        {
            readyToGo?.Invoke();
        }
    }

    public bool IsTouristGroupFull() => numberOfTourists == 4;


    public void SetDefault()
    {
        numberOfTourists = 0;
    }

    public TouristGroupData SaveData()
    {
        return new TouristGroupData(numberOfTourists);
    }
    
    public void LoadData(TouristGroupData data)
    {
        numberOfTourists = data.NumberOfTourists;
    }


}

