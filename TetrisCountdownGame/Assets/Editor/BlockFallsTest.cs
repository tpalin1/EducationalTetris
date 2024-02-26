using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;

namespace HorizontalMovementTest
{
    public class BlockFallsTest
    {
        [UnityTest]
        public IEnumerator BlockMovesDownOnTimer()
        {
            // Instantiate the GameObject with HorizontalMovement script
            GameObject tetrisBlock = new GameObject();
            tetrisBlock.AddComponent<BlockBehaviour>();

            // Store the initial position
            Vector3 initialPosition = tetrisBlock.transform.position;

            // Wait for a short time to simulate the passage of time
            yield return new WaitForSeconds(1.0f);

            // Check if the block has moved down
            Assert.Less(tetrisBlock.transform.position.y, initialPosition.y);
        }

        // [UnityTest]
        // public IEnumerator BlockMovesLeftOnLeftArrow()
        // {
        //     // Instantiate the GameObject with HorizontalMovement script
        //     GameObject tetrisBlock = new GameObject();
        //     BlockBehaviour horizontalMovement = tetrisBlock.AddComponent<BlockBehaviour>();

        //     // Store the initial position
        //     Vector3 initialPosition = tetrisBlock.transform.position;

        //     // Simulate pressing the left arrow key
        //     Input.GetKeyDown(KeyCode.LeftArrow);

        //     // Wait for a short time to allow for movement
        //     yield return new WaitForSeconds(0.1f);

        //     // Check if the block has moved left
        //     Assert.Less(tetrisBlock.transform.position.x, initialPosition.x);
        // }

        // [UnityTest]
        // public IEnumerator BlockMovesRightOnRightArrow()
        // {
        //     // Instantiate the GameObject with HorizontalMovement script
        //     GameObject tetrisBlock = new GameObject();
        //     BlockBehaviour horizontalMovement = tetrisBlock.AddComponent<BlockBehaviour>();

        //     // Store the initial position
        //     Vector3 initialPosition = tetrisBlock.transform.position;

        //     // Simulate pressing the right arrow key
        //     Input.GetKeyDown(KeyCode.RightArrow);

        //     // Wait for a short time to allow for movement
        //     yield return new WaitForSeconds(0.1f);

        //     // Check if the block has moved right
        //     Assert.Greater(tetrisBlock.transform.position.x, initialPosition.x);
        // }
    }

}
