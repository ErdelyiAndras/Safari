using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;


public class AnimalManager : MonoBehaviour
{
    public PlacementManager placementManager;

    public GameObject carnivore1Prefab, carnivore2Prefab, herbivore1Prefab, herbivore2Prefab;
    private int numberOfAnimals = 5;
    private List<GameObject> spawnedAnimals = new List<GameObject>();
    //le tudjam kérdezni minden állat pozícióját és típusát, lehetőleg egy foreach (var in spawnedAnimals) -al

    public List<GameObject> Animals => spawnedAnimals;
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
        float x = Random.Range(0, placementManager.width);
        float z = Random.Range(0, placementManager.height);
        return new Vector3(x, 0, z);
    }
}
