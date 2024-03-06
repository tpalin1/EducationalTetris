using System.Collections;
using System.Collections.Generic;
using GameStateSpace;
using Spawner;
using UnityEngine;
using UnityEngine.UIElements;

using TMPro;
using UnityEngine.SceneManagement;





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

    public int currentScore = 0;



    
    public int numberofRowsThisTurn = 0;

    public TMP_Text scoreText;

    public int scoreOneline = 4;
    public int scoreTwoline = 10;
    public int scoreThreeline = 30;
    public int scoreFourline = 60;


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


        scoreText.text = GameState.Instance.GetScore().ToString();
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

                //Check if the block is reached the top
                
                if(isAbove()){

                    
                    GameOver();

                }

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
        
        // // If _canControlMovement is false, we don't want to allow the player to move the block
        // if (!_canControlMovement)
        // {
        //     return;
        // }
        
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
        UpdateScore();
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
    

        // Check if the block is within the grid boundaries
        if (roundedX < 0 || roundedX >= gridWidth || roundedY < 0 || roundedY >= gridHeight)
        {
            return false;
        }

        // Check if the grid cell is already occupied
        if (grid[roundedX, roundedY] != null)
        {
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

        }
    }
            
    void checkForLine(){
        for(int i = gridHeight - 1; i >= 0; i--){
            if(HasLine(i)){
                DeleteLine(i);
                RowDown(i);
                numberofRowsThisTurn++;
                
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



    bool isAbove(){
        for(int i = 0; i < gridWidth; i++){
            if(grid[i, gridHeight-2] != null) {
                return true;
            }
        }
        return false;
    }


    void GameOver(){
        Debug.Log("Game Over");
        SceneManager.LoadScene("GameOver Scene");
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




       public bool CheckIsAboveGrid(Transform block)
    {
        for (int x = 0; x < gridWidth; ++x)
        {
            foreach (Transform child in block)
            {
                Vector2 pos = new Vector2(Mathf.RoundToInt(child.position.x), Mathf.RoundToInt(child.position.y));

                if (pos.y > gridHeight - 1)
                {
                    return true;
                }
            }
        }
        return false;
    }

        public void UpdateUI()
    {
        scoreText.text = currentScore.ToString();

    }
    // score update
    public void UpdateScore()
    {
        if (numberofRowsThisTurn > 0)
        {
            if (numberofRowsThisTurn == 1)
            {



                ClearedOneLine();

            }
            else if (numberofRowsThisTurn == 2)
            {
                ClearedTwoLines();

            }
            else if (numberofRowsThisTurn == 3)
            {
                ClearedThreeLines();

            }
            else if (numberofRowsThisTurn == 4)
            {
                ClearedFourLines();

            }
            numberofRowsThisTurn = 0;
        }
    }

    // methods for scoring
    public void ClearedOneLine()
    {
       
        
        //Increase the gamestate socre

        currentScore = GameState.Instance.GetScore();

        GameState.Instance.SetScore(currentScore += scoreOneline);

        
    }

    public void ClearedTwoLines()
    {
        currentScore = GameState.Instance.GetScore();

        GameState.Instance.SetScore(currentScore += scoreTwoline);
        
    }

    public void ClearedThreeLines()
    {
        currentScore = GameState.Instance.GetScore();

        GameState.Instance.SetScore(currentScore+= scoreThreeline);
    }

    public void ClearedFourLines()
    {

        currentScore = GameState.Instance.GetScore();
        GameState.Instance.SetScore(currentScore += scoreFourline);
        
    }

   

  
}