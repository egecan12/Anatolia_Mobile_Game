using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectSpawner : MonoBehaviour
{
    public List<GameObject> prefabs; // List of prefabs to spawn
    public float spawnRate = 10f; // Spawn rate in seconds
    public List<Vector3> initialSpawnLocations; // Initial spawn locations for each prefab

    private void Start()
    {
        // Start spawning
        InvokeRepeating(nameof(Spawn), spawnRate, spawnRate);
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
    }
}