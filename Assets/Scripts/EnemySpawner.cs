using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    public GameObject enemyHomelessPrefab;
    public Vector3 spawnPosition;
    public float spawnDelay = 5f; // Delay in seconds

    public void SpawnEnemy()
    {
        // Instantiate a new EnemyHomeless prefab at the specified position
        Instantiate(enemyHomelessPrefab, spawnPosition, Quaternion.identity);
    }

    public void SpawnEnemyAfterDelay()
    {
        // Call the SpawnEnemy method after a delay
        Invoke("SpawnEnemy", spawnDelay);
    }
}