using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
public class BirdSpawner : MonoBehaviour
{
    public GameObject enemyBirdPrefab;
    public GameObject giantBirdPrefab;
    public Vector3 spawnPosition;
    public float spawnDelay = 5f; // Delay in seconds

    // Add a static integer to count the spawned birds
    public static int birdCount = 0;
    private TMP_Text threeHoursText;
    private Image threeHoursPanel;

    // Add a boolean to track whether the giant bird has been spawned
    private int giantBirdCount = 0;
    // Find the text with the tag "3hourstext"

    void Start()
    {
        // Find the text with the tag "3hourstext"
        threeHoursText = GameObject.FindGameObjectWithTag("3HoursText").GetComponent<TMP_Text>();
        threeHoursPanel = GameObject.FindGameObjectWithTag("3HoursPanel").GetComponent<Image>();
        // Get the current color of the panel
        Color panelColor = threeHoursPanel.color;

        // Make the text invisible by setting the alpha to 0
        threeHoursText.alpha = 0f;
        // Set the alpha of the color to 0
        panelColor.a = 0f;

        // Apply the new color to the panel
        threeHoursPanel.color = panelColor;

    }
    public void SpawnBird()
    {

        // If bird count is more than 5 and the giant bird has not been spawned, spawn the giant bird
        if (birdCount > 3)
        {
            if (giantBirdCount <= 0)
            {
                StartCoroutine(ShowTextAndSpawnGiantBird());

            }
            else
            {
                return;
            }
        }
        else  // Only spawn small birds if the giant bird has not been spawned
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

            // Increment the bird count
            birdCount++;
        }
    }

    public void SpawnEnemyAfterDelay()
    {
        // Call the SpawnBird method after a delay
        Invoke("SpawnBird", spawnDelay);
    }
    IEnumerator ShowTextAndSpawnGiantBird()
    {

        // Make the text visible by setting the alpha of the color to 1
        Color color = threeHoursText.color;

        color.a = 1f;
        threeHoursText.color = color;

        // Get the current color of the panel
        Color panelColor = threeHoursPanel.color;
        panelColor.a = 1f;
        threeHoursPanel.color = panelColor; // Apply the new color back to the panel

        Time.timeScale = 0; // Pause the game
        yield return new WaitForSecondsRealtime(4f); // Wait for 10 seconds in real time
        Time.timeScale = 1; // Resume the game

        color.a = 0f;
        threeHoursText.color = color;
        panelColor.a = 0f;
        threeHoursPanel.color = panelColor; // Apply the new color back to the panel

        // Define the spawn position for the giant bird
        Vector3 giantBirdSpawnPosition = new Vector3(-30f, 0f, 0f); // Adjust these values as needed
        if (giantBirdCount <= 0)
        {
            // Instantiate the giant bird prefab at the defined spawn position
            Instantiate(giantBirdPrefab, giantBirdSpawnPosition, Quaternion.identity);
            giantBirdCount = giantBirdCount + 1;
        }
        else
        {
            yield break;
        }
    }
}