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
        // Move the giant bird towards the player
        transform.position = Vector3.MoveTowards(transform.position, player.position, speed * Time.deltaTime);
    }


}
