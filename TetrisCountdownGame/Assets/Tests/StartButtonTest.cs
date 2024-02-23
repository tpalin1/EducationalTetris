using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.TestTools;
using UnityEngine.SceneManagement;
using UnityEditor.SceneManagement;
public class StartButtonTest
{
    [UnityTest]
    public IEnumerator StartButton_LoadsNextScene()
    {
        // Open the start game scene using EditorSceneManager
        EditorSceneManager.OpenScene("Assets/Scenes/SampleScene.unity");

        // Wait for one frame to let the scene load
        yield return null;

        // Find the StartButton in the scene
        var startButtonGameObject = GameObject.Find("StartGameButton");

        // Check if the GameObject is found
        Assert.NotNull(startButtonGameObject, "StartButton GameObject not found in the scene.");

        // Get the Button component
        var startButton = startButtonGameObject.GetComponent<UnityEngine.UI.Button>();

        // Check if the Button component is found
        Assert.NotNull(startButton, "Button component not found on StartGameButton GameObject.");

        // Trigger the button click event
        startButton.onClick.Invoke();

        // Wait for one frame to let the next scene load
        yield return null;

        // Assert that the current scene is now "NextGame"
        Assert.AreEqual("NextGame", UnityEngine.SceneManagement.SceneManager.GetActiveScene().name);
    }
}