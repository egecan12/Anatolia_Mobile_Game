using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CloudMovement : MonoBehaviour
{
    public float minSpeed = 0.2f;
    public float maxSpeed = 0.5f;
    public float rightEdge = 25f;
    public float leftEdge = -10f;

    private float speed;

    void Start()
    {
        speed = Random.Range(minSpeed, maxSpeed);
    }

    void Update()
    {
        Vector3 newPosition = transform.position;
        newPosition.x += speed * Time.deltaTime;

        // If the cloud has reached the right edge of the scene, move it to the left edge
        if (newPosition.x > rightEdge)
        {
            newPosition.x = leftEdge;
        }

        transform.position = newPosition;
    }
}