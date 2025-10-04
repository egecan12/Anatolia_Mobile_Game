using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TicketSeller : MonoBehaviour
{
    public Transform playerTransform; // The player's transform
    public Transform ticketSellerTransform; // The ticket seller's transform
    public float moveDuration = 3f; // The duration of the move - 3 seconds for smooth lift
    public Transform playerBalloonTransform; // The player balloon's transform

    private SpriteRenderer ticketSellerSpriteRenderer; // TicketSeller's sprite renderer
    private GameObject askGoldCloud;
    private Collider2D ticketSellerCollider; // Reference to ticket seller's collider
    private bool isBalloonActive = false; // Track if balloon ride has started
    // Start is called before the first frame update
    public Player player; // Reference to the Player script
    public float additionalHeight = 10f; // The additional height they should rise
    void Start()
    {
        // Get TicketSeller's sprite renderer for hiding during balloon ride
        ticketSellerSpriteRenderer = ticketSellerTransform.GetComponent<SpriteRenderer>();
        askGoldCloud = GameObject.FindGameObjectWithTag("askGoldCloud"); // Initialize askGoldCloud here
        if (askGoldCloud != null)
        {
            askGoldCloud.SetActive(false); // Make askGold initially invisible
        }
        else
        {
            Debug.LogError("No GameObject found with the tag 'askGoldCloud'.");
        }
        
        // Get the ticket seller's collider and PERMANENTLY make it non-blocking
        ticketSellerCollider = ticketSellerTransform.GetComponent<Collider2D>();
        if (ticketSellerCollider != null)
        {
            // COMPLETELY DISABLE COLLISION - DRASTIC SOLUTION
            ticketSellerCollider.enabled = false;
            Debug.Log("❌ TicketSeller collider COMPLETELY DISABLED!");
            Debug.Log($"🎫 TicketSeller collider type: {ticketSellerCollider.GetType()}");
            
            // Force re-enable it but set as trigger
            ticketSellerCollider.enabled = true;
            ticketSellerCollider.isTrigger = true;
            Debug.Log("✅ TicketSeller collider re-enabled as TRIGGER only!");
        }
        else
        {
            Debug.LogError("❌ TicketSeller collider NOT FOUND!");
        }
    }

    // Update is called once per frame
    private float lastDebugTime = 0f;
    void Update()
    {
        // Debug every 5 seconds just to show system is working
        if (Time.time - lastDebugTime > 5f)
        {
            if (playerTransform != null && ticketSellerTransform != null)
            {
                float distanceToPlayer = Vector2.Distance(playerTransform.position, ticketSellerTransform.position);
                Debug.Log($"🎫 Distance: {distanceToPlayer:F2}, ALWAYS NON-BLOCKING, BalloonActive: {isBalloonActive}");
                lastDebugTime = Time.time;
            }
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Player")) // Replace "Player" with the tag of your player
        {
            Debug.Log($"🎫 Player entered TicketSeller area - Coins: {player.coinCount}");
            
            if (player.coinCount >= 10) // Check if player's coin count is equal or greater than 10
            {
                Debug.Log("🎫 Player has enough coins - STARTING INSTANT BALLOON RIDE!");
                Debug.Log("🎈 Player will be instantly positioned behind balloon!");
                LoadScene2();
            }
            else
            {
                Debug.Log("🎫 Player needs more coins - Showing askGold cloud");
                askGoldCloud.SetActive(true); // Make askGold visible
            }
        }
    }
    void OnTriggerExit2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Player")) // Replace "Player" with the tag of your player
        {
            askGoldCloud.SetActive(false); // Make askGold invisible again
        }
    }
    public void LoadScene2()
    {
        isBalloonActive = true; // Mark balloon ride as started
        Debug.Log("🎈 BALLOON RIDE STARTING - Disabling collision completely!");
        
        // COMPLETELY DISABLE TICKETSELLER COLLISION AFTER BALLOON RIDE STARTS
        if (ticketSellerCollider != null)
        {
            ticketSellerCollider.enabled = false;
            Debug.Log("❌ TicketSeller collider DISABLED during balloon ride!");
        }
        
        // DISABLE ALL COLLISIONS AROUND BALLOON AREA
        Debug.Log("🎈 DISABLING ALL NEARBY COLLISIONS for smooth balloon ride!");
        
        // Find all GameObjects with colliders near balloon area
        GameObject[] allObjects = FindObjectsOfType<GameObject>();
        foreach (GameObject obj in allObjects)
        {
            Collider2D objCollider = obj.GetComponent<Collider2D>();
            if (objCollider != null && objCollider.enabled)
            {
                float distanceToBalloon = Vector2.Distance(obj.transform.position, playerBalloonTransform.position);
                if (distanceToBalloon < 5f && !obj.name.Contains("Player") && !obj.name.Contains("Ground"))
                {
                    objCollider.enabled = false;
                    Debug.Log($"❌ Disabled collision for: {obj.name} (distance: {distanceToBalloon:F2})");
                }
            }
        }
        
        // INSTANTLY POSITION PLAYER BEHIND BALLOON
        Debug.Log("🎈 INSTANTLY POSITIONING PLAYER BEHIND BALLOON!");
        
        // HIDE TICKETSELLER DURING BALLOON RIDE
        if (ticketSellerSpriteRenderer != null)
        {
            ticketSellerSpriteRenderer.enabled = false;
            Debug.Log("🎈 TicketSeller sprite HIDDEN during balloon ride!");
        }
        
        // Calculate player's final relative position behind balloon
        Vector3 targetPlayerPosition = playerBalloonTransform.position;
        targetPlayerPosition.z = 5f; // Behind balloon
        targetPlayerPosition.y -= 1.5f; // Slightly below balloon center
        targetPlayerPosition.x += 0.2f; // Slightly to the side
        
        // INSTANTLY SET PLAYER POSITION
        if (playerTransform != null)
        {
            playerTransform.position = targetPlayerPosition;
            Debug.Log($"🎈 Player instantly positioned at: {targetPlayerPosition}");
        }
        
        // Notify Player that balloon ride has started
        if (player != null)
        {
            player.SetBalloonRideStatus(true);
            Debug.Log("🎈 Player notified: Balloon ride started!");
        }
        
        StartCoroutine(MoveObjectsOffScreen());
    }

    IEnumerator MoveObjectsOffScreen()
    {
        float elapsedTime = 0;
        Vector3 balloonVelocity = Vector3.zero;
        bool hasLoggedStart = false;

        // Get the initial relative positions
        Vector3 playerInitialRelativePosition = playerTransform.position - playerBalloonTransform.position;
        Vector3 ticketSellerInitialRelativePosition = ticketSellerTransform.position - playerBalloonTransform.position;

        // Calculate the target position for the balloon
        float targetY = Camera.main.ScreenToWorldPoint(new Vector3(0, Screen.height, 0)).y + additionalHeight;
        Vector3 targetPosition = new Vector3(playerBalloonTransform.position.x, targetY, playerBalloonTransform.position.z);
        
        // Set proper z-ordering for balloon ride (DRAMATIC VERSION)
        // Player and TicketSeller should be behind the balloon
        Vector3 playerPos = playerTransform.position;
        Vector3 ticketSellerPos = ticketSellerTransform.position;
        playerPos.z = 5f; // BEHIND BALLOON - MORE DRAMATIC (higher z = further back)
        ticketSellerPos.z = 5f; // BEHIND BALLOON - MORE DRAMATIC
        playerTransform.position = playerPos;
        ticketSellerTransform.position = ticketSellerPos;
        
        // Ensure balloon stays in front
        Vector3 balloonPos = playerBalloonTransform.position;
        balloonPos.z = 0f; // BALLOON IN FRONT
        playerBalloonTransform.position = balloonPos;
        
        Debug.Log($"🎈 BALLOON Z-ordering: Player behind (z={playerPos.z}), Balloon in front (z={balloonPos.z})");

        // Balloon physics parameters - 1.5x faster lift speed
        float balloonLiftForce = 12f; // 8f * 1.5 = Stronger upward force
        float balloonDrag = 0.98f; // Less air resistance for faster movement
        float horizontalWindForce = 1.2f; // More noticeable horizontal sway
        float wobbleIntensity = 0.5f; // More prominent wobbling motion
        float accelerationFactor = 2.25f; // 1.5f * 1.5 = Additional acceleration boost

        Vector3 currentBalloonPosition = playerBalloonTransform.position;

        while (elapsedTime < moveDuration)
        {
            float deltaTime = Time.deltaTime;
            elapsedTime += deltaTime;

            // Calculate distance to target and force direction
            Vector3 toTarget = (targetPosition - currentBalloonPosition);
            
            // Calculate distance to target for acceleration
            float distanceToTarget = Vector3.Distance(currentBalloonPosition, targetPosition);
            float progress = elapsedTime / moveDuration; // How much time has progressed
            
            // Apply dynamic balloon lift - starts faster and accelerates
            if (currentBalloonPosition.y < targetPosition.y)
            {
                // Progressive acceleration: starts moderate, gets faster
                float speedMultiplier = 1f + (progress * accelerationFactor);
                balloonVelocity.y += (balloonLiftForce * speedMultiplier) * deltaTime;
            }

            // Apply gentle horizontal wind movement (more realistic balloon sway)
            balloonVelocity.x += Mathf.Sin(Time.time * horizontalWindForce) * deltaTime * wobbleIntensity;

            // Apply drag to create realistic balloon behavior (less drag for faster movement)
            balloonVelocity *= balloonDrag;

            // Limit maximum speed for smooth but even faster movement (1.5x speed)
            balloonVelocity.y = Mathf.Clamp(balloonVelocity.y, -3f, 9f); // 6f * 1.5 = Higher upward speed
            balloonVelocity.x = Mathf.Clamp(balloonVelocity.x, -2.25f, 2.25f); // 1.5f * 1.5 = More horizontal movement

            // Update balloon position
            currentBalloonPosition += balloonVelocity * deltaTime;
            playerBalloonTransform.position = currentBalloonPosition;

            // Set the new positions for the player and ticket seller relative to the balloon
            Vector3 newPlayerPos = playerBalloonTransform.position + playerInitialRelativePosition;
            Vector3 newTicketSellerPos = playerBalloonTransform.position + ticketSellerInitialRelativePosition;
            
            // INSTANTLY KEEP PLAYER BEHIND BALLOON (SLIGHTLY BELOW AND TO THE SIDE)
            newPlayerPos = new Vector3(
                currentBalloonPosition.x + 0.2f,  // Slightly to the side
                currentBalloonPosition.y - 1.5f, // Slightly below balloon center
                5f  // Behind balloon (same z-order throughout)
            );
            newTicketSellerPos.z = 5f; // Keep ticket seller FAR behind balloon
            
            // Ensure balloon stays in front during movement
            currentBalloonPosition.z = 0f; // BALLOON ALWAYS IN FRONT
            
            playerTransform.position = newPlayerPos;
            ticketSellerTransform.position = newTicketSellerPos;
            
            // KEEP TICKETSELLER HIDDEN DURING ENTIRE BALLOON RIDE
            if (ticketSellerSpriteRenderer != null && ticketSellerSpriteRenderer.enabled)
            {
                ticketSellerSpriteRenderer.enabled = false;
            }

            // Log movement start only once
            if (!hasLoggedStart && elapsedTime > 0.1f)
            {
                Debug.Log($"🚀 Balloon moving: TicketSeller hidden, Player behind balloon!");
                hasLoggedStart = true;
            }

            yield return null;
        }

        // Load the new scene
        Debug.Log("Balloon lift completed! Floating to next scene...");
        SceneManager.LoadScene("LoadingScene2");
    }

}