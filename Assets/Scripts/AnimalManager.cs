using System.Collections.Generic;
using UnityEngine;

public class AnimalManager : MonoBehaviour
{
    public GameObject animalPrefab;
    private int numberOfAnimals = 5;
    private List<GameObject> spawnedAnimals = new List<GameObject>(); // Tároljuk a példányokat

    private void Start()
    {
        SpawnAnimals();
    }

    private void SpawnAnimals()
    {
        for (int i = 0; i < numberOfAnimals; i++)
        {

            int x = Random.Range(0, 50);
            int y = Random.Range(0, 50);

            Vector3 spawnPosition = new Vector3(x, 0, y);
            GameObject newAnimal = Instantiate(animalPrefab, spawnPosition, Quaternion.identity);
            newAnimal.name = $"Animal_{i + 1}";
            spawnedAnimals.Add(newAnimal);
        }
    }
}