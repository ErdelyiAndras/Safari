using System;
using UnityEngine;

[System.Serializable]
public abstract class AnimalData : EntityData
{
    [SerializeField]
    private Animal.State state;
    [SerializeField]
    private AnimalInternalStateData internalState;
    [SerializeField]
    private Vector3 targetPosition;
    [SerializeField]
    private bool callOnceFlag;
    [SerializeField]
    private float elapsedTime;

    public Animal.State MyState
    {
        get
        {
            return state;
        }
    }

    public AnimalInternalState State
    {
        get
        {
            return new AnimalInternalState(internalState);
        }
    }

    public Vector3 TargetPosition
    {
        get
        {
            return targetPosition;
        }
    }

    public bool CallOnceFlag
    {
        get
        {
            return callOnceFlag;
        }
    }

    public float ElapsedTime
    {
        get
        {
            return elapsedTime;
        }
    }

    public AnimalData(
        Guid id, Vector3 spawnPosition, Vector3 position, Quaternion rotation,
        Animal.State state, AnimalInternalState internalState, Vector3 targetPosition, Herd herd, bool callOnceFlag, float elapsedTime
    ) : base(id, spawnPosition, position, rotation)
    {
        this.state = state;
        this.internalState = internalState.SaveData();
        this.targetPosition = targetPosition;
        //this.herd = herd.SaveData();
        this.callOnceFlag = callOnceFlag;
        this.elapsedTime = elapsedTime;
    }
}
