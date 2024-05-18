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
        GameObject enemyHomelessClone = Instantiate(enemyHomelessPrefab, spawnPosition, Quaternion.identity);
        // Get the EnemyHomeless component of the clone
        EnemyHomeless enemyHomeless = enemyHomelessClone.GetComponent<EnemyHomeless>();

        if (enemyHomeless != null)
        {
            enemyHomeless.coinPrefab = coinPrefab;
        }
    }

    public void SpawnEnemyAfterDelay()
    {
        // Call the SpawnEnemy method after a delay
        Invoke("SpawnEnemy", spawnDelay);
    }
}