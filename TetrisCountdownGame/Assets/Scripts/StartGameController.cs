using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace StartGame
{
    public class StartGameController : MonoBehaviour
    {

        public void OnButtonClick()
        {
            // Your code for button click interaction goes here
            LoadNextScene();
        }

        public UnityEngine.UI.Button StartGameButton;

        private void Start()
        {
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
            // Load the next scene by name
            SceneManager.LoadScene("SampleScene");
        }
    }
}
