using UnityEngine;

public class SandbagDrop : MonoBehaviour
{
    public GameObject sandbagPrefab; // The sandbag prefab
    public float dropInterval = 5f; // The interval between drops

    void Start()
    {
        // Start dropping sandbags every dropInterval seconds
        InvokeRepeating("DropSandbag", dropInterval, dropInterval);
    }

    void DropSandbag()
    {
        // Instantiate a new sandbag at the balloon's position
        Instantiate(sandbagPrefab, transform.position, Quaternion.identity);
    }
}