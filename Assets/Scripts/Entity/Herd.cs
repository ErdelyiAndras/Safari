using System;
using System.Collections.Generic;
using System.Diagnostics.Tracing;
using System.Linq;
using UnityEngine;

public abstract class Herd : IPositionable, ISaveable<HerdData>
{
    public Guid Id { get; private set; }
    private AnimalType animalTypesOfHerd; // if mixed herds are allowed this can be a set
    public AnimalType AnimalTypesOfHerd => animalTypesOfHerd;
    protected List<Animal> animals;
    protected Vector2Int Centroid
    {
        get
        {
            Vector2Int sum = Vector2Int.zero;
            foreach (Animal animal in animals)
            {
                sum += new Vector2Int((int)animal.Position.x, (int)animal.Position.z);
            }
            return sum / animals.Count;
        }
        set
        {

        }
    }
    private PlacementManager placementManager;
    public int Count { get { return animals.Count; } }
    public Vector3 Position { get { return animals.Count == 0 ? GetRandomPosition() : new Vector3(Centroid.x, 0, Centroid.y); } }
    public int DistributionRadius { get; protected set; }
    public GameObject ObjectInstance { get; set; } = null;
    public List<Animal> Animals { get { return animals; } }
    public Action<Herd> Reproduce;
    protected int reproductionCoolDown;
    public Action<Herd> animalRemovedFromHerd;

    public Herd(PlacementManager placementManager, AnimalManager parent, AnimalType type)
    {
        ObjectInstance = new GameObject();
        Id = Guid.NewGuid();
        animals = new List<Animal>();
        this.placementManager = placementManager;
        animalTypesOfHerd = type;
        ObjectInstance.transform.SetParent(parent.transform);
        reproductionCoolDown = Constants.ReproductionCooldown[animalTypesOfHerd];
    }

    public Herd(HerdData data, PlacementManager placementManager, AnimalManager parent)
    {
        ObjectInstance = new GameObject();
        ObjectInstance.transform.SetParent(parent.transform);
    }

    public void AddAnimalToHerd(Animal animal)
    {
        animal.AnimalDied += a => AnimalDiedHandler(a);
        animals.Add(animal);
    }

    private void RemoveAnimalFromHerd(Animal animal)
    {
        animals.Remove(animal);
    }

    protected void AnimalDiedHandler(Animal animal)
    {
        RemoveAnimalFromHerd(animal);
        placementManager.PlacedObjects.DeleteObject(animal.Id);
        animalRemovedFromHerd?.Invoke(this);
    }

    public void CheckState()
    {
        for (int i = animals.Count - 1; i >= 0; i--)
        {
            animals[i].CheckState();
        }
    }

    private int GetGrownAnimalCount
    {
        get
        {
            int counter = 0;
            foreach (Animal animal in animals)
            {
                if (animal.state.RemainingLifetime < Constants.MaxLifeTime[animalTypesOfHerd] * Constants.AdultLifetimeThreshold[animalTypesOfHerd])
                {
                    counter++;
                }
            }
            return counter;
        }
    }

    public void AgeAnimals()
    {
        for (int i = animals.Count - 1; i >= 0; i--)
        {
            animals[i].MatureAnimal();
        }
        reproductionCoolDown--;

        if (reproductionCoolDown <= 0 && GetGrownAnimalCount >= 2)
        {
            Reproduce?.Invoke(this);
            reproductionCoolDown = Constants.ReproductionCooldown[animalTypesOfHerd];
        }
    }
    private Vector3 GetRandomPosition()
    {
        int randomX = 0, randomZ = 0;
        do
        {
            randomX = UnityEngine.Random.Range(0, placementManager.Width);
            randomZ = UnityEngine.Random.Range(0, placementManager.Height);
        }
        while (!placementManager.IsPositionWalkable(new Vector3Int(randomX, 0, randomZ)));

        return new Vector3(randomX, 0, randomZ);
    }

    public abstract HerdData SaveData();

    public virtual void LoadData(HerdData data, PlacementManager placementManager)
    {
        this.placementManager = placementManager;
        Id = data.Guid;
        animalTypesOfHerd = data.AnimalTypesOfHerd;
        Centroid = data.Centroid;
        DistributionRadius = data.DistributionRadius;
        reproductionCoolDown = data.ReproductionCoolDown;
    }

    public void ResetData()
    {
        foreach (Animal animal in animals)
        {
            animal.DeleteGameObject();
        }
        if (ObjectInstance != null)
        {
            UnityEngine.Object.Destroy(ObjectInstance);
            ObjectInstance = null;
        }
    }
}

