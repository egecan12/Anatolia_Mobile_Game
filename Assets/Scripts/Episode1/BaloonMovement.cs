using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaloonMovement : MonoBehaviour
{
    [Header("Floating Settings")]
    public float verticalSpeed = 0.3f;          // Slower vertical movement
    public float amplitude = 0.2f;             // Smaller amplitude for gentle floating
    public float horizontalSpeed = 0.4f;       // Slightly faster horizontal sway
    public float horizontalAmplitude = 0.3f;    // More noticeable horizontal movement
    
    [Header("Realistic Physics")]
    public float windInfluence = 0.1f;         // Random wind effects
    public float smoothnessFactor = 0.02f;     // How smoothly the balloon moves
    public bool addWindVariation = true;       // Optional wind variation
    
    private Vector3 initialPosition;
    private Vector3 currentOffset;
    private Vector3 targetOffset;
    private float windTimer;
    private float randomSeed;

    void Start()
    {
        initialPosition = transform.position;
        currentOffset = Vector3.zero;
        randomSeed = Random.Range(0f, 10f); // Add variety to wind patterns
    }

    void Update()
    {
        // Generate realistic balloon movement
        float time = Time.time + randomSeed;
        windTimer += Time.deltaTime;
        
        // Calculate target floating offset with gentle sine waves
        float verticalOffset = Mathf.Sin(time * verticalSpeed) * amplitude;
        float horizontalOffset = Mathf.Sin(time * horizontalSpeed) * horizontalAmplitude;
        
        // Add wind variation for realism
        if (addWindVariation)
        {
            float windVariation = Mathf.Sin(time * 0.2f) * windInfluence;
            horizontalOffset += windVariation;
        }
        
        targetOffset = new Vector3(horizontalOffset, verticalOffset, 0);
        
        // Smoothly interpolate to target position (realistic balloon inertia)
        currentOffset = Vector3.Lerp(currentOffset, targetOffset, smoothnessFactor);
        
        // Apply the offset to the balloon's position
        transform.position = initialPosition + currentOffset;
    }
}