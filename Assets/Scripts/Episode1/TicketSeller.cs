using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TicketSeller : MonoBehaviour
{

    private SpriteRenderer spriteRenderer;
    private GameObject askGoldCloud;
    // Start is called before the first frame update
    public Player player; // Reference to the Player script
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
                SceneManager.LoadScene("LoadingScene2"); // Load the scene
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


}