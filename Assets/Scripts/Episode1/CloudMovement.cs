using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CloudMovement : MonoBehaviour
{
    public float minSpeed = 0.2f;
    public float maxSpeed = 0.5f;
    public float screenWidth = 35f; // Total screen width for seamless looping
    
    private float speed;
    private float initialX; // Starting X position
    private float initialY; // Starting Y position (to maintain cloud height)

    void Start()
    {
        speed = Random.Range(minSpeed, maxSpeed);
        initialX = transform.position.x; // Store starting position
        initialY = transform.position.y; // Store starting Y position
        
        // Cloud movement initialized
    }

    void Update()
    {
        float timeMovement = speed * Time.time; // Movement based on time (always forward)
        float loopedPosition = initialX + (timeMovement % screenWidth);
        
        // Apply movement while maintaining original Y position - NO vertical movement
        Vector3 currentPos = transform.position;
        currentPos.x = loopedPosition;
        currentPos.y = initialY; // Keep original Y position - clouds stay at their intended height
        
        transform.position = currentPos;
        
        // NO vertical sway - clouds stay at fixed height for stable background
        
        // Cloud movement update - no debug spam
    }
}