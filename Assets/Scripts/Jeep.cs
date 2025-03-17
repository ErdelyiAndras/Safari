using Unity.VisualScripting;
using UnityEngine;
using System;

public enum State
{
    Waiting,
    Moving,
    Leaving,
    Returning
}
public class Jeep : Entity
{
    private Vector3Int endPosition;
    private Vector3Int position;
    public TouristGroup tourists;
    public State MyState { get; private set; }
    public static Action<Jeep> JeepArrived, JeepWaiting;
    public Jeep(int width, int height)
    {
        endPosition = new Vector3Int(width - 1, 0, height - 1);
        position = new Vector3Int(0, 0, 0);
        MyState = State.Waiting;
        tourists = new TouristGroup();
        tourists.readyToGo += () => MyState = State.Moving;
    }
    
    private void Update()
    {
        CheckJeepState();
    }

    private void CheckJeepState()
    {
        switch (MyState)
        {
            case State.Waiting:
                JeepWaiting.Invoke(this);
                break;
            case State.Moving:
                if (position == endPosition)
                {
                    MyState = State.Leaving;
                }
                else
                {
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
    }

    public override void Move(Vector3 targetPosition)
    {
    }

    private void CheckForNewAnimals()
    {
        // ha látunk új állatot akkor a touristgroup metrikáit állítjuk
    }

}

