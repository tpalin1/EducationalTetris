using UnityEngine;
using UnityEngine.SceneManagement;

namespace GameOver
{
    public class GameOverController : MonoBehaviour
    {
        public UnityEngine.UI.Button RestartButton;
        public UnityEngine.UI.Button QuitButton;

        void Start()
        {
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

        public void RestartGame()
        {
            // Restart the game by loading the MainGame scene
            SceneManager.LoadScene("MainGame");
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

