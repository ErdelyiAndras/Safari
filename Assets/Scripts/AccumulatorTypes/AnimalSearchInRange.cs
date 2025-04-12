using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UIElements;


public class BFSLists 
{
    public List<Vector3> permanentList;
    public List<Vector3> temporaryList;
}
public abstract class AnimalSearchInRange : SearchInRange
{
    protected List<Vector3> discoveredDrink = new List<Vector3>();
    protected List<Vector3> drinkInRange = new List<Vector3>();

    private readonly float viewExtenderScale;
    protected float GetViewDistance(Vector3 Position) => placementManager.GetTypeOfPosition(placementManager.RoundPosition(Position)) == CellType.Hill ? visionRange * viewExtenderScale : visionRange; 
   
    protected AnimalSearchInRange(float _visionRange, PlacementManager _placementManager, float _viewExtenderScale) : base(_visionRange, _placementManager)
    {
        viewExtenderScale = _viewExtenderScale;
    }

    public List<Vector3> GetDrinkResult() => drinkInRange;

    protected void GeneralBFS(Vector3 Position, Dictionary<CellType, BFSLists> data)
    {
        foreach(var item in data)
        {
            item.Value.temporaryList.Clear();
        }
        Queue<Vector3Int> queue = new Queue<Vector3Int>();
        HashSet<Vector3Int> visited = new HashSet<Vector3Int>();

        Vector3Int startPosition = Vector3Int.RoundToInt(Position);
        queue.Enqueue(startPosition);
        visited.Add(startPosition);

        while (queue.Count > 0)
        {
            Vector3Int current = queue.Dequeue();
            CellType currentCellType = placementManager.GetTypeOfPosition(current);
            if (data.ContainsKey(currentCellType))
            {
                data[currentCellType].temporaryList.Add(current);
                if (!data[currentCellType].permanentList.Contains(current))
                {
                    data[currentCellType].permanentList.Add(current);
                }
            }

            if (Vector3Int.Distance(startPosition, current) <= GetViewDistance(Position))
            {
                foreach (Vector3Int neighbor in GetNeighbors(current))
                {
                    if (!visited.Contains(neighbor))
                    {
                        queue.Enqueue(neighbor);
                        visited.Add(neighbor);
                    }
                }
            }
        }
    }

    private List<Vector3Int> GetNeighbors(Vector3Int position)
    {
        List<Vector3Int> neighbors = new List<Vector3Int>
        {
            position + new Vector3Int(1, 0, 0),
            position + new Vector3Int(-1, 0, 0),
            position + new Vector3Int(0, 0, 1),
            position + new Vector3Int(0, 0, -1),
            position + new Vector3Int(1, 0, 1),
            position + new Vector3Int(1, 0, -1),
            position + new Vector3Int(-1, 0, 1),
            position + new Vector3Int(-1, 0, -1)
        };

        neighbors.RemoveAll(neighbor => !placementManager.CheckIfPositionInBound(neighbor));

        return neighbors;
    }
    public Vector3? GetClosestWater(Vector3 Position, Vector3 herdspawnpoint, int herdradius) => GetClosestTarget(Position, herdspawnpoint, herdradius, drinkInRange, discoveredDrink);
    protected Vector3? GetClosestTarget(Vector3 Position, Vector3 Spawnpoint, int DistributionRadius, List<Vector3> targetInviewDistance, List<Vector3> discoveredTargets)
    {
        Vector3? targetPosition = null;
        if (targetInviewDistance.Count == 1)
        {
            targetPosition = targetInviewDistance[0];
        }
        else if (targetInviewDistance.Count > 1)
        {
            Vector3? closestWithinHerd = null;
            Vector3? closestOutOFHerd = null;
            float closestDistanceInHerd = float.MaxValue;
            float closestDistanceOutOfHerd = float.MaxValue;
            foreach (Vector3 position in targetInviewDistance)
            {
                if (Vector3.Distance(Spawnpoint, position) <= DistributionRadius)
                {
                    float distanceFromPostiion = Vector3.Distance(Position, position);
                    if (distanceFromPostiion < closestDistanceInHerd)
                    {
                        closestWithinHerd = position;
                        closestDistanceInHerd = distanceFromPostiion;
                    }
                }
                else
                {
                    float distanceFromPostiion = Vector3.Distance(Position, position);
                    if (distanceFromPostiion < closestDistanceOutOfHerd)
                    {
                        closestOutOFHerd = position;
                        closestDistanceOutOfHerd = distanceFromPostiion;
                    }
                }
            }
            if (closestWithinHerd != null)
            {
                targetPosition = (Vector3)closestWithinHerd;
            }
            else if (closestOutOFHerd != null)
            {
                targetPosition = (Vector3)closestOutOFHerd;
            }
        }
        else if (discoveredTargets.Count != 0)
        {
            discoveredTargets.Sort((a, b) => Vector3.Distance(Position, a).CompareTo(Vector3.Distance(Position, b)));
            while (discoveredTargets.Count != 0 && Vector3.Distance(Position, discoveredTargets[0]) <= GetViewDistance(Position))
            {
                discoveredTargets.RemoveAt(0);
            }
            if (discoveredTargets.Count != 0)
            {
                Vector3? inHerdRadius = null;
                int i = 0;
                while (inHerdRadius == null && i < discoveredTargets.Count)
                {
                    if (Vector3.Distance(discoveredTargets[i], Spawnpoint) <= DistributionRadius)
                    {
                        inHerdRadius = discoveredTargets[i];
                    }
                    ++i;
                }
                targetPosition = inHerdRadius ?? discoveredTargets[0];
            }
        }
        return targetPosition;
    }
}

