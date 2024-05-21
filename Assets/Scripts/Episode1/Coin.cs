using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Coin : MonoBehaviour
{

    private Collider2D col;

    void Start()
    {
        col = gameObject.AddComponent<BoxCollider2D>();
        col.isTrigger = true;
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            other.gameObject.GetComponent<Player>().IncreaseCoinCount(); // Increase the player's coin count
            Destroy(gameObject); // Destroy the coin
        }
    }
}