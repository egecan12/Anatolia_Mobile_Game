using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BirdSpawner : MonoBehaviour
{
    public GameObject enemyBirdPrefab;
    public Vector3 spawnPosition;
    public float spawnDelay = 5f; // Delay in seconds

    public void SpawnBird()
    {
        // Define the scene bounds
        float minY = 0.5f; // Adjust these values as needed
        float maxY = 8.0f;

        // Generate a random y position within the defined bounds
        float randomY = Random.Range(minY, maxY);

        // Create a new spawn position with the random x value
        Vector3 randomSpawnPosition = new Vector3(spawnPosition.x, randomY, spawnPosition.z);

        GameObject enemyBirdClone = Instantiate(enemyBirdPrefab, randomSpawnPosition, Quaternion.identity);
        // Get the EnemyBird component of the clone
        EnemyBird enemyBird = enemyBirdClone.GetComponent<EnemyBird>();
    }

    public void SpawnEnemyAfterDelay()
    {
        // Call the SpawnBird method after a delay
        Invoke("SpawnBird", spawnDelay);
    }
}