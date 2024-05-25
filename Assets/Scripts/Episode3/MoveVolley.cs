using UnityEngine;

public class MoveVolley : MonoBehaviour
{
    public float speed = 1f; // Speed of the object
    public float rotationSpeed = 0.03f; // Rotation speed of the object
    public float scaleDecreaseSpeed = 0.01f; // Speed at which the object's scale decreases

    private void Update()
    {
        // Move the object from left to right
        transform.position -= new Vector3(speed * Time.deltaTime, 0, 0);

        // Rotate the object a little bit on each frame around the Y axis
        transform.Rotate(0, rotationSpeed, 0);

        // Decrease the x and z scales of the object over time to simulate 3D perspective
        float newXScale = transform.localScale.x - scaleDecreaseSpeed * Time.deltaTime;
        float newZScale = transform.localScale.z - scaleDecreaseSpeed * Time.deltaTime;
        transform.localScale = new Vector3(Mathf.Max(newXScale, 0.1f), transform.localScale.y, Mathf.Max(newZScale, 0.1f));
    }

    void OnTriggerEnter2D(Collider2D other)  //Checks characters collisions
    {
        // Falling reduces the health logic
        if (other.gameObject.tag == "Death")
        {
            Destroy(gameObject);
        }
    }
}