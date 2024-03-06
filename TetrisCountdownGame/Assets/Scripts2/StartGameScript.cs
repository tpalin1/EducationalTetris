using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;


namespace StartGame{

 public class StartGameScript : MonoBehaviour
    {
        public Button startGameButton;

        public void Start()

        {


            //Get the startGameButton
            
            Debug.Log("let's goooo");
            // Ensure the button is assigned before using it
            if (startGameButton != null)
            {
                // Attach the click event
                startGameButton.onClick.AddListener(LoadNextScene);
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

