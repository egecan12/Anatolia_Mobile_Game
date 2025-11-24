using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI; // You need to import UnityEngine.UI to use Image
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement; // Include this namespace
using TMPro; // Import the TextMeshPro namespace
public class PlayerBalloon : MonoBehaviour
{
    public Image[] hearts;
    public int maxHealth;
    public int currentHealth;
    private bool isExploding = false; // Define isExploding

    bool isImmune = false;
    public float immuneTime = 3f; // The duration of the immunity and blinking effect (3 seconds)
    public float blinkInterval = 0.1f; // The interval between each blink
    public float upForce = 200f; // The upward force
    private Rigidbody2D rb; // The balloon's rigidbody
    private Vector3 startPosition; // Define startPosition
    private Animator anim; // Define anim
    private bool isRising;
    private GiantBird giantBird;
    public InputActionReference jump;
    private SpriteRenderer balloonRenderer; // Balloon sprite renderer for blink effect
    private bool isBlinking = false;
    
    // Game Over UI
    public TextMeshProUGUI gameOverText;
    public Button continueButton; // Continue/Restart button reference
    private bool isGameOver = false;
    private GameObject gameOverPanel; // Game Over panel reference


    // Start is called before the first frame update
    void Start()
    {
        // Get the rigidbody component
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>(); // Get the Animator component
        balloonRenderer = GetComponent<SpriteRenderer>(); // Get the SpriteRenderer for blink effect
        startPosition = transform.position; // Set the start position
        
        // Always reset game state when scene starts (this ensures restart works properly)
        ResetGameState();
        
        // Hide Game Over UI initially
        if (gameOverText != null)
        {
            gameOverText.gameObject.SetActive(false);
            gameOverPanel = gameOverText.transform.parent.gameObject; // Get the panel
            gameOverPanel.SetActive(false);
        }
        if (continueButton != null)
        {
            continueButton.gameObject.SetActive(false);
            
            // Check if button has click listeners
            int persistentEventCount = continueButton.onClick.GetPersistentEventCount();
            int runtimeListenerCount = continueButton.onClick.GetPersistentEventCount();
            
            Debug.Log($"🔍 Button Event Kontrolü:");
            Debug.Log($"   - Persistent Events: {persistentEventCount}");
            Debug.Log($"   - Runtime Listeners: {runtimeListenerCount}");
            
            if (persistentEventCount > 0 || runtimeListenerCount > 0)
            {
                Debug.Log($"✅ ContinueButton {persistentEventCount + runtimeListenerCount} event listener'a sahip");
            }
            else
            {
                Debug.LogWarning("⚠️ ContinueButton'da event listener yok! Otomatik olarak atanıyor...");
                
                // Automatically add the click listener
                continueButton.onClick.RemoveAllListeners();
                continueButton.onClick.AddListener(() => {
                    Debug.Log("🔄 Restart butonu tıklandı! (Otomatik atanan)");
                    SimpleRestart(); // Use simple method for better reliability
                });
                
                Debug.Log("✅ Button click event otomatik olarak atandı!");
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (isGameOver)
        {
            return; // Stop responding to inputs when game is over
        }

        if (jump.action.triggered && currentHealth > 0)
        {
            Jump();
        }

        checkHealthStatus();
    }

    void Jump()
    {
        rb.velocity = Vector2.zero;
        rb.AddForce(new Vector2(0, upForce));
        isRising = true;
        anim.SetBool("isRising", isRising);
        StartCoroutine(WaitForAnimation());

    }

    IEnumerator WaitForAnimation()
    {
        // Wait for the length of the animation
        yield return new WaitForSeconds(anim.GetCurrentAnimatorStateInfo(0).length);
        // Set isRising to false
        isRising = false;
        // Update the isRising parameter in the Animator
        anim.SetBool("isRising", isRising);
    }

    public void reduceHealth(int amount)
    {
        if (!isImmune)
        {
            currentHealth -= amount;
            isImmune = true; // PlayerBalloon becomes immune after health is reduced
            
            // Start blink effect immediately when health is reduced
            StartCoroutine(BlinkBalloonEffect());

            //Perform the explosion animation every time when reduceHealth is called
            if (anim != null)
            {
                anim.SetBool("isExploding", true);

                // Start a Coroutine to wait for the animation to finish
                StartCoroutine(WaitForExplosionAnimation());
            }

            //death
            if (currentHealth <= 0)
            {
                if (rb != null)
                {
                    // Apply a downward force
                    rb.AddForce(new Vector2(0, -1), ForceMode2D.Impulse);
                    // If the player is dead, freeze the X position and rotation
                    rb.constraints = RigidbodyConstraints2D.FreezePositionX | RigidbodyConstraints2D.FreezeRotation;
                    anim.SetBool("isExploded", true);



                }
            }
        }
    }
    IEnumerator WaitForExplosionAnimation()
    {
        // Wait for the length of the explosion animation
        yield return new WaitForSeconds(anim.GetCurrentAnimatorStateInfo(0).length);
        // Set isExploding to false
        anim.SetBool("isExploding", false);
        // Immunity will be handled by BlinkBalloonEffect coroutine


        // Change the sprite of the object

    }

    // Blink effect coroutine for 3 seconds immunity period
    IEnumerator BlinkBalloonEffect()
    {
        isBlinking = true;
        Color originalColor = balloonRenderer.color;
        
        // Blink for the duration of immunity
        float blinkDuration = immuneTime;
        float blinkTimer = 0f;
        
        while (blinkTimer < blinkDuration)
        {
            // Toggle visibility
            balloonRenderer.enabled = !balloonRenderer.enabled;
            
            // Wait for blink interval
            yield return new WaitForSeconds(blinkInterval);
            
            blinkTimer += blinkInterval;
        }
        
        // Ensure balloon is visible at the end
        balloonRenderer.enabled = true;
        balloonRenderer.color = originalColor;
        isBlinking = false;
        isImmune = false; // End immunity period
    }

    void checkHealthStatus()
    {
        for (int i = 0; i < hearts.Length; i++)
        {
            hearts[i].gameObject.SetActive(false);
        }

        for (int i = 0; i < currentHealth; i++)
        {
            hearts[i].gameObject.SetActive(true);
        }

        // Check if player is dead and show Game Over UI
        if (currentHealth <= 0 && !isGameOver)
        {
            Debug.Log("💀 Player öldü! Game Over UI başlatılıyor...");
            StartCoroutine(GameOverAfterDelay(1f)); // Show Game Over after 1 second delay
        }
    }

    void OnCollisionEnter2D(Collision2D col)  //Checks characters collisions
    {
        // Falling reduces the health logic
        if (col.gameObject.tag == "Death")
        {
            // Use reduceHealth to trigger Game Over UI and immunity
            reduceHealth(10); // Use reduceHealth instead of direct currentHealth change
        }
    }
    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.tag == "Enemy")
        {
            // If the player is not attacking, reduce their health
            reduceHealth(1); // replace 1 with the amount of health you want to reduce
        }
        if (other.gameObject.tag == "GiantBird")
        {
            // If the player is not attacking, reduce their health
            reduceHealth(3); // replace 3 with the amount of health you want to reduce

            // Start the LoadSceneAfterDelay coroutine
            StartCoroutine(LoadSceneAfterDelay(2));

        }

    }
    IEnumerator LoadSceneAfterDelay(float delay)
    {
        // Wait for the specified delay
        yield return new WaitForSeconds(delay);

        // Load the new scene
        SceneManager.LoadScene("loadingScene3");
    }

    // Game Over coroutine (similar to Episode 1)
    IEnumerator GameOverAfterDelay(float delay)
    {
        Debug.Log($"⏰ {delay} saniye bekleniyor...");
        yield return new WaitForSecondsRealtime(delay); // Wait for the specified delay
        isGameOver = true;
        
        Debug.Log("🎮 Game Over UI gösteriliyor...");
        
        // Show Game Over panel first
        if (gameOverPanel != null)
        {
            gameOverPanel.SetActive(true);
            Debug.Log($"✅ Game Over panel gösterildi - Aktif: {gameOverPanel.activeInHierarchy}");
        }
        else
        {
            Debug.LogError("❌ gameOverPanel referansı null!");
        }
        
        // Show Game Over text
        if (gameOverText != null)
        {
            gameOverText.text = "Game Over";
            gameOverText.gameObject.SetActive(true);
            Debug.Log($"✅ Game Over text gösterildi - Aktif: {gameOverText.gameObject.activeInHierarchy}, Parent: {gameOverText.transform.parent.name}");
        }
        else
        {
            Debug.LogError("❌ gameOverText referansı null!");
        }
        
        // Show Continue/Restart button
        if (continueButton != null)
        {
            continueButton.gameObject.SetActive(true);
            Debug.Log($"✅ Restart button gösterildi - Aktif: {continueButton.gameObject.activeInHierarchy}, Parent: {continueButton.transform.parent.name}");
        }
        else
        {
            Debug.LogError("❌ continueButton referansı null!");
        }
    }

    // Continue/Restart button click handler (same as Episode 1)
    public void OnContinueClick()
    {
        Debug.Log("🔄 OnContinueClick() çağrıldı! Sahne yeniden yükleniyor...");
        
        // Get current scene name for debugging
        string currentSceneName = SceneManager.GetActiveScene().name;
        int currentSceneIndex = SceneManager.GetActiveScene().buildIndex;
        
        Debug.Log($"📍 Mevcut sahne: {currentSceneName} (Index: {currentSceneIndex})");
        Debug.Log("🔄 Sahne baştan yükleniyor...");
        
        // Simply restart the entire scene - cleaner and more reliable
        SceneManager.LoadScene(currentSceneIndex);
        
        Debug.Log("✅ Sahne yeniden yükleme komutu gönderildi!");
    }
    
    // Alternative restart method using coroutine
    public void RestartSceneWithDelay()
    {
        StartCoroutine(RestartSceneCoroutine());
    }
    
    IEnumerator RestartSceneCoroutine()
    {
        Debug.Log("🔄 Restart coroutine başlatıldı...");
        
        // Wait a frame to ensure UI is processed
        yield return null;
        
        // Get current scene info
        string currentSceneName = SceneManager.GetActiveScene().name;
        int currentSceneIndex = SceneManager.GetActiveScene().buildIndex;
        
        Debug.Log($"📍 Restart için sahne: {currentSceneName} (Index: {currentSceneIndex})");
        
        // Try multiple restart methods
        Debug.Log("🔄 Yöntem 1: SceneManager.LoadScene() ile restart...");
        SceneManager.LoadScene(currentSceneIndex);
        
        // Wait a bit to see if it works
        yield return new WaitForSeconds(0.1f);
        
        // If still in same scene, try alternative method
        if (SceneManager.GetActiveScene().name == currentSceneName)
        {
            Debug.Log("⚠️ Yöntem 1 başarısız! Yöntem 2 deneniyor...");
            
            // Alternative: Load scene by name
            SceneManager.LoadScene(currentSceneName);
            
            yield return new WaitForSeconds(0.1f);
            
            if (SceneManager.GetActiveScene().name == currentSceneName)
            {
                Debug.Log("⚠️ Yöntem 2 de başarısız! Yöntem 3 deneniyor...");
                
                // Alternative: Force reload with async
                AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(currentSceneIndex);
                asyncLoad.allowSceneActivation = true;
                
                while (!asyncLoad.isDone)
                {
                    yield return null;
                }
            }
        }
        
        Debug.Log("✅ Restart coroutine tamamlandı!");
    }
    
    // Simple restart method - load Episode 2 through loading scene (like Episode 1)
    public void SimpleRestart()
    {
        Debug.Log("🔄 Episode 2 restart başlatıldı...");
        Debug.Log("📍 Loading scene üzerinden Episode 2'ye yönlendiriliyor...");
        
        // Load Episode 2 through loading scene (same as Episode 1)
        SceneManager.LoadScene("loadingScene2");
        
        Debug.Log("✅ Loading scene'e yönlendirildi!");
    }
    
    // Reset game state method - called when Episode2 is loaded after restart
    public void ResetGameState()
    {
        Debug.Log("🔄 Episode 2 oyun durumu sıfırlanıyor...");
        
        // Reset health and game state
        currentHealth = maxHealth;
        isImmune = false;
        isBlinking = false;
        isGameOver = false;
        
        // Reset position to start position
        if (rb != null)
        {
            rb.velocity = Vector2.zero;
            rb.constraints = RigidbodyConstraints2D.None; // Remove any constraints
            transform.position = startPosition;
        }
        
        // Reset animation states
        if (anim != null)
        {
            anim.SetBool("isRising", false);
            anim.SetBool("isExploding", false);
            anim.SetBool("isExploded", false);
        }
        
        // Reset balloon visibility
        if (balloonRenderer != null)
        {
            balloonRenderer.enabled = true;
            balloonRenderer.color = Color.white;
        }
        
        // Reset bird counters (important for restart functionality)
        BirdSpawner birdSpawner = FindObjectOfType<BirdSpawner>();
        if (birdSpawner != null)
        {
            birdSpawner.ResetCounters();
            Debug.Log("🔄 Bird counters sıfırlandı!");
        }
        else
        {
            // Fallback: directly reset static birdCount
            BirdSpawner.birdCount = 0;
            Debug.Log("⚠️ BirdSpawner bulunamadı, sadece static counter sıfırlandı!");
        }
        
        // Hide Game Over UI
        if (gameOverPanel != null)
        {
            gameOverPanel.SetActive(false);
        }
        if (gameOverText != null)
        {
            gameOverText.gameObject.SetActive(false);
        }
        if (continueButton != null)
        {
            continueButton.gameObject.SetActive(false);
        }
        
        Debug.Log("✅ Episode 2 oyun durumu başarıyla sıfırlandı!");
    }
}