using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;

namespace Spawner
{
    public class SpawnerObjectsTests
    {
        // A Test behaves as an ordinary method
        [Test]
        public void SpawnerObjectsTestsSimplePasses()
        {
            // Use the Assert class to test conditions
        }

        // A UnityTest behaves like a coroutine in Play Mode. In Edit Mode you can use
        // `yield return null;` to skip a frame.
        [UnityTest]
        public IEnumerator SpawnerObjectsTestsWithEnumeratorPasses()
        {
            // Use the Assert class to test conditions.
            // Use yield to skip a frame.
            yield return null;
        }

        [UnityTest]
        public IEnumerator SpawnBlock_UpdatesBlocks()
        {
            // Create a new GameObject with SpawnerForObjects script attached
            GameObject spawnerObject = new GameObject("TestSpawner");
            SpawnerObjects spawner = spawnerObject.AddComponent<SpawnerObjects>();

            // Assign a mock array of blocks to the spawner
            spawner.blocks = new GameObject[] { new GameObject("MockBlock1"), new GameObject("MockBlock2"), new GameObject("MockBlock3") };

            // Initial block spawn
            spawner.SpawnBlock();

            // Wait for one frame to let the instantiation happen
            yield return null;

            // Check if the currentBlock and upcomingBlock are correctly updated
            Assert.NotNull(spawner.currentBlock, "currentBlock is null after initial spawn.");
            Assert.NotNull(spawner.upcomingBlock, "upcomingBlock is null after initial spawn.");

            // Save the currentBlock and upcomingBlock for comparison after the second spawn
            GameObject previousCurrentBlock = spawner.currentBlock;
            GameObject previousUpcomingBlock = spawner.upcomingBlock;

            // Spawn another block
            spawner.SpawnBlock();

            // Wait for one frame to let the instantiation happen
            yield return null;

            // Check if the currentBlock and upcomingBlock are correctly updated after the second spawn
            Assert.NotNull(spawner.currentBlock, "currentBlock is null after second spawn.");
            Assert.NotNull(spawner.upcomingBlock, "upcomingBlock is null after second spawn.");

            // Check if the currentBlock is different from the previous one
            Assert.AreNotEqual(previousCurrentBlock, spawner.currentBlock, "currentBlock is not updated after second spawn.");

            // Check if the upcomingBlock is different from the previous one
            Assert.AreNotEqual(previousUpcomingBlock, spawner.upcomingBlock, "upcomingBlock is not updated after second spawn.");

            // Cleanup
            Object.Destroy(spawnerObject);
        }

    }

}
