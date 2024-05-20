using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BalloonControl : MonoBehaviour
{
    public float upForce = 200f; // The upward force
    private Rigidbody2D rb; // The balloon's rigidbody

    // Start is called before the first frame update
    void Start()
    {
        // Get the rigidbody component
        rb = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        // If the space bar is pressed...
        if (Input.GetKeyDown(KeyCode.Space))
        {
            // ...apply an upward force to the balloon
            rb.velocity = Vector2.zero;
            rb.AddForce(new Vector2(0, upForce));
        }
    }
}