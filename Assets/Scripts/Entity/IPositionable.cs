using UnityEngine;

public interface IPositionable
{   
    public Vector3 Position { get; }
    public GameObject ObjectInstance { get; set; }

}