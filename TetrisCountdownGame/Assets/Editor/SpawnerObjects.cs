using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace Spawner
{
  public class SpawnerObjects : MonoBehaviour
  {

    public GameObject[] blocks;
    public bool isFalling = true;
    public GameObject upcomingBlock;
    public GameObject currentBlock;
    private bool _isFirstBlock = true;

    // Start is called before the first frame update
    void Start()
    {
      SpawnBlock();
    }

    // Update is called once per frame
    void Update()
    {

    }

    /// <summary>
    /// Used when a new block should be spawned. Updates the current block and upcoming block.
    /// </summary>
    public void SpawnBlock()
    {
      if (_isFirstBlock)
      {
        currentBlock = GetRandomBlock();
      }
      else
      {
        currentBlock = upcomingBlock;
      }
      upcomingBlock = GetRandomBlock();

      // Instantiate the block at the spawner's position
      Instantiate(currentBlock, transform.position, Quaternion.identity);

      isFalling = false;
    }

    private GameObject GetRandomBlock()
    {
      // Spawn a random block at the spawner's position, there is Row, Lshape, LRshape, and Square so choose randomly from the list
      // Select a random block from the array
      return blocks[Random.Range(0, blocks.Length)];
    }
  }
}

