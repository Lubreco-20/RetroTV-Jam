using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class GameState : MonoBehaviour
{
    [SerializeField] GameObject pauseCanvas;

    bool isGamePause;
    bool isEndGame; // Win/GameOver

    static GameState instance;
    public static GameState Instance { get { return instance; } }

    void Awake()
    {
        if (instance == null) instance = this;

        pauseCanvas.SetActive(false);
    }

    public void PauseResume(InputAction.CallbackContext ctx)
    {
        if (Time.timeScale == 1f)
            PauseGame();
        else
            ResumeGame();
    }

    public void PauseGame() 
    {
        if (!isGamePause && !isEndGame)
        {
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;

            Time.timeScale = 0f;
            DisableEnableGameCanvas(false);
            pauseCanvas.SetActive(true);
            isGamePause = true;
        }
    }

    public void ResumeGame()
    {
        if (isGamePause && !isEndGame)
        {
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;

            Time.timeScale = 1f;
            DisableEnableGameCanvas(true);
            pauseCanvas.SetActive(false);
            isGamePause = false;
        }
    }

    public bool CheckIfGameIsPause()
        => isGamePause;

    public void GameOver()
    {
        Time.timeScale = 0f;
        DisableEnableGameCanvas(false);
        isEndGame = true;
    }

    public void DisableEnableGameCanvas(bool state)
    {
        GraphicRaycaster raycaster = pauseCanvas.GetComponent<GraphicRaycaster>();

        if (raycaster == null) return;

        raycaster.enabled = state;
    }
}