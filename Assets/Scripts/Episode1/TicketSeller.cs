using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TicketSeller : MonoBehaviour
{
    public Transform playerTransform; // The player's transform
    public Transform ticketSellerTransform; // The ticket seller's transform
    public float moveDuration = 10f; // The duration of the move
    public Transform playerBalloonTransform; // The player balloon's transform

    private SpriteRenderer spriteRenderer;
    private GameObject askGoldCloud;
    // Start is called before the first frame update
    public Player player; // Reference to the Player script
    public float additionalHeight = 10f; // The additional height they should rise
    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        askGoldCloud = GameObject.FindGameObjectWithTag("askGoldCloud"); // Initialize askGoldCloud here
        if (askGoldCloud != null)
        {
            askGoldCloud.SetActive(false); // Make askGold initially invisible
        }
        else
        {
            Debug.LogError("No GameObject found with the tag 'askGoldCloud'.");
        }
    }

    // Update is called once per frame
    void Update()
    {

    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Player")) // Replace "Player" with the tag of your player
        {
            if (player.coinCount >= 10) // Check if player's coin count is equal or greater than 10
            {
                LoadScene2();
            }
            else
            {
                askGoldCloud.SetActive(true); // Make askGold visible

            }
        }
    }
    void OnTriggerExit2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Player")) // Replace "Player" with the tag of your player
        {
            askGoldCloud.SetActive(false); // Make askGold invisible again
        }
    }
    public void LoadScene2()
    {
        StartCoroutine(MoveObjectsOffScreen());
    }

    IEnumerator MoveObjectsOffScreen()
    {
        float elapsedTime = 0;

        // Get the initial relative positions
        Vector3 playerInitialRelativePosition = playerTransform.position - playerBalloonTransform.position;
        Vector3 ticketSellerInitialRelativePosition = ticketSellerTransform.position - playerBalloonTransform.position;

        // Calculate the target position for the balloon
        // Calculate the target position for the balloon
        float targetY = Camera.main.ScreenToWorldPoint(new Vector3(0, Screen.height, 0)).y + additionalHeight; Vector3 playerBalloonTargetPosition = new Vector3(playerBalloonTransform.position.x, targetY, playerBalloonTransform.position.z);

        while (elapsedTime < moveDuration)
        {
            // Calculate the new position for the balloon
            Vector3 playerBalloonNewPosition = Vector3.Lerp(playerBalloonTransform.position, playerBalloonTargetPosition, elapsedTime / moveDuration);

            // Set the new position for the balloon
            playerBalloonTransform.position = playerBalloonNewPosition;

            // Set the new positions for the player and the ticket seller relative to the balloon
            playerTransform.position = playerBalloonTransform.position + playerInitialRelativePosition;
            ticketSellerTransform.position = playerBalloonTransform.position + ticketSellerInitialRelativePosition;

            elapsedTime += Time.deltaTime;
            yield return null;
        }

        // Load the new scene
        SceneManager.LoadScene("LoadingScene2");
    }

}