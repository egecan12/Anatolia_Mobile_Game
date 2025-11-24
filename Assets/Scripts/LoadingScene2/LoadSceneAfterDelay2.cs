using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class LoadSceneAfterDelay2 : MonoBehaviour
{
    public string sceneName = "Episode2";

    void Start()
    {
        StartCoroutine(LoadAfterDelay(sceneName));
    }

    IEnumerator LoadAfterDelay(string sceneName)
    {
        // Wait for 2 seconds then automatically load the scene
        yield return new WaitForSeconds(2f);
        
        Debug.Log($"🔄 Loading scene'den {sceneName} sahnesine geçiliyor...");
        SceneManager.LoadScene(sceneName);
    }
}