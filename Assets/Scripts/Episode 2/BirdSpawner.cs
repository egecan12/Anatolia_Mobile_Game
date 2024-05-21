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
        float minY = -4.70f; // Adjust these values as needed
        float maxY = 4.70f;

        // Define the x position from where you want to spawn the birds
        float spawnX = -42f; // Adjust this value as needed

        // Generate a random y position within the defined bounds
        float randomY = Random.Range(minY, maxY);

        // Create a new spawn position with the defined x value
        Vector3 randomSpawnPosition = new Vector3(spawnX, randomY, spawnPosition.z);

        GameObject enemyBirdClone = Instantiate(enemyBirdPrefab, randomSpawnPosition, Quaternion.identity);

        //this is very important to attach rigidbody for clones otherwise they are not colliding
        Rigidbody2D rb = enemyBirdClone.GetComponent<Rigidbody2D>();
        if (rb != null)
        {
            rb.simulated = true;
        }
        // Get the EnemyBird component of the clone
        EnemyBird enemyBird = enemyBirdClone.GetComponent<EnemyBird>();
    }

    public void SpawnEnemyAfterDelay()
    {
        // Call the SpawnBird method after a delay
        Invoke("SpawnBird", spawnDelay);
    }
}