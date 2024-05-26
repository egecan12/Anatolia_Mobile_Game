using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;
using UnityEngine.InputSystem;

public class LoadSceneAfterDelay : MonoBehaviour
{
    public string sceneName = "Episode1";
    [SerializeField] private InputActionReference pass;
    private bool loadSceneTriggered = false;

    void OnEnable()
    {
        pass.action.performed += _ => loadSceneTriggered = true;
    }

    void OnDisable()
    {
        pass.action.performed -= _ => loadSceneTriggered = true;
    }

    void Start()
    {
        StartCoroutine(LoadAfterDelay(sceneName));
    }

    IEnumerator LoadAfterDelay(string sceneName)
    {
        while (true)
        {
            if (loadSceneTriggered)
            {
                break;
            }

            yield return null;
        }

        SceneManager.LoadScene(sceneName);
    }
}