using UnityEngine;

public class MoveObject : MonoBehaviour
{
    public float speed = 1f; // Speed of the object

    private void Update()
    {
        // Move the object from left to right
        transform.position -= new Vector3(speed * Time.deltaTime, 0, 0);
    }
    void OnBecameInvisible()
    {
        Destroy(gameObject);
    }
}