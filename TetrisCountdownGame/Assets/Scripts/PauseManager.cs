using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using GameStateSpace;
using UnityEngine.SceneManagement;

public class PauseManager : MonoBehaviour
{
    public GameObject pauseMenu;
    public GameStateEnum prevState;
    public Button pause;

    public TextMeshProUGUI pauseButtonText;

    //aDD TMP for the button
    


    void Start()
    {
        Time.timeScale = 1f;
        SetPauseMenuActive(false);
        // Add a listener to the button click event
        // Add a listener to the button click event
        //Get the pause button text
        pauseButtonText = pause.GetComponentInChildren<TextMeshProUGUI>();

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

         pauseButtonText.text = "Resume"; // Change the button text back to "Pause"

        prevState = GameState.Instance.GetGameState();
        Time.timeScale = 0f; // Pause the game
        SetPauseMenuActive(true);
// Deactivate the selectables
    ProblemSetController.Instance.buttonContainer.gameObject.SetActive(false);
    ProblemSetController.Instance.operatorButtonContainer.gameObject.SetActive(false);

        



        // Update game state based on your logic
        if (prevState == GameStateEnum.TetrisPlayable)
        {
            GameState.Instance.SetGameState(GameStateEnum.TetrisPlayableButPaused);
        }
        else if (prevState == GameStateEnum.CountdownBeingSolved)
        {
            GameState.Instance.SetGameState(GameStateEnum.CountdownBeingSolvedButPaused);
        }


       


        //Change the button text to resume
        

    }

    void ResumeGame()
    {

         pauseButtonText.text = "Pause"; // Change the button text back to "Pause"

         prevState = GameState.Instance.GetGameState();
        Time.timeScale = 1f; // Resume the game
        SetPauseMenuActive(false);


        // Activate the selectables
        ProblemSetController.Instance.buttonContainer.gameObject.SetActive(true);
        ProblemSetController.Instance.operatorButtonContainer.gameObject.SetActive(true);

        if(prevState == GameStateEnum.TetrisPlayableButPaused){
              GameState.Instance.SetGameState(GameStateEnum.TetrisPlayable);

        }
        if(prevState == GameStateEnum.CountdownBeingSolvedButPaused){
            GameState.Instance.SetGameState(GameStateEnum.CountdownBeingSolved);
        }

    }

    void SetPauseMenuActive(bool isActive)
    {
        if (pauseMenu != null)
        {
            pauseMenu.SetActive(isActive);
        }
    }
}
