using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Balloon : MonoBehaviour
{
    public float speed = 1f; // Horizontal movement speed
    public float verticalSpeed = 0.5f; // Vertical movement speed
    public float maxDistance = 20f; // Maximum horizontal movement distance
    public float maxVerticalDistance = 2f; // Maximum vertical movement distance
    public float rotationSpeed = 3f; // Speed of the rotation
    public float maxRotation = 5f; // Maximum rotation angle

    private Vector3 startPosition;

    void Start()
    {
        startPosition = transform.position;
    }

    void Update()
    {
        float horizontalMovement = Mathf.PingPong(Time.time * speed, maxDistance * 2) - maxDistance;
        float verticalMovement = maxVerticalDistance * Mathf.Sin(Time.time * verticalSpeed);
        float rotation = maxRotation * Mathf.Sin(Time.time * rotationSpeed);
        transform.position = startPosition + new Vector3(horizontalMovement, verticalMovement, 0);
        transform.rotation = Quaternion.Euler(0, 0, rotation);
    }
}