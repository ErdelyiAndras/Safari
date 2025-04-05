
public class HerbivoreHerd : Herd
{
    public HerbivoreHerd(PlacementManager placementManager, AnimalManager parent, HerdType type) : base(placementManager, parent, type) 
    {
        DistributionRadius = 2;
        gameObject.name = "HerbivoreHerd";
    }


}

