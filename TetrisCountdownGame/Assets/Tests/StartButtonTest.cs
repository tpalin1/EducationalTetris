using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.TestTools;
using UnityEngine.SceneManagement;
using UnityEditor.SceneManagement;
using StartGameTest;
public class StartButtonTest
{
    [Test]
    public void StartButtonAssigned_Click_LoadsNextScene()
    {
        // Instantiate the GameObject with StartGameController script
        GameObject gameObject = new GameObject();
        StartGame startGameController = gameObject.AddComponent<StartGame>();

        // Add a Button component to the GameObject
        UnityEngine.UI.Button button = gameObject.AddComponent<Button>();

        // Assign the Button component to the StartGameButton field
        startGameController.StartGameButton = button;

        // Call Start method (this would typically be called automatically, but in edit mode tests, you might need to call it explicitly)
        startGameController.Start();

        // Simulate a button click by invoking the onClick event
        button.onClick.Invoke();

        // Verify that the next scene is loaded
        Assert.AreEqual("SampleScene", EditorSceneManager.GetActiveScene().name);
    }

    [Test]
    public void MissingStartButton_LogsError()
    {
        // Instantiate the GameObject with StartGameController script
        GameObject gameObject = new GameObject();
        StartGame startGameController = gameObject.AddComponent<StartGame>();

        // Call Start method (this would typically be called automatically, but in edit mode tests, you might need to call it explicitly)
        startGameController.Start();

        // Use LogAssert to handle expected log messages
        LogAssert.Expect(LogType.Error, "StartButton not assigned in the Inspector!");

        // Simulate a button click (this is not expected to happen in this test)
        // This would typically happen if StartGameButton is not assigned

        // Remove the LogAssert expectation
        LogAssert.NoUnexpectedReceived();
    }
}