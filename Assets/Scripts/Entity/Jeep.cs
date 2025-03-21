using Unity.VisualScripting;
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

    private Vector3 endPosition;
    public TouristGroup tourists;
    public State MyState { get; private set; }
    public static Action<Jeep> JeepArrived, JeepWaiting;
    public Jeep(PlacementManager _placementManager, GameObject prefab)
    {
        placementManager = _placementManager;
        endPosition = new Vector3Int(placementManager.width - 1, 0, placementManager.height - 1);
        spawnPosition = new Vector3(0, 0, 0);
        MyState = State.Waiting;
        tourists = new TouristGroup();
        tourists.readyToGo += () => MyState = State.Moving;
        SpawnEntity(prefab);
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
                else
                {
                    //mozog a jeep
                    CheckForNewAnimals();
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

    public override void Move(Vector3 targetPosition)
    {
        List<Point> roads = placementManager.placementGrid.Roads;
    }

    private void CheckForNewAnimals()
    {
        // ha látunk új állatot akkor a touristgroup metrikáit állítjuk
    }


}

