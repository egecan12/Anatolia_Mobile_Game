using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;
using UnityEngine.InputSystem;

public class LoadSceneAfterDelay : MonoBehaviour
{
    public string sceneName;
    [SerializeField] private InputActionReference pass;
    private bool loadSceneTriggered = false;
    private bool canLoadScene = false;


    void OnEnable()
    {
        pass.action.performed += _ => loadSceneTriggered = true;
    }

    void OnDisable()
    {
        pass.action.performed -= _ => loadSceneTriggered = false;
    }

    void Start()
    {
        StartCoroutine(WaitForSeconds(6));

        StartCoroutine(LoadAfterDelay(sceneName));
    }

    IEnumerator LoadAfterDelay(string sceneName)
    {
        while (true)
        {
            if (loadSceneTriggered && canLoadScene)
            {
                break;
            }

            yield return null;
        }

        SceneManager.LoadScene(sceneName);
    }
    IEnumerator WaitForSeconds(int seconds)
    {
        yield return new WaitForSeconds(seconds);
        canLoadScene = true;
    }
}