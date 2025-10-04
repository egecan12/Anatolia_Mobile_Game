using System.Collections;
using UnityEngine;


public class Parallax2 : MonoBehaviour
{
    public Transform backgroundTransform; // The background's transform
    public float moveSpeed = 0.5f; // The speed at which the background moves
    private Vector3 startPosition; // The starting position of the background
    private float loopWidth = 120f; // Much larger loop width for smooth scrolling

    // Start is called before the first frame update
    void Start()
    {
        // Save the starting position
        startPosition = backgroundTransform.position;
    }

    // Update is called once per frame
    void Update()
    {
        // Calculate movement using time-based seamless looping
        float movement = moveSpeed * Time.time;
        float loopedMovement = movement % loopWidth;
        
        // Create smooth infinite scrolling - NO vertical movement for stability
        Vector3 newPosition = startPosition;
        newPosition.x -= loopedMovement;
        // newPosition.y stays at startPosition.y - no vertical wobble
        
        backgroundTransform.position = newPosition;
        
        // Background movement update - clean console
    }
}