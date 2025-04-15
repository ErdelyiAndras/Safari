using System;
using UnityEngine;

[System.Serializable]
public abstract class AnimalData : EntityData
{
    [SerializeField]
    private Animal.State myState;
    [SerializeField]
    private AnimalInternalStateData state;
    [SerializeField]
    private Vector3 targetPosition;
    [SerializeField]
    private string myHerd;
    [SerializeField]
    private bool callOnceFlag;
    [SerializeField]
    private bool targetCorrection;
    [SerializeField]
    private float elapsedTime;

    public Animal.State MyState
    {
        get
        {
            return myState;
        }
    }

    public AnimalInternalState State
    {
        get
        {
            return new AnimalInternalState(state);
        }
    }

    public Vector3 TargetPosition
    {
        get
        {
            return targetPosition;
        }
    }

    public Guid MyHerd
    {
        get
        {
            return Guid.Parse(myHerd);
        }
    }

    public bool CallOnceFlag
    {
        get
        {
            return callOnceFlag;
        }
    }

    public bool TargetCorrection
    {
        get
        {
            return targetCorrection;
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
        Guid id, Vector3 spawnPosition, Vector3 position, Quaternion rotation, float baseMoveSpeed, float baseRotationSpeed,
        Animal.State myState, AnimalInternalState state, Vector3 targetPosition, Guid myHerd, bool callOnceFlag, bool targetCorrection, float elapsedTime
    ) : base(id, spawnPosition, position, rotation, baseMoveSpeed, baseRotationSpeed)
    {
        this.myState = myState;
        this.state = state.SaveData();
        this.targetPosition = targetPosition;
        this.myHerd = myHerd.ToString();
        this.callOnceFlag = callOnceFlag;
        this.targetCorrection = targetCorrection;
        this.elapsedTime = elapsedTime;
    }
}
