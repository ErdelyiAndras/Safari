using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public enum ObjectType
{
    Jeep,
    Carnivore,
    Herbivore,
    CarnivoreHerd,
    HerbivoreHerd
}
public struct SafariObject
{
    public Guid guid;
    public ObjectType type;
    public IPositionable entity;
    public float Distance(Vector3 target) => Vector3.Distance(target, entity.Position);

    /*public override bool Equals(object obj)
    {
        if (obj is SafariObject other)
        {
            return guid == other.guid;
        }
        return false;
    }

    public override int GetHashCode()
    {
        return guid.GetHashCode();
    }*/
}
public class PlacedObjects
{
    private HashSet<SafariObject> objects = new HashSet<SafariObject>();
    public HashSet<SafariObject> AnimalObjects 
    { 
        get 
        { 
            return new HashSet<SafariObject>(objects.Where(h => h.type != ObjectType.Jeep && 
                                                                h.type != ObjectType.CarnivoreHerd && 
                                                                h.type != ObjectType.HerbivoreHerd)); 
        } 
    }
    //public HashSet<SafariObject> CarnivoreObjects { get { return new HashSet<SafariObject>(objects.Where(h => h.type == ObjectType.Carnivore)); } }
    //public HashSet<SafariObject> HerbivoreObjects { get { return new HashSet<SafariObject>(objects.Where(h => h.type == ObjectType.Herbivore)); } }
    public Herd GetMyHerd(Guid guid)
    {
        return (Herd)objects.FirstOrDefault(h => h.guid == guid /*&& (h.type == ObjectType.CarnivoreHerd || h.type == ObjectType.HerbivoreHerd)*/).entity;
    }
    public IPositionable GetGameObjectWrapper(GameObject obj) => objects.FirstOrDefault(h => h.entity.ObjectInstance == obj).entity;
    public HashSet<HerbivoreHerd> GetHerbivoreHerds() => new HashSet<HerbivoreHerd>(objects.Where(h => h.type == ObjectType.HerbivoreHerd).Select(h => (HerbivoreHerd)h.entity));

    public void AddObject(SafariObject obj)
    {
        if (objects == null)
        {
            objects = new HashSet<SafariObject>();
        }
        objects.Add(obj);
    }

    public void DeleteObject(Guid id)
    {
        objects.RemoveWhere(h => h.guid == id);
    }
}

