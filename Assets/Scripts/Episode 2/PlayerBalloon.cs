using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI; // You need to import UnityEngine.UI to use Image
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement; // Include this namespace
public class PlayerBalloon : MonoBehaviour
{
    public Image[] hearts;
    public int maxHealth;
    public int currentHealth;
    private bool isExploding = false; // Define isExploding

    bool isImmune = false;
    public float immuneTime = 2f; // The duration of the immunity and blinking effect
    public float blinkInterval = 0.1f; // The interval between each blink
    public float upForce = 200f; // The upward force
    private Rigidbody2D rb; // The balloon's rigidbody
    private Vector3 startPosition; // Define startPosition
    private Animator anim; // Define anim
    private bool isRising;
    private GiantBird giantBird;
    public InputActionReference jump;


    // Start is called before the first frame update
    void Start()
    {
        // Get the rigidbody component
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>(); // Get the Animator component
        startPosition = transform.position; // Set the start position
        currentHealth = maxHealth;
    }

    // Update is called once per frame
    void Update()
    {
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

            // Perform the explosion animation every time when reduceHealth is called
            // if (anim != null)
            // {
            //     anim.SetBool("isExploding", true);

            //     // Start a Coroutine to wait for the animation to finish
            //     StartCoroutine(WaitForExplosionAnimation());
            // }

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
    // IEnumerator WaitForExplosionAnimation()
    // {
    //     // Wait for the length of the explosion animation
    //     yield return new WaitForSeconds(anim.GetCurrentAnimatorStateInfo(0).length);
    //     // Set isExploding to false
    //     anim.SetBool("isExploding", false);
    //     isImmune = false;


    //     // Change the sprite of the object

    // }

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

    }

    void OnCollisionEnter2D(Collision2D col)  //Checks characters collisions
    {
        // Falling reduces the health logic
        if (col.gameObject.tag == "Death")
        {
            // Reduce current health
            currentHealth -= 10; // or any value you want
        }
    }
    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.tag == "Enemy")
        {
            // If the player is not attacking, reduce their health
            reduceHealth(1); // replace 1 with the amount of health you want to reduce
        }

    }
    IEnumerator LoadSceneAfterDelay(float delay)
    {
        // Wait for the specified delay
        yield return new WaitForSeconds(delay);

        // Load the new scene
        SceneManager.LoadScene("loadingScene3");
    }
}