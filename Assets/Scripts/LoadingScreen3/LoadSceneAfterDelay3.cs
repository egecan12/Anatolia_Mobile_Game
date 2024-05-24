using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class LoadSceneAfterDelay3 : MonoBehaviour
{
    private bool canLoadScene = false;

    void Start()
    {
        StartCoroutine(WaitForSeconds(6));
    }

    void Update()
    {
        if (canLoadScene && Input.anyKeyDown)
        {
            // Load your scene here
            SceneManager.LoadScene("Episode3");
        }
    }

    IEnumerator WaitForSeconds(int seconds)
    {
        yield return new WaitForSeconds(seconds);
        canLoadScene = true;
    }
}