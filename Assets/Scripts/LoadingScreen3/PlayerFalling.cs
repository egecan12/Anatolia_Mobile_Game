using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro; // Import the TextMeshPro namespace

public class PlayerFalling : MonoBehaviour
{
    private Animator animator;
    private Canvas canvas;
    private CanvasGroup canvasGroup;

    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();

        // Find all elements with the "TextUI" tag
        GameObject[] textElements = GameObject.FindGameObjectsWithTag("TextUI");

        // Loop through each element
        foreach (GameObject element in textElements)
        {
            // Get the TextMeshProUGUI component and set its color alpha to 0
            TextMeshProUGUI textComponent = element.GetComponent<TextMeshProUGUI>();
            if (textComponent != null)
            {
                Color color = textComponent.color;
                color.a = 0;
                textComponent.color = color;
            }
        }
    }

    // Update is called once per frame
    void Update()
    {


        animator.SetBool("isFalling", true);


    }
    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Death")
        {
            animator.speed = 0;
            // Make all UI elements visible again
            GameObject[] textElements = GameObject.FindGameObjectsWithTag("TextUI");
            foreach (GameObject element in textElements)
            {
                TextMeshProUGUI textComponent = element.GetComponent<TextMeshProUGUI>();
                if (textComponent != null)
                {
                    Color color = textComponent.color;
                    color.a = 1;
                    textComponent.color = color;
                }
            }

        }

    }
}

