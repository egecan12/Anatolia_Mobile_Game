using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow3 : MonoBehaviour
{
    public Transform playerTransform; // The player's transform

    // Update is called once per frame
    void Update()
    {
        // Update the camera's position to match the player's position
        transform.position = new Vector3(playerTransform.position.x, playerTransform.position.y, transform.position.z);
    }
}
