using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;
using UnityEngine.SceneManagement;

public class StartButtonTest
{
    [UnityTest]
    public IEnumerator StartButton_LoadsNextScene()
    {
        // Load the start game scene
        SceneManager.LoadScene("GameStarts");

        // Wait for one frame to let the scene load
        yield return null;

        // Find the StartButton in the scene
        var startButton = GameObject.Find("StartGameButton").GetComponent<UnityEngine.UI.Button>();

        // Trigger the button click event
        startButton.onClick.Invoke();

        // Wait for one frame to let the next scene load
        yield return null;

        // Assert that the current scene is now "NextGame"
        Assert.AreEqual("NextGame", SceneManager.GetActiveScene().name);
    }
}
