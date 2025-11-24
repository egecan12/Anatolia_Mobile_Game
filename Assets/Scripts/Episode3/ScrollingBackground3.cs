using UnityEngine;

public class ScrollingBackground3 : MonoBehaviour
{
    public float backgroundSpeed = 2f;
    public GameObject background1;
    public GameObject background2;

    private float width1;
    private float width2;
    
    // Store initial Y positions to maintain vertical stability
    private float initialY1;
    private float initialY2;

    void Start()
    {
        // Get widths from renderers
        if (background1.GetComponent<Renderer>())
        {
            width1 = background1.GetComponent<Renderer>().bounds.size.x;
            initialY1 = background1.transform.position.y;
        }
        
        if (background2.GetComponent<Renderer>())
        {
            width2 = background2.GetComponent<Renderer>().bounds.size.x;
            initialY2 = background2.transform.position.y;
        }

        // Ensure they are aligned at start (optional but recommended)
        // This places background2 exactly after background1
        if (background1 != null && background2 != null)
        {
             Vector3 startPos2 = background1.transform.position;
             startPos2.x += width1;
             background2.transform.position = startPos2;
        }
    }

    void Update()
    {
        if (background1 == null || background2 == null) return;

        float move = backgroundSpeed * Time.deltaTime;

        // Move both backgrounds left
        background1.transform.Translate(Vector3.left * move);
        background2.transform.Translate(Vector3.left * move);

        // Check if background1 is off screen to the left
        if (background1.transform.position.x < -width1)
        {
            // Move background1 to the right of background2
            Vector3 newPos = background2.transform.position;
            newPos.x += width2;
            // Reset Y to initial to avoid drift
            newPos.y = initialY1; 
            background1.transform.position = newPos;
        }

        // Check if background2 is off screen to the left
        if (background2.transform.position.x < -width2)
        {
            // Move background2 to the right of background1
            Vector3 newPos = background1.transform.position;
            newPos.x += width1;
            // Reset Y to initial
            newPos.y = initialY2;
            background2.transform.position = newPos;
        }
    }
}
