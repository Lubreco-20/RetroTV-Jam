using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class SceneLoaderManager : MonoBehaviour
{
    string sceneName;

    Animator anim;

    [SerializeField]PlayerController controller;

    static SceneLoaderManager inst;

    private void Awake()
    {
        DisableDuplicatedComponents();

        if (inst == null) inst = this;
        else Destroy(gameObject);
    }

    private void Start()
    {
        Time.timeScale = 1f;
        anim = GetComponent<Animator>();
        controller.DisableInput();
        anim.updateMode = AnimatorUpdateMode.Normal;
        anim.Play("FadeOut");
    }

    public void EnableInput()
        => controller.EnableInput();

    public void GoToScene(string name)
    {
        sceneName = name;
        controller.DisableInput();
        anim.updateMode = AnimatorUpdateMode.UnscaledTime;
        anim.Play("FadeIn");
    }

    // Call from animation
    public void LoadAndUnloadLastScene()
    {
        if (sceneName == null) return;

        AsyncOperation operation = SceneManager.LoadSceneAsync(sceneName);
        StartCoroutine(LoadingAsync(operation, true));
    }

    // Call from animation
    public void LoadAndNotUnloadLastScene()
    {
        if (sceneName == null) return;

        AsyncOperation operation = SceneManager.LoadSceneAsync(sceneName, LoadSceneMode.Additive);
        StartCoroutine(LoadingAsync(operation, false));
    }

    public IEnumerator LoadingAsync(AsyncOperation operation, bool unloadLastScene)
    {
        operation.allowSceneActivation = false;

        while (!operation.isDone)
        {
            float progress = Mathf.Clamp01(operation.progress / 0.9f);

            if (progress >= 0.9f)
            {
                operation.allowSceneActivation = true;
                if (unloadLastScene)
                    SceneManager.LoadScene(sceneName);
            }

            yield return null;
        }
    }

    public void ExitGame()
    {
        Application.Quit();
    }

    public void DisableDuplicatedComponents()
    {
        EventSystem[] eventSystems = Object.FindObjectsByType<EventSystem>(FindObjectsSortMode.None);
        if (eventSystems.Length > 1)
        {
            for (int i = 1; i < eventSystems.Length; i++)
            {
                eventSystems[i].gameObject.SetActive(false);
            }
        }

        AudioListener[] audioListeners = Object.FindObjectsByType<AudioListener>(FindObjectsSortMode.None);
        if (audioListeners.Length > 1)
        {
            for (int i = 1; i < audioListeners.Length; i++)
            {
                audioListeners[i].enabled = false;
            }
        }
    }
}
