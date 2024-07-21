using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimalSpawner : MonoBehaviour
{
    public GameObject animalPrefab; // Animal prefab
    public Transform player; // Player's transform
    public int maxAnimals = 5; // Maximum number of animals
    public float despawnDistance = 10f; // Distance at which animals despawn
    public Vector3 spawnRangeMin = new Vector3(-5f, 0f, 10f); // Minimum spawn position
    public Vector3 spawnRangeMax = new Vector3(5f, 0f, 20f); // Maximum spawn position
    public float minSpawnInterval = 2f; // Minimum spawn interval
    public float maxSpawnInterval = 5f; // Maximum spawn interval
    public float minCrossInterval = 3f; // Minimum crossing interval
    public float maxCrossInterval = 7f; // Maximum crossing interval

    private List<GameObject> activeAnimals; // List of active animals
    private Queue<GameObject> animalPool; // Pool of inactive animals

    void Start()
    {
        activeAnimals = new List<GameObject>();
        animalPool = new Queue<GameObject>();
        StartCoroutine(SpawnAnimals());
    }

    void Update()
    {
        // Remove old animals
        for (int i = activeAnimals.Count - 1; i >= 0; i--)
        {
            if (activeAnimals[i].transform.position.z < player.position.z - despawnDistance)
            {
                // Deactivate and pool the animal instead of destroying it
                activeAnimals[i].SetActive(false);
                animalPool.Enqueue(activeAnimals[i]);
                activeAnimals.RemoveAt(i);
            }
        }
    }

    IEnumerator SpawnAnimals()
    {
        while (true)
        {
            if (activeAnimals.Count < maxAnimals)
            {
                SpawnAnimal();
            }
            yield return new WaitForSeconds(Random.Range(minSpawnInterval, maxSpawnInterval));
        }
    }

    private void SpawnAnimal()
    {
        Vector3 spawnPosition = new Vector3(Random.Range(spawnRangeMin.x, spawnRangeMax.x), 0, Random.Range(spawnRangeMin.z, spawnRangeMax.z) + player.position.z);
        GameObject animal;

        // Get an animal from the pool if available, otherwise create a new one
        if (animalPool.Count > 0)
        {
            animal = animalPool.Dequeue();
            animal.transform.position = spawnPosition;
            animal.SetActive(true);
        }
        else
        {
            animal = Instantiate(animalPrefab, spawnPosition, Quaternion.identity);
        }

        activeAnimals.Add(animal);

        // Start the crossing coroutine for the animal
        StartCoroutine(AnimalCrossing(animal));
    }

    IEnumerator AnimalCrossing(GameObject animal)
    {
        while (animal.activeSelf)
        {
            yield return new WaitForSeconds(Random.Range(minCrossInterval, maxCrossInterval)); // Wait for a random duration

            // Target position for the animal to cross to
            Vector3 targetPosition = new Vector3(-animal.transform.position.x, animal.transform.position.y, animal.transform.position.z);

            // Rotate animal to face the direction it is moving
            if (targetPosition.x < animal.transform.position.x)
            {
                animal.transform.rotation = Quaternion.Euler(0, 180, 0);
            }
            else
            {
                animal.transform.rotation = Quaternion.Euler(0, 0, 0);
            }

            while (Vector3.Distance(animal.transform.position, targetPosition) > 0.1f)
            {
                animal.transform.position = Vector3.MoveTowards(animal.transform.position, targetPosition, Time.deltaTime * 5f);
                yield return null;
            }

            // Wait for a random duration before crossing back
            yield return new WaitForSeconds(Random.Range(minCrossInterval, maxCrossInterval));
            targetPosition = new Vector3(-animal.transform.position.x, animal.transform.position.y, animal.transform.position.z);

            // Rotate animal to face the direction it is moving
            if (targetPosition.x < animal.transform.position.x)
            {
                animal.transform.rotation = Quaternion.Euler(0, 90, 0);
            }
            else
            {
                animal.transform.rotation = Quaternion.Euler(0, 0, 0);
            }

            //while (Vector3.Distance(animal.transform.position, targetPosition) > 0.1f)
            //{
            //    animal.transform.position = Vector3.MoveTowards(animal.transform.position, targetPosition, Time.deltaTime * 5f);
            //    yield return null;
            //}
        }
    }
}
