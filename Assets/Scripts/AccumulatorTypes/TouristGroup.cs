using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public class TouristGroup
{
    public Action readyToGo;
    private int numberOfTourists;
    private HashSet<Guid> animalsSeen;
    private HashSet<AnimalType> animalTypesSeen;

    public void AddSeenAnimal(Animal animal)
    {
        animalsSeen.Add(animal.Id);
        animalTypesSeen.Add(animal.Type);
    }
    
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
        animalsSeen = new HashSet<Guid>();
        animalTypesSeen = new HashSet<AnimalType>();
    }
    public int CalculateSatisfaction()
    {
        return animalsSeen.Count * animalTypesSeen.Count;
    }
}

