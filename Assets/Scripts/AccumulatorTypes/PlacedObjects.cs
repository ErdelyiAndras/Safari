using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public enum ObjectType
{
    Jeep,
    Carnivore,
    Herbivore
}
public struct SafariObject
{
    public Guid guid;
    public ObjectType type;
    public Entity entity;
    public float Distance(Vector3 target) => Vector3.Distance(target, entity.Position);
}
public class PlacedObjects
{
    private HashSet<SafariObject> objects;
    public HashSet<SafariObject> AnimalObjects { get { return new HashSet<SafariObject>(objects.Where(h => h.type != ObjectType.Jeep)); } }
    public HashSet<SafariObject> CarnivoreObjects { get { return new HashSet<SafariObject>(objects.Where(h => h.type == ObjectType.Carnivore)); } }
    public HashSet<SafariObject> HerbivoreObjects { get { return new HashSet<SafariObject>(objects.Where(h => h.type == ObjectType.Herbivore)); } }

    public void AddObject(SafariObject obj)
    {
        if (objects == null)
        {
            objects = new HashSet<SafariObject>();
        }
        objects.Add(obj);
    }

    public void DeleteObject(Entity entity)
    {
        objects.RemoveWhere(h => h.guid == entity.Id);
    }
}

