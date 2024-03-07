using UnityEngine;
using UnityEngine.SceneManagement;


using GameStateSpace;
using TMPro;
namespace GameOver
{
    public class GameOverController : MonoBehaviour
    {
        public UnityEngine.UI.Button RestartButton;
        public UnityEngine.UI.Button QuitButton;

        //Show score
        public TMP_Text scoreText;

        void Start()
        {


            

            scoreText.text = "Your score was " +GameState.Instance.GetScore().ToString();
            // Ensure the buttons are assigned before using them
            if (RestartButton != null)
            {
                // Attach the click event for restart button
                RestartButton.onClick.AddListener(RestartGame);
            }
            else
            {
                Debug.LogError("RestartButton not assigned in the Inspector!");
            }

            if (QuitButton != null)
            {
                // Attach the click event for quit button
                QuitButton.onClick.AddListener(QuitGame);
            }
            else
            {
                Debug.LogError("QuitButton not assigned in the Inspector!");
            }
        }

       // Modify the RestartGame method in GameOverController to reset the game state
        public void RestartGame()
        {
            // Reset the game state
            GameState.Instance.ResetGame();
            // Load the start scene
            SceneManager.LoadScene("Start");
        }
        public void QuitGame()
        {
#if UNITY_EDITOR
            // Quit the game only in build, not in editor
            UnityEditor.EditorApplication.isPlaying = false;
#else
                        // Quit the application in build
                        Application.Quit();
#endif
        }
    }
}

