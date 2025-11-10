using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class SceneLoaderManager : MonoBehaviour//, ISceneLoader
{
    string sceneName;

    [SerializeField] GameObject loadingScreen;
    [SerializeField] Slider slider;
    [SerializeField] GameObject fader;
    Animator anim;

    //ISound sfx;

    static SceneLoaderManager inst;

    private void Awake()
    {
        DisableDuplicatedComponents();

        if (inst == null) inst = this;
        else Destroy(gameObject);
    }

    private void Start()
    {
        loadingScreen.SetActive(false);
        //sfx = GetComponent<ISound>();
        anim = fader.GetComponent<Animator>();
        anim.Play("FadeOut");
    }

    public void GoToScene(string name)
    {
        sceneName = name;
        anim.Play("FadeIn");
    }

    public void LoadAndUnloadLastScene()
    {
        if (sceneName == null) return;

        AsyncOperation operation = SceneManager.LoadSceneAsync(sceneName);
        StartCoroutine(LoadingAsync(operation, true));
    }

    public void LoadAndNotUnloadLastScene()
    {
        if (sceneName == null) return;

        AsyncOperation operation = SceneManager.LoadSceneAsync(sceneName, LoadSceneMode.Additive);
        StartCoroutine(LoadingAsync(operation, false));
    }

    public IEnumerator LoadingAsync(AsyncOperation operation, bool unloadLastScene)
    {
        operation.allowSceneActivation = false;
        loadingScreen.SetActive(true);

        while (!operation.isDone)
        {
            float progress = Mathf.Clamp01(operation.progress / 0.9f);
            slider.value = progress;

            if (progress >= 0.9f)
            {
                operation.allowSceneActivation = true;
                loadingScreen.SetActive(false);
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

    public void DisableDuplicatedComponents() //FindObjectsOfType
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
