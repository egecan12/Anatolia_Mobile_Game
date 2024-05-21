using System.Collections;
using UnityEngine;


public class Parallax2 : MonoBehaviour
{
    public Transform backgroundTransform; // The background's transform
    public float moveSpeed = 0.5f; // The speed at which the background moves
    private Vector3 startPosition; // The starting position of the background
    public float resetPosition = -20f; // The x position at which the background resets
    public float startPositionX = 20f; // The x position at which the background starts

    // Start is called before the first frame update
    void Start()
    {
        // Save the starting position
        startPosition = backgroundTransform.position;
    }

    // Update is called once per frame
    void Update()
    {
        // Move the background to the left
        backgroundTransform.position = new Vector3(backgroundTransform.position.x - moveSpeed * Time.deltaTime, backgroundTransform.position.y, backgroundTransform.position.z);

        // If the background is no longer visible, reset its position
        if (backgroundTransform.position.x < resetPosition)
        {
            backgroundTransform.position = new Vector3(startPositionX, backgroundTransform.position.y, backgroundTransform.position.z);
        }
    }
}