public class CarnivoreHerd : Herd
{
    public CarnivoreHerd(PlacementManager placementManager, AnimalManager parent, AnimalType type) : base(placementManager, parent, type) 
    {
        DistributionRadius = Constants.CarnivoreHerdDistributionRadius;
        gameObject.name = "CarnivoreHerd";
    }

    public CarnivoreHerd(HerdData data, PlacementManager placementManager) : base(data, placementManager)
    {
        gameObject.name = "CarnivoreHerd";
    }


}

