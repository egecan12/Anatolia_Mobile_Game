using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class LoadSceneAfterDelay : MonoBehaviour
{
    public string sceneName = "Episode1";
    public float delay = 5f;

    void Start()
    {
        StartCoroutine(LoadSceneAfterDelayCoroutine());
    }

    IEnumerator LoadSceneAfterDelayCoroutine()
    {
        yield return new WaitForSeconds(delay);
        SceneManager.LoadScene(sceneName);
    }
}