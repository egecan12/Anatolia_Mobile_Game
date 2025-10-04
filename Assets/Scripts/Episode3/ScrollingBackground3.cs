using UnityEngine;

public class ScrollingBackground : MonoBehaviour
{
    public float backgroundSpeed = 2f;
    public GameObject background1;
    public GameObject background2;

    private Vector3 background1StartPosition;
    private Vector3 background2StartPosition;
    private float backgroundWidth1;
    private float backgroundWidth2;
    private float totalCycleWidth; // For seamless looping

    void Start()
    {
        background1StartPosition = background1.transform.position;
        background2StartPosition = background2.transform.position;
        backgroundWidth1 = background1.GetComponent<Renderer>().bounds.size.x;
        backgroundWidth2 = 2 * background2.GetComponent<Renderer>().bounds.size.x;
        
        // Calculate total cycle width for seamless looping
        totalCycleWidth = backgroundWidth1 + backgroundWidth2;
        
        // Background loop configured
    }

    void Update()
    {
        float movement = backgroundSpeed * Time.time;
        
        // Use modular arithmetic for seamless looping
        float loopedMovement1 = movement % totalCycleWidth;
        float loopedMovement2 = (movement + backgroundWidth1 * 0.5f) % totalCycleWidth;
        
        // Apply smooth seamless movement
        Vector3 pos1 = background1StartPosition;
        pos1.x -= loopedMovement1;
        background1.transform.position = pos1;
        
        Vector3 pos2 = background2StartPosition;
        pos2.x -= loopedMovement2;
        background2.transform.position = pos2;
        
        // Optional: Add subtle parallax effect
        float parallaxOffset = Mathf.Sin(Time.time * 0.1f) * 0.1f;
        background1.transform.position += new Vector3(0, parallaxOffset, 0);
        background2.transform.position += new Vector3(0, -parallaxOffset * 0.5f, 0);
        
        // Background movement update - clean console
    }
}