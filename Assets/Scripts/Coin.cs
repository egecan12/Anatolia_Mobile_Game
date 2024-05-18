using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Coin : MonoBehaviour
{


    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            other.gameObject.GetComponent<Player>().IncreaseCoinCount(); // Increase the player's coin count
            Destroy(gameObject); // Destroy the coin
        }
    }
}