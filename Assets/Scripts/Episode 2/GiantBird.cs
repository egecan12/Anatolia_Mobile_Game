using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GiantBird : MonoBehaviour
{
    public float speed = 2.0f; // The speed at which the giant bird moves towards the player

    private Transform player; // The player's position

    private void Start()
    {
        // Find the player's position
        player = GameObject.FindWithTag("Player").transform;
    }

    private void Update()
    {
        Debug.Log("mr");
        // Move the giant bird towards the player
        transform.position = Vector3.MoveTowards(transform.position, player.position, speed * Time.deltaTime);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        // Check if the giant bird has collided with the player
        if (collision.gameObject.tag == "Player")
        {
            // Handle collision with player
            // For example, you can destroy the player
            Destroy(collision.gameObject);

            // Get the PlayerBalloon script attached to the player GameObject
            PlayerBalloon playerBalloon = collision.gameObject.GetComponent<PlayerBalloon>();

            // Call the reduceHealth method
            playerBalloon.reduceHealth(3);

        }
    }
}
