using UnityEngine;
using UnityEngine.SceneManagement;

namespace Return
{
<<<<<<< HEAD
    public class ReturnController : MonoBehaviour
=======
    public class Return : MonoBehaviour
>>>>>>> d5efaacbfed3774e36a608823f838f6a6d835291
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
            SceneManager.LoadScene("StartGameScene");
        }
    }
}