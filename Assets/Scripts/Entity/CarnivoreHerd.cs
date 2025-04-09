
public class CarnivoreHerd : Herd
{
    public CarnivoreHerd(PlacementManager placementManager, AnimalManager parent, HerdType type) : base(placementManager, parent, type) 
    {
        DistributionRadius = Constants.CarnivoreHerdDistributionRadius;
        gameObject.name = "CarnivoreHerd";

    }


}

