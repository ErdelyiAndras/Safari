using UnityEngine;
using System;
using System.Collections.Generic;

public class Jeep : Entity
{
    public enum State
    {
        Waiting,
        Moving,
        Leaving,
        Returning
    }

    public TouristGroup tourists;
    private List<Vector3Int> jeepPath = null;
    private int currentPathIndex = 0;

    public State MyState { get; private set; }
    public static Action<Jeep> JeepArrived, JeepWaiting, AcquireAdmissionFee;
    public int AdmissionFee { get; set; }
    public Jeep(PlacementManager _placementManager, GameObject prefab, TouristManager parent) : base(_placementManager)
    {
        spawnPosition = PlacementManager.startPosition;
        MyState = State.Waiting;
        tourists = new TouristGroup();
        tourists.SetDefault();
        tourists.readyToGo += () => { AcquireAdmissionFee?.Invoke(this); MyState = State.Moving; };
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
                if (Position == PlacementManager.endPosition)
                {
                    JeepArrived?.Invoke(this);
                    MyState = State.Leaving;
                    jeepPath = null;
                    currentPathIndex = 0;
                }
                else if (placementManager.HasFullPathProperty && jeepPath == null)
                {
                    jeepPath = placementManager.PickRandomRoadPath(Vector3Int.RoundToInt(spawnPosition), Vector3Int.RoundToInt(PlacementManager.endPosition));
                    Move();
                    discoverEnvironment.SearchInViewDistance(Position);
                }
                else if (jeepPath != null)
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
                    jeepPath = null;
                    currentPathIndex = 0;
                }
                else if (placementManager.HasFullPathProperty && jeepPath == null)
                {
                    jeepPath = placementManager.PickShortestPath(Vector3Int.RoundToInt(PlacementManager.endPosition), Vector3Int.RoundToInt(spawnPosition));
                    Move();
                }
                else if (jeepPath != null)
                {
                    Move();
                }
                break;
        }
    }


    protected override void Move()
    {
        Vector3 target = new Vector3(jeepPath[currentPathIndex].x, 0, jeepPath[currentPathIndex].z);
        
        Position = Vector3.MoveTowards(Position, target, MoveSpeed * Time.deltaTime);

        Vector3 direction = target - Position;
        direction.y = 0;
        if (direction != Vector3.zero)
        {
            Quaternion targetRotation = Quaternion.LookRotation(direction);
            ObjectInstance.transform.rotation = Quaternion.Slerp(
                ObjectInstance.transform.rotation,
                targetRotation,
                RotationSpeed * Time.deltaTime
            );
        }

        if (Vector3.Distance(Position, target) < 0.1f && currentPathIndex < jeepPath.Count - 1)
        {
            ++currentPathIndex;
        }
    }
    public float CalculateSatisfaction() => Math.Clamp(((JeepSearchInRange)discoverEnvironment).AnimalsSeenCount * ((JeepSearchInRange)discoverEnvironment).AnimalTypesSeenCount, 0.0f, 100.0f) - (AdmissionFee / 10.0f);

    public override EntityData SaveData()
    {
        return new JeepData(
            Id, spawnPosition, Position, ObjectInstance.transform.rotation, baseMoveSpeed, baseRotationSpeed,
            (JeepSearchInRange)discoverEnvironment, MyState, PlacementManager.endPosition, tourists, jeepPath, currentPathIndex, AdmissionFee
        );
    }

    public override void LoadData(EntityData data, PlacementManager placementManager)
    {
        base.LoadData((JeepData)data, placementManager);
        discoverEnvironment = ((JeepData)data).DiscoverEnvironment(placementManager);
        MyState = ((JeepData)data).State;
        tourists = ((JeepData)data).TouristGroup;
        jeepPath = ((JeepData)data).JeepPath;
        currentPathIndex = ((JeepData)data).CurrentPathIndex;
        tourists.readyToGo += () => MyState = State.Moving;
    }
}

