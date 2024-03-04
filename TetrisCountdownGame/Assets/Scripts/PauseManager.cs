using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using GameStateSpace;

public class PauseManager : MonoBehaviour
{
    public GameObject pauseMenu;
    public GameStateEnum prevState;
    public Button pause;

    void Start()
    {
        Time.timeScale = 1f;
        SetPauseMenuActive(false);
        // Add a listener to the button click event
        // Add a listener to the button click event
        pause.onClick.AddListener(TogglePause);

    }



    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            TogglePause();
        }

        

    }

    public void TogglePause()
    {
        if (Time.timeScale == 0f)
        {
            ResumeGame();
        }
        else
        {
            PauseGame();
        }
    }

    void PauseGame()
    {
        prevState = GameState.Instance.GetGameState();
        Time.timeScale = 0f; // Pause the game
        SetPauseMenuActive(true);

        // Update game state based on your logic
        if (prevState == GameStateEnum.TetrisPlayable)
        {
            GameState.Instance.SetGameState(GameStateEnum.TetrisPlayableButPaused);
        }
        else if (prevState == GameStateEnum.CountdownBeingSolved)
        {
            GameState.Instance.SetGameState(GameStateEnum.CountdownBeingSolvedButPaused);
        }
    }

    void ResumeGame()
    {
         prevState = GameState.Instance.GetGameState();
        Time.timeScale = 1f; // Resume the game
        SetPauseMenuActive(false);

        if(prevState == GameStateEnum.TetrisPlayableButPaused){
              GameState.Instance.SetGameState(GameStateEnum.TetrisPlayable);

        }
        if(prevState == GameStateEnum.CountdownBeingSolvedButPaused){
            GameState.Instance.SetGameState(GameStateEnum.CountdownBeingSolved);
        }

        // Reset game state to its previous state
        GameState.Instance.SetGameState(prevState);
    }

    void SetPauseMenuActive(bool isActive)
    {
        if (pauseMenu != null)
        {
            pauseMenu.SetActive(isActive);
        }
    }
}
