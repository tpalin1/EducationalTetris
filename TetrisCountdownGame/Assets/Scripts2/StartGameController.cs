
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;


namespace StartGame
{
    public class StartGameController : MonoBehaviour
    {
        public Button StartGameButton;

        public void Start()

        {


            //Get the startGameButton
            
            Debug.Log("let's goooo");
            // Ensure the button is assigned before using it
            if (StartGameButton != null)
            {
                
                // Attach the click event
                StartGameButton.onClick.AddListener(LoadNextScene);
            }
            else
            {
                Debug.LogError("StartButton not assigned in the Inspector!");
            }
        }

        public void LoadNextScene()
        {
            Debug.Log("Loading next scene...");
            // Load the next scene by name
            SceneManager.LoadScene("MainGame");
        }
    }
}


