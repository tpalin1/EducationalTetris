using System.Collections;
using System.Collections.Generic;
using GameStateSpace;
using Spawner;
using UnityEngine;
using UnityEngine.UIElements;
using GameStateSpace;

public class HorizontalMovement : MonoBehaviour, IGameStateObserver
{   
    private float previousTime;
     public float fallSpeed = 1.0f; // Speed at which the block falls. Adjust as needed.
     private float timer = 0f;
     public Vector3 rotationPoint;

    public static int gridWidth = 10;
    public static int gridHeight = 20;
    // Start is called before the first frame update
     public static Transform[,] grid = new Transform[gridWidth, gridHeight];

     private bool _canControlMovement = false;


    void Start()
    {
        //subscribe to game state
        GameState.Instance.Subscribe(this);
        
    }
    
    public void OnGameStateChanged(GameStateEnum gameState)
    {
        _canControlMovement = gameState == GameStateEnum.TetrisPlayable;
    }

    // Update is called once per frame
    void Update()
    {

        Debug.Log("This is the grid" + grid);

        

        timer += Time.deltaTime;

       fallSpeed = 1.0f;

       if(_canControlMovement && Input.GetKeyDown(KeyCode.DownArrow)){
           fallSpeed = 10.0f;   
       }
        

        

        if(Time.time - previousTime > (Input.GetKey(KeyCode.DownArrow)? fallSpeed /10 : fallSpeed)){
            transform.position += new Vector3(0, -1, 0);
            if(!validMove()){

                transform.position -= new Vector3(0,-1, 0);
                AddToGrid();
                checkForLine();

                // when the block has fallen and landed, spawn a new one and update game state to not playable
                this.enabled = false;
                FindObjectOfType<SpawnerForObjects>().SpawnBlock();  
                if(_canControlMovement == false) {
                    GameState.Instance.SetGameState(GameStateEnum.TetrisGameUnsolved);
                    
                }
                GameState.Instance.SetGameState(GameStateEnum.CountdownBeingSolved);              
            }
            previousTime = Time.time;
        }



        //Spawn a new block when the timer reaches 10 seconds
        
        // If _canControlMovement is false, we don't want to allow the player to move the block
        if (!_canControlMovement)
        {
            return;
        }
        
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
        transform.RotateAround(transform.TransformPoint(rotationPoint), new Vector3(0, 0, 1), 90);
        if (!validMove())
        {
            transform.RotateAround(transform.TransformPoint(rotationPoint), new Vector3(0, 0, 1), -90);
        }
    }



   bool validMove()
{
    foreach (Transform child in transform)
    {
        int roundedX = Mathf.RoundToInt(child.position.x);
        int roundedY = Mathf.RoundToInt(child.position.y);

        // Debug logs for debugging
        Debug.Log("Child position: " + child.position);
        Debug.Log("Rounded position: (" + roundedX + ", " + roundedY + ")");

        // Check if the block is within the grid boundaries
        if (roundedX < 0 || roundedX >= gridWidth || roundedY < 0 || roundedY >= gridHeight)
        {
            Debug.Log("Block is out of bounds");
            return false;
        }

        // Check if the grid cell is already occupied
        if (grid[roundedX, roundedY] != null)
        {
            Debug.Log("Grid cell (" + roundedX + ", " + roundedY + ") is occupied");
            return false;
        }
    }
    return true;
}


    void AddToGrid()
    {
        foreach (Transform children in transform)
        {
            int roundedX = Mathf.RoundToInt(children.transform.position.x);
            int roundedY = Mathf.RoundToInt(children.transform.position.y);
            grid[roundedX, roundedY] = children;

            Debug.Log("This is where it got added to the grid"+ roundedX + " " + roundedY);
        }
    }
            
    void checkForLine(){
        for(int i = gridHeight - 1; i >= 0; i--){
            if(HasLine(i)){
                DeleteLine(i);
                RowDown(i);
            }
        }
    }

    bool HasLine(int i){
        for(int j = 0; j < gridWidth; j++){
            if(grid[j,i] == null){
                return false;
            }
        }
        return true;
    }

    void DeleteLine(int i){
        for(int j = 0; j < gridWidth; j++){
            Destroy(grid[j,i].gameObject);
            grid[j,i] = null;
        }
    }

    void RowDown(int i){
        for(int y = i; y < gridHeight; y++){
            for(int j = 0; j < gridWidth; j++){
                if(grid[j,y] != null){
                    grid[j,y-1] = grid[j,y];
                    grid[j,y] = null;
                    grid[j,y-1].transform.position -= new Vector3(0,1,0);
                }
            }
        }
    }
}

