using UnityEngine;

[System.Serializable]
public class AnimalInternalStateData
{
    [SerializeField]
    private AnimalType type;
    [SerializeField]
    private float remainingLifetime;
    [SerializeField]
    private float hunger;
    [SerializeField]
    private float thirst;
    [SerializeField]
    private float health;

    public AnimalType Type
    {
        get
        {
            return type;
        }
    }

    public float RemainingLifetime
    {
        get
        {
            return remainingLifetime;
        }
    }

    public float Hunger
    {
        get
        {
            return hunger;
        }
    }

    public float Thirst
    {
        get
        {
            return thirst;
        }
    }

    public float Health
    {
        get
        {
            return health;
        }
    }

    public AnimalInternalStateData(AnimalType type, float remainingLifetime, float hunger, float thirst, float health)
    {
        this.type = type;
        this.remainingLifetime = remainingLifetime;
        this.hunger = hunger;
        this.thirst = thirst;
        this.health = health;
    }
}