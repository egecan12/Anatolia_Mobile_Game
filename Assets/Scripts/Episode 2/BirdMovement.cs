using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BirdMovement : MonoBehaviour
{
    public float moveSpeed = 5f; // The speed at which the bird moves

    // Update is called once per frame
    void Update()
    {
        // Move the bird to the left
        transform.position += new Vector3(-moveSpeed * Time.deltaTime, 0, 0);
    }
}