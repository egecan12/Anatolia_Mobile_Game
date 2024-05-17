using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public Transform playerTransform;
    public Vector3 offset;
    // Start is called before the first frame update
    void LateUpdate()
    {
        Vector3 newPosition = transform.position;
        newPosition.x = Mathf.Clamp(playerTransform.position.x + offset.x, -1.8f, float.MaxValue);
        transform.position = newPosition;
    }
}

