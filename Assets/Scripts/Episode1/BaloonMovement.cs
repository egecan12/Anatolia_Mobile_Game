using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaloonMovement : MonoBehaviour
{
    public float verticalSpeed = 0.5f;
    public float amplitude = 0.5f;
    public float horizontalSpeed = 0.3f;
    public float horizontalAmplitude = 0.2f;

    private Vector3 initialPosition;

    void Start()
    {
        initialPosition = transform.position;
    }

    void Update()
    {
        float newY = initialPosition.y + Mathf.Sin(Time.time * verticalSpeed) * amplitude;
        float newX = initialPosition.x + Mathf.Sin(Time.time * horizontalSpeed) * horizontalAmplitude;
        transform.position = new Vector3(newX, newY, initialPosition.z);
    }
}