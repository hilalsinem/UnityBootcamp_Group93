using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class AICarSpawner : MonoBehaviour
{
    [SerializeField]
    GameObject[] carPrefabs;

    [SerializeField]
    GameObject[] spawnPoints;

    GameObject[] carAIPool = new GameObject[20];
    Transform playerCarTransform;

    float timelastcarspawned = 0;
    WaitForSeconds wait = new WaitForSeconds(0.5f);

    void Start()
    {
        int prefabIndex = 0;
        for (int i = 0; i < carAIPool.Length; i++)
        {
            carAIPool[i] = Instantiate(carPrefabs[prefabIndex]);
            carAIPool[i].SetActive(true);

            prefabIndex++;

            if (prefabIndex == carPrefabs.Length - 1)
            {
                prefabIndex = 0;
            }
        }

        StartCoroutine(UpdateLessOftenCO());
    }

    IEnumerator UpdateLessOftenCO()
    {
        while (true)
        {
            SpawnNewCars();
            yield return wait;
        }
    }

    void SpawnNewCars()
    {
        if (Time.time - timelastcarspawned < 2)
            return;

        GameObject carToSpawn = null;

        foreach (GameObject aiCar in carAIPool)
        {
            if (aiCar.activeInHierarchy)
                continue;

            carToSpawn = aiCar;
            break;
        }

        if (carToSpawn == null)
            return;

        Vector3 spawnPosition = new Vector3(0, 0, playerCarTransform.transform.position.z + 100);
        carToSpawn.transform.position = spawnPosition;
        carToSpawn.SetActive(true);

        timelastcarspawned = Time.time;
    }

    void CleanUpCarsBeyondView()
    {
        foreach (GameObject aiCar in carAIPool)
        {
            if (!aiCar.activeInHierarchy)
                continue;

            if (aiCar.transform.position.z - playerCarTransform.position.z > 200)
            {
                aiCar.SetActive(false);
            }

            if (aiCar.transform.position.z - playerCarTransform.position.z < -50)
            {
                aiCar.SetActive(false);
            }
        }
    }
}
