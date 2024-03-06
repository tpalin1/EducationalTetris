using UnityEngine;
using UnityEngine.SceneManagement;

namespace GameOver
{
    public class GameOverController : MonoBehaviour
    {
        public UnityEngine.UI.Button ReturnButton;

        void Start()
        {
            if (ReturnButton != null)
            {
                // Attach the click event for return button
                ReturnButton.onClick.AddListener(ReturnToStart);
            }
            else
            {
                Debug.LogError("ReturnButton not assigned in the Inspector!");
            }
        }

        // Define method to return to start scene
        public void ReturnToStart()
        {
            // Load the start scene
            SceneManager.LoadScene("Start");
        }
    }
}