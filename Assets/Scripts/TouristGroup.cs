using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public class TouristGroup
{
    public Action readyToGo;
    private int numberOfTourists;
    private int animalsSeen;
    private int animalTypesSeen;

    
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
        animalsSeen = 0;
        animalTypesSeen = 0;
    }
    public int CalculateSatisfaction()
    {
        return animalsSeen * animalTypesSeen;
    }
}

