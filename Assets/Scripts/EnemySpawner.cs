using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    public GameObject enemyHomelessPrefab;
    public Vector3 spawnPosition;
    public float spawnDelay = 5f; // Delay in seconds
    public GameObject coinPrefab; // Add this line


    public void SpawnEnemy()
    {
        // Get the player's position
        GameObject player = GameObject.FindWithTag("Player");
        if (player != null)
        {
            Vector3 playerPosition = player.transform.position;

            // Define the scene bounds
            float minX = -10.0f; // Adjust these values as needed
            float maxX = 25.0f;

            // Calculate the furthest position from the player
            Vector3 spawnPosition = playerPosition.x > 0 ? new Vector3(minX, -3.5f, 0) : new Vector3(maxX - 5, -3.5f, 0);


            GameObject enemyHomelessClone = Instantiate(enemyHomelessPrefab, spawnPosition, Quaternion.identity);
            // Get the EnemyHomeless component of the clone
            EnemyHomeless enemyHomeless = enemyHomelessClone.GetComponent<EnemyHomeless>();
            if (enemyHomeless != null)
            {
                enemyHomeless.coinPrefab = coinPrefab;
            }
        }
    }

    public void SpawnEnemyAfterDelay()
    {
        // Call the SpawnEnemy method after a delay
        Invoke("SpawnEnemy", spawnDelay);
    }
}