using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HorizontalMovement : MonoBehaviour
{
     public float fallSpeed = 1.0f; // Speed at which the block falls. Adjust as needed.
     private float timer = 0f;
    public SpawnerForObjects spawner; // Reference to the spawner script. Set this in the Unity editor.



    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {

         // Make the block fall down constantly
        transform.position += new Vector3(0, -fallSpeed * Time.deltaTime, 0);

        timer += Time.deltaTime;

        if(timer>=10){
            FindObjectOfType<SpawnerForObjects>().SpawnBlock();
            this.enabled = false;
            timer = 0; // Reset the time
        }

        //Spawn a new block when the timer reaches 10 seconds
        
        //If they move arrow left, move the blocks left 1 space
        if (Input.GetKeyDown(KeyCode.LeftArrow))
        {
            transform.position += new Vector3(-1, 0, 0);
        }
        //If they move arrow right, move the blocks right 1 space
        else if (Input.GetKeyDown(KeyCode.RightArrow))
        {
            transform.position += new Vector3(1, 0, 0);
        }
    }
}
