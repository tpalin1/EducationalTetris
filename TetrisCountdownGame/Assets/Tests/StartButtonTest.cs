using System.Collections;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;
using UnityEngine.SceneManagement;
using StartGameTest;
public class StartButtonTest
{
    [TestFixture]
    public class StartGameTests
    {
        [UnityTest]
        public IEnumerator StartButton_LoadsNextScene()
        {
            // Arrange
            var gameObject = new GameObject();
            var startGameScript = gameObject.AddComponent<StartGame>();

            // Create a button and assign it to the script
            var buttonGameObject = new GameObject();
            var button = buttonGameObject.AddComponent<UnityEngine.UI.Button>();
            startGameScript.StartGameButton = button;

            // Act
            startGameScript.Start(); // Simulate Start method

            // Simulate button click
            button.onClick.Invoke();

            // Yield to the next frame to allow SceneManager.LoadScene to take effect
            yield return null;

            // Assert
            Assert.AreEqual("MainGame", SceneManager.GetActiveScene().name);

            // Clean up
            Object.Destroy(gameObject);
            Object.Destroy(buttonGameObject);
        }

        [UnityTest]
        public IEnumerator StartButton_NotAssigned_ShowError()
        {
            // Arrange
            var gameObject = new GameObject();
            var startGameScript = gameObject.AddComponent<StartGame>();

            // Act
            startGameScript.Start(); // Simulate Start method

            // Assert
            LogAssert.Expect(LogType.Error, "StartButton not assigned in the Inspector!");

            // Clean up
            Object.Destroy(gameObject);

            // Yield to the next frame to process error logs
            yield return null;
        }
    }
}