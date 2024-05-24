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

        GameObject landscape = GameObject.Find("Landscape");
        // Start spawning
        Spawn();

    }
    private void Update()
    {
        // changeColor();
    }

    private void Spawn()
    {
        GameObject landscape = GameObject.Find("Landscape");

        // Select a random prefab
        int prefabIndex = Random.Range(0, prefabs.Count);

        // Get the initial spawn location for the selected prefab
        Vector3 spawnLocation = initialSpawnLocations[prefabIndex];

        // Instantiate the prefab at the spawner's position as a child of the Landscape GameObject
        GameObject clones = Instantiate(prefabs[prefabIndex], spawnLocation, Quaternion.identity, landscape.transform);
        // Attach the MoveObject script to the new object
        clones.AddComponent<MoveObject>();

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
    // public void changeColor()
    // {
    //     GameObject landscape = GameObject.Find("Landscape");

    //     // Convert the hex color to a Color
    //     Color newColor;
    //     if (ColorUtility.TryParseHtmlString("#261E2E", out newColor))
    //     {
    //         // Change the color of all child sprites
    //         foreach (Transform child in landscape.transform)
    //         {
    //             SpriteRenderer spriteRenderer = child.GetComponent<SpriteRenderer>();
    //             if (spriteRenderer != null)
    //             {
    //                 spriteRenderer.color = newColor;
    //             }
    //         }
    //     }
    //     else
    //     {
    //         Debug.LogError("Invalid color");
    //     }
    // }
}