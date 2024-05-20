using System.Collections;
using UnityEngine;

public class Parallax2 : MonoBehaviour
{
    public Transform backgroundTransform; // The background's transform
    public float moveSpeed = 0.5f; // The speed at which the background moves
    public float moveDuration = 10f; // The duration of the move
    private Vector3 startPosition; // The starting position of the background

    // Start is called before the first frame update
    void Start()
    {
        // Save the starting position
        startPosition = backgroundTransform.position;

        // Start the MoveAndReset coroutine
        StartCoroutine(MoveAndReset());
    }

    IEnumerator MoveAndReset()
    {
        while (true)
        {
            float elapsedTime = 0; // The elapsed time since the start of the move

            // The initial position at the start of the move
            Vector3 initialPosition = backgroundTransform.position;

            // The target position at the end of the move
            Vector3 targetPosition = initialPosition - new Vector3(moveSpeed * moveDuration, 0, 0);

            while (elapsedTime < moveDuration)
            {
                // Calculate the new position
                Vector3 newPosition = Vector3.Lerp(initialPosition, targetPosition, elapsedTime / moveDuration);

                // Set the new position
                backgroundTransform.position = newPosition;

                elapsedTime += Time.deltaTime;
                yield return null;
            }

            // Ensure the background is exactly at the target position at the end of the move
            backgroundTransform.position = targetPosition;

            // Reset the background to the starting position
            backgroundTransform.position = startPosition;
        }
    }
}