using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class StartGameController : MonoBehaviour
{
    public void LoadNextScene()
    {
        // Load the next scene by name
        SceneManager.LoadScene("GameStarts");
    }
}
