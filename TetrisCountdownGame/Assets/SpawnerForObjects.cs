using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnerForObjects : MonoBehaviour
{

    public GameObject[] blocks;

    public bool isFalling = true;
    // Start is called before the first frame update
    void Start()
    {
        SpawnBlock();
    }

    // Update is called once per frame
    void Update()
    {
        



        
        
    }

    public void SpawnBlock()
    {
        // Spawn a random block at the spawner's position, there is Row, Lshape, LRshape, and Square so choose randomly from the list
        // Select a random block from the array
        GameObject blockToSpawn = blocks[Random.Range(0, blocks.Length)];

        // Instantiate the block at the spawner's position
        Instantiate(blockToSpawn, transform.position, Quaternion.identity);

        isFalling = false;


        
    }
}
