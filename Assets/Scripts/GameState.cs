using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class GameState : MonoBehaviour
{
    [SerializeField] GameObject pauseCanvas;
    [SerializeField] GameObject endGameObj;
    TextMeshProUGUI endGameText;
    Animator endGameAnim;

    bool isGamePause;
    bool isEndGame; // Win/GameOver

    static GameState instance;
    public static GameState Instance { get { return instance; } }

    void Awake()
    {
        if (instance == null) instance = this;

        pauseCanvas.SetActive(false);
        endGameText = endGameObj.GetComponentInChildren<TextMeshProUGUI>();
        endGameAnim = endGameObj.GetComponent<Animator>();
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

    public bool CheckIfIsEndGame()
        => isEndGame;

    public void EndOfTheGame(bool isGameOver)
    {
        Time.timeScale = 0f;
        DisableEnableGameCanvas(false);
        isEndGame = true;
        GetComponent<AudioSource>().Stop();
        GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>().DisableInput();
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
        endGameText.text = isGameOver ? "You Lose!" : "You Win!";
        endGameAnim.SetTrigger("EndGame");
    }

    public void DisableEnableGameCanvas(bool state)
    {
        GraphicRaycaster raycaster = pauseCanvas.GetComponent<GraphicRaycaster>();

        if (raycaster == null) return;

        raycaster.enabled = state;
    }
}