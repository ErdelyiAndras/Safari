using Unity.VisualScripting;
using UnityEngine;
using System;
using System.Collections.Generic;
using TMPro;
using System.IO;
using NUnit;

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

    public float rotationSpeed = 5.0f;
    private Vector3 targetPosition;

    private List<Vector3Int> jeepPath;
    private int currentPathIndex = 0;

    public State MyState { get; private set; }
    public static Action<Jeep> JeepArrived, JeepWaiting;
    bool hasFullPath;
    public Jeep(PlacementManager _placementManager, GameObject prefab, TouristManager parent)
    {
        placementManager = _placementManager;
        endPosition = new Vector3Int(placementManager.width - 1, 0, placementManager.height - 1);
        spawnPosition = new Vector3(0, 0, 0);
        //MyState = State.Moving;
        MyState = State.Waiting; // Ez legyen State.Moving helyett, ha már lesznek túristák
        tourists = new TouristGroup();
        tourists.readyToGo += () => MyState = State.Moving; // Ez kell majd ha lesznek túristák
        SpawnEntity(prefab, parent.transform);
        baseMoveSpeed = 1.0f; // DEFAULT ÉRTÉK?!
        SpeedMultiplier = 1.0f; // EZT KELL ÁLLÍTANI
        hasFullPath = false;
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
                    //CheckForNewAnimals();
                }
                break;
            case State.Leaving:
                tourists.SetDefault();
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
                rotationSpeed * Time.deltaTime
            );
        }

        if (Vector3.Distance(Position, target) < 0.1f && currentPathIndex < jeepPath.Count - 1)
        {
            currentPathIndex++;
        }
    }

    private void CheckForNewAnimals()
    {
        // ha látunk új állatot akkor a touristgroup metrikáit állítjuk
    }

}

