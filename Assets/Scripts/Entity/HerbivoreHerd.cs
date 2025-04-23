public class HerbivoreHerd : Herd
{
    public HerbivoreHerd(PlacementManager placementManager, AnimalManager parent, AnimalType type) : base(placementManager, parent, type) 
    {
        DistributionRadius = Constants.HerbivoreHerdDistributionRadius;
        ObjectInstance.name = "HerbivoreHerd";
    }

    public HerbivoreHerd(HerdData data, PlacementManager placementManager, AnimalManager parent) : base(data, placementManager, parent)
    {
        ObjectInstance.name = "HerbivoreHerd";
        LoadData(data, placementManager);
    }

    public override HerdData SaveData()
    {
        return new HerbivoreHerdData(Id, AnimalTypesOfHerd, animals, centroid, DistributionRadius, reproductionCoolDown);
    }

    public override void LoadData(HerdData data, PlacementManager placementManager)
    {
        base.LoadData(data, placementManager);
        animals = ((HerbivoreHerdData)data).Animals(placementManager, this);
        foreach (Animal animal in animals)
        {
            animal.AnimalDied += a => AnimalDiedHandler(a);
            if (animal is Herbivore1)
            {
                placementManager.RegisterObject(animal.Id, ObjectType.Herbivore, (Herbivore1)animal);
            }
            else if (animal is Herbivore2)
            {
                placementManager.RegisterObject(animal.Id, ObjectType.Herbivore, (Herbivore2)animal);
            }
            else
            {
                throw new System.Exception("Unknown animal type");
            }
        }
    }
}

