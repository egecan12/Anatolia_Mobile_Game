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

    void Start()
    {
        background1StartPosition = background1.transform.position;
        background2StartPosition = background2.transform.position;
        backgroundWidth1 = background1.GetComponent<Renderer>().bounds.size.x;
        backgroundWidth2 = 2 * background2.GetComponent<Renderer>().bounds.size.x;
    }

    void Update()
    {
        // Move the backgrounds
        background1.transform.position -= new Vector3(backgroundSpeed * Time.deltaTime, 0, 0);
        background2.transform.position -= new Vector3(backgroundSpeed * Time.deltaTime, 0, 0);

        // If background1 has moved off-screen, reset its position
        if (background1.transform.position.x < background1StartPosition.x - backgroundWidth1)
        {
            Vector3 spawnPosition = new Vector3(background2.transform.position.x + backgroundWidth1, background1.transform.position.y, background1.transform.position.z);
            background1.transform.position = spawnPosition;
        }
        // If background2 has moved off-screen, reset its position
        if (background2.transform.position.x < background2StartPosition.x - backgroundWidth2)
        {
            background2.transform.position = background2StartPosition;
        }


    }
}