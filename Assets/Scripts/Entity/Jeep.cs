using UnityEngine;
using System;
using System.Collections.Generic;
using Unity.Burst.CompilerServices;

public class Jeep : Entity
{
    public enum State
    {
        Waiting,
        Moving,
        Leaving,
        Returning
    }

    private Vector3 endPosition;
    public TouristGroup tourists;

    private List<Vector3Int> jeepPath;
    private int currentPathIndex = 0;

    public State MyState { get; private set; }
    public static Action<Jeep> JeepArrived, JeepWaiting;
    bool hasFullPath = false;
    public Jeep(PlacementManager _placementManager, GameObject prefab, TouristManager parent) : base(_placementManager)
    {
        endPosition = new Vector3Int(placementManager.width - 1, 0, placementManager.height - 1);
        spawnPosition = new Vector3(0, 0, 0);
        MyState = State.Waiting;
        tourists = new TouristGroup();
        tourists.SetDefault();
        tourists.readyToGo += () => MyState = State.Moving;
        SpawnEntity(prefab, parent.transform);
        baseMoveSpeed = Constants.JeepBaseMoveSpeed;
        baseRotationSpeed = Constants.JeepBaseRotationSpeed;
        discoverEnvironment = new JeepSearchInRange(15.0f, placementManager);
    }

    public Jeep(JeepData data, PlacementManager placementManager, TouristManager parent) : base(data, placementManager, parent.placementManager.prefabManager.JeepPrefab, parent.gameObject)
    {
        LoadData(data, placementManager);
    }

    public override void CheckState()
    {
        switch (MyState)
        {
            case State.Waiting:
                JeepWaiting.Invoke(this);
                break;
            case State.Moving:
                if (Position == endPosition)
                {
                    JeepArrived?.Invoke(this);
                    MyState = State.Leaving;                    
                    hasFullPath = false;
                    currentPathIndex = 0;
                }
                else if (!hasFullPath)
                {
                    hasFullPath = placementManager.HasFullPath(Vector3Int.RoundToInt(spawnPosition), Vector3Int.RoundToInt(endPosition));
                    if (hasFullPath)
                    {
                        jeepPath = placementManager.PickRandomRoadPath(Vector3Int.RoundToInt(spawnPosition), Vector3Int.RoundToInt(endPosition));
                    }
                }
                else
                {
                    Move();
                    discoverEnvironment.SearchInViewDistance(Position);
                }
                break;
            case State.Leaving:
                tourists.SetDefault();
                ((JeepSearchInRange)discoverEnvironment).SetDefault();
                MyState = State.Returning;
                break;
            case State.Returning:
                if (Position == spawnPosition)
                {
                    MyState = State.Waiting;
                    hasFullPath = false;
                    currentPathIndex = 0;
                }
                else if (!hasFullPath)
                {
                    hasFullPath = placementManager.HasFullPath(Vector3Int.RoundToInt(spawnPosition), Vector3Int.RoundToInt(endPosition));
                    if (hasFullPath)
                    {
                        jeepPath = placementManager.PickShortestPath(Vector3Int.RoundToInt(endPosition), Vector3Int.RoundToInt(spawnPosition));

                    }
                }
                else
                {
                    Move();
                }
                break;
        }
    }


    protected override void Move()
    {
        Vector3 target = new Vector3(
            jeepPath[currentPathIndex].x,
            0,
            jeepPath[currentPathIndex].z
        );


        Position = Vector3.MoveTowards(
            Position,
            target,
            MoveSpeed * Time.deltaTime
        );

        Vector3 direction = target - Position;
        direction.y = 0;
        if (direction != Vector3.zero)
        {
            Quaternion targetRotation = Quaternion.LookRotation(direction);
            entityInstance.transform.rotation = Quaternion.Slerp(
                entityInstance.transform.rotation,
                targetRotation,
                RotationSpeed * Time.deltaTime

            );
        }

        if (Vector3.Distance(Position, target) < 0.1f && currentPathIndex < jeepPath.Count - 1)
        {
            currentPathIndex++;
        }
    }
    public int CalculateSatisfaction() => ((JeepSearchInRange)discoverEnvironment).AnimalsSeenCount * ((JeepSearchInRange)discoverEnvironment).AnimalTypesSeenCount;

    public override EntityData SaveData()
    {
        return new JeepData(
            Id, spawnPosition, Position, entityInstance.transform.rotation, baseMoveSpeed, baseRotationSpeed,
            (JeepSearchInRange)discoverEnvironment, MyState, endPosition, tourists, jeepPath, currentPathIndex, hasFullPath
        );
    }

    public override void LoadData(EntityData data, PlacementManager placementManager)
    {
        base.LoadData((JeepData)data, placementManager);
        discoverEnvironment = ((JeepData)data).DiscoverEnvironment(placementManager);
        MyState = ((JeepData)data).State;
        endPosition = ((JeepData)data).EndPosition;
        tourists = ((JeepData)data).TouristGroup;
        jeepPath = ((JeepData)data).JeepPath;
        currentPathIndex = ((JeepData)data).CurrentPathIndex;
        hasFullPath = ((JeepData)data).HasFullPath;
        tourists.readyToGo += () => MyState = State.Moving;
    }
}

