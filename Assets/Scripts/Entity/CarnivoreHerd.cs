public class CarnivoreHerd : Herd
{
    public CarnivoreHerd(PlacementManager placementManager, AnimalManager parent, AnimalType type) : base(placementManager, parent, type) 
    {
        DistributionRadius = Constants.CarnivoreHerdDistributionRadius;
        reproductionCoolDown = Constants.CarnivoreHerdReproductionCooldown;
        gameObject.name = "CarnivoreHerd";
    }

    public CarnivoreHerd(HerdData data, PlacementManager placementManager, AnimalManager parent) : base(data, placementManager, parent)
    {
        gameObject.name = "CarnivoreHerd";
        LoadData(data, placementManager);
    }

    public override HerdData SaveData()
    {
        return new CarnivoreHerdData(Id, AnimalTypesOfHerd, animals, centroid, DistributionRadius, reproductionCoolDown);
    }

    public override void LoadData(HerdData data, PlacementManager placementManager)
    {
        base.LoadData(data, placementManager);
        animals = ((CarnivoreHerdData)data).Animals(placementManager, this);
        foreach (Animal animal in animals)
        {
            animal.AnimalDied += a => AnimalDiedHandler(a);
            if (animal is Carnivore1)
            {
                placementManager.RegisterObject(animal.Id, ObjectType.Carnivore, (Carnivore1)animal);
            }
            else if (animal is Carnivore2)
            {
                placementManager.RegisterObject(animal.Id, ObjectType.Carnivore, (Carnivore2)animal);
            }
            else
            {
                throw new System.Exception("Unknown animal type");
            }
        }
    }
}

