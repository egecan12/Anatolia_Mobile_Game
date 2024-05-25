using UnityEngine;

public class MoveFatguy : MonoBehaviour
{
    public float speed = 1f; // Speed of the object
    public float scaleSpeed = 0.03f; // The speed at which the object scales up

    private void Update()
    {
        // Move the object from left to right
        transform.position -= new Vector3(speed * Time.deltaTime, 0, 0);
        transform.localScale += new Vector3(scaleSpeed, scaleSpeed, scaleSpeed) * Time.deltaTime;

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