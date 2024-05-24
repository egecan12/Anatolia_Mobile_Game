using System.Collections.Generic;
using UnityEngine;

public class ObjectSpawner : MonoBehaviour
{
    public List<GameObject> prefabs; // List of prefabs to spawn
    public List<Vector3> initialSpawnLocations; // Initial spawn locations for each prefab
    public float minSpawnRate = 1f; // Minimum spawn rate in seconds
    public float maxSpawnRate = 5f; // Maximum spawn rate in seconds
    public float defaultSpawnRate = 5f; // Maximum spawn rate in seconds


    private void Start()
    {
        // Start spawning
        Spawn();
    }

    private void Spawn()
    {
        // Select a random prefab
        int prefabIndex = Random.Range(0, prefabs.Count);

        // Get the initial spawn location for the selected prefab
        Vector3 spawnLocation = initialSpawnLocations[prefabIndex];

        // Instantiate the prefab at the spawner's position
        GameObject newObj = Instantiate(prefabs[prefabIndex], spawnLocation, Quaternion.identity);

        // Attach the MoveObject script to the new object
        newObj.AddComponent<MoveObject>();

        // Call Spawn again after a random delay
        float spawnRate;
        if (minSpawnRate == 0 || maxSpawnRate == 0)
        {
            spawnRate = defaultSpawnRate; // Default spawn rate
        }
        else
        {
            spawnRate = Random.Range((float)minSpawnRate, (float)maxSpawnRate);
        }
        Invoke(nameof(Spawn), spawnRate);
    }
}