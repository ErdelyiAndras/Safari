using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

public abstract class BuildManagerBase : MonoBehaviour
{
    public PlacementManager placementManager;

    protected List<Vector3Int> temporaryPlacementPositions = new List<Vector3Int>();

    abstract public int Cost { get; }

    abstract public void PlaceObject(Vector3Int position);

    abstract public void FinalizeObject(bool result, GameObject type = null);
    
    abstract protected bool CheckPositionBeforePlacement(Vector3Int position);
  


}

