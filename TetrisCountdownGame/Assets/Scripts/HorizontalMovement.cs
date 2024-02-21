using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class HorizontalMovement : MonoBehaviour
{   
    private float previousTime;
     public float fallSpeed = 1.0f; // Speed at which the block falls. Adjust as needed.
     private float timer = 0f;
     public Vector3 rotationPoint;
    public SpawnerForObjects spawner; // Reference to the spawner script. Set this in the Unity editor.


    public static int gridWidth = 10;
    public static int gridHeight = 20;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {

        

        timer += Time.deltaTime;

        if(Time.time - previousTime > (Input.GetKey(KeyCode.DownArrow)? fallSpeed /10 : fallSpeed)){
            transform.position += new Vector3(0, -1, 0);
            if(!validMove()){
                transform.position -= new Vector3(0,-1, 0);
                
            }
            previousTime = Time.time;
        }



        //Spawn a new block when the timer reaches 10 seconds
        
        //If they move arrow left, move the blocks left 1 space
        if (Input.GetKeyDown(KeyCode.LeftArrow))
        {
            transform.position += new Vector3(-1, 0, 0);
            if(!validMove()){
                transform.position -= new Vector3(-1, 0, 0);
            }
        }
        //If they move arrow right, move the blocks right 1 space
        else if (Input.GetKeyDown(KeyCode.RightArrow))
        {
            transform.position += new Vector3(1, 0, 0);
            if(!validMove()){
                transform.position -= new Vector3(1, 0, 0);
            }
        }
        else if (Input.GetKeyDown(KeyCode.UpArrow))
        {
            Rotate();
        }
    }

     void Rotate()
    {
        Vector3 pivot = GetComponent<SpriteRenderer>().bounds.center;
        transform.RotateAround(pivot, Vector3.forward, 90f);
    }

    //Allow block collisions so that they can stack on one another

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Block")
        {
            FindObjectOfType<SpawnerForObjects>().SpawnBlock();
            Debug.Log("Block has collided with another block");
            this.enabled = false;
        }
    }

   //Do is valid move for the bottom and the left and right
    bool validMove()
    {
        foreach (Transform children in transform)
        {
            int roundedX = Mathf.RoundToInt(children.transform.position.x);
            int roundedY = Mathf.RoundToInt(children.transform.position.y);

            Debug.Log("This is x" + roundedX);
            Debug.Log("This is Y"+ roundedY);
           
            if (roundedX < -7 || roundedX >= gridWidth+6 || roundedY <-15)
            {
                Debug.Log("Invalid move");
                return false;
            }
        }
        return true;
    }
}

