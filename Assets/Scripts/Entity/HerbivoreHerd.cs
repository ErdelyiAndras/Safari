public class HerbivoreHerd : Herd
{
    public HerbivoreHerd(PlacementManager placementManager, AnimalManager parent, AnimalType type) : base(placementManager, parent, type) 
    {
        DistributionRadius = Constants.HerbivoreHerdDistributionRadius;
        gameObject.name = "HerbivoreHerd";
    }

    public HerbivoreHerd(HerdData data, PlacementManager placementManager) : base(data, placementManager)
    {
        gameObject.name = "HerbivoreHerd";
    }
}

