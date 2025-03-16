using System.Collections.Generic;
using UnityEngine;

public class AnimalManager : MonoBehaviour
{
    public PlacementManager placementManager;

    public GameObject carnivore1Prefab;
    public GameObject carnivore2Prefab;
    public GameObject herbivore1Prefab;
    public GameObject herbivore2Prefab;
    private int numberOfAnimals = 5;
    public int mapWidth = 50, mapHeight = 50;
    private List<GameObject> spawnedAnimals = new List<GameObject>();

    private void Start()
    {
   
    }

    public void SpawnAnimal(GameObject animalPrefab)
    {
        Vector3 spawnPosition = GetRandomSpawnPosition();
        GameObject newAnimal = Instantiate(animalPrefab, spawnPosition, Quaternion.identity);

        newAnimal.name = $"Animal_{spawnedAnimals.Count + 1}";
        spawnedAnimals.Add(newAnimal);
        Debug.Log($"Spawnolt állat: {newAnimal.name}, Pozíció: {spawnPosition}");
    }
     
    private Vector3 GetRandomSpawnPosition()
    {
        float x = Random.Range(0, mapWidth);
        float z = Random.Range(0, mapHeight);
        return new Vector3(x, 0, z);
    }
}
