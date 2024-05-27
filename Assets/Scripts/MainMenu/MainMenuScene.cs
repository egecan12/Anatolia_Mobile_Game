using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuScene : MonoBehaviour
{
    private CanvasGroup fadeGroup;
    private float fadeInSpeed = 0.33f;

    private void Start()
    {
        //Grab the only CanvasGroup in the scene
        fadeGroup = FindObjectOfType<CanvasGroup>();

        //Start with a white screen
        fadeGroup.alpha = 1;
    }

    private void Update()
    {
        //Fade-in
        fadeGroup.alpha = 1 - Time.timeSinceLevelLoad * fadeInSpeed;

        //When the screen is fully faded in, disable the CanvasGroup
        if (fadeGroup.alpha <= 0)
        {
            fadeGroup.interactable = false;
            fadeGroup.blocksRaycasts = false;
        }
    }

    public void OnPlayClick()
    {
        SceneManager.LoadScene("LoadingScreen");
    }
}
