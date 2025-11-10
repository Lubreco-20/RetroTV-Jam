using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameState : MonoBehaviour//, IGameState
{
    //public delegate void GameStateDelegate(IGameState state);
    //public static event GameStateDelegate OnSetState;

    public delegate void GameStateSoundDelegate();
    public static event GameStateSoundDelegate OnStopSound;

    [SerializeField] GameObject gameCanvas;

    bool isGamePause;
    bool isEndGame; // Win/GameOver

    public void PauseGame() 
    {
        if (!isGamePause && !isEndGame)
        {
            Time.timeScale = 0f;
            DisableEnableGameCanvas(false);
            isGamePause = true;
        }
    }

    public void ResumeGame()
    {
        if (isGamePause && !isEndGame)
        {
            Time.timeScale = 1f;
            DisableEnableGameCanvas(true);
            isGamePause = false;
        }
    }

    public bool CheckIfGameIsPause()
    {
        return isGamePause;
    }

    public void EndGame()
    {
        Time.timeScale = 0f;
        DisableEnableGameCanvas(false);
        isEndGame = true;

        OnStopSound?.Invoke();
    }

    public void DisableEnableGameCanvas(bool state) => gameCanvas.GetComponent<GraphicRaycaster>().enabled = state;

    void OnEnable()
    {
        //OnSetState?.Invoke(GetComponent<IGameState>());
    }
}
