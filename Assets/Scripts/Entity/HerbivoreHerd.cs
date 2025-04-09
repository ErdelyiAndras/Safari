
public class HerbivoreHerd : Herd
{
    public HerbivoreHerd(PlacementManager placementManager, AnimalManager parent, HerdType type) : base(placementManager, parent, type) 
    {
        DistributionRadius = Constants.HerbivoreHerdDistributionRadius;
        gameObject.name = "HerbivoreHerd";
    }


}

