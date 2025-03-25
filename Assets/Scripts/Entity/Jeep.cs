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
    public Jeep(PlacementManager _placementManager, GameObject prefab)
    {
        placementManager = _placementManager;
        endPosition = new Vector3Int(placementManager.width - 1, 0, placementManager.height - 1);
        spawnPosition = new Vector3(0, 0, 0);
        MyState = State.Moving;
        //MyState = State.Waiting; // MAJD AKKOR KELL HA LESZNEK TURISTAK
        tourists = new TouristGroup();
        //tourists.readyToGo += () => MyState = State.Moving; // MAJD AKKOR KELL HA LESZNEK TURISTAK
        SpawnEntity(prefab);
        baseMoveSpeed = 1.0f; // DEFAULT ÉRTÉK?!
        SpeedMultiplier = 1.0f; // EZT KELL ÁLLÍTANI
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
                    MyState = State.Leaving;
                }
                else if (placementManager.HasFullPath(Vector3Int.RoundToInt(spawnPosition), Vector3Int.RoundToInt(endPosition)))
                {
                    Move();
                    //CheckForNewAnimals();
                }
                break;
            case State.Leaving:
                JeepArrived?.Invoke(this);
                break;
            case State.Returning:
                break;
        }
    }
    public void Return()
    {
        MyState = State.Returning;
        tourists.SetDefault();
        // visszamegy a kezdőpozícióba (0,0,0)
    }

    public override void Move()
    {
        if (jeepPath == null)
        {
            jeepPath = placementManager.PickRandomRoadPath(
                Vector3Int.RoundToInt(spawnPosition),
                Vector3Int.RoundToInt(endPosition)
            );

            if (jeepPath?.Count > 1)
            {
                currentPathIndex = 0;
            }
            else
            {
                return;
            }
        }

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

        if (Vector3.Distance(Position, target) < 0.1f && target != endPosition)
        {
            currentPathIndex++;
        }
    }

    private void CheckForNewAnimals()
    {
        // ha látunk új állatot akkor a touristgroup metrikáit állítjuk
    }


}

