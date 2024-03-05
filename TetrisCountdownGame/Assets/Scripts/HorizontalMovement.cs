using System.Collections;
using System.Collections.Generic;
using GameStateSpace;
using Spawner;
using UnityEngine;
using UnityEngine.UIElements;

public class HorizontalMovement : MonoBehaviour, IGameStateObserver
{   
    private float previousTime;
    public float fallSpeed = 1.0f; // Speed at which the block falls. Adjust as needed.
    private float timer = 0f;
    public Vector3 rotationPoint;

    public static int gridWidth = 10;
    public static int gridHeight = 20;
    public static Transform[,] grid = new Transform[gridWidth, gridHeight];

    private bool _canControlMovement = false;

    void Start()
    {
        GameState.Instance.Subscribe(this);
    }
    
    public void OnGameStateChanged(GameStateEnum gameState)
    {
        _canControlMovement = gameState == GameStateEnum.TetrisPlayable;
    }

    void Update()
    {
        Debug.Log("This is the grid:");

        for (int y = 0; y < gridHeight; y++)
        {
            string row = "";
            for (int x = 0; x < gridWidth; x++)
            {
                if (grid[x, y] != null)
                    row += "X ";
                else
                    row += "- ";
            }
            Debug.Log(row);
        }

        timer += Time.deltaTime;
        fallSpeed = 1.0f;

        if (_canControlMovement && Input.GetKeyDown(KeyCode.DownArrow))
        {
            fallSpeed = 10.0f;
        }

        if (Time.time - previousTime > (Input.GetKey(KeyCode.DownArrow) ? fallSpeed / 10 : fallSpeed))
        {
            transform.position += new Vector3(0, -1, 0);
            if (!validMove())
            {
                transform.position -= new Vector3(0, -1, 0);
                AddToGrid();
                checkForLine();

                if (IsBlockAtTop())
                {
                    GameState.Instance.SetGameState(GameStateEnum.TetrisGameOver);
                    return;
                }

                this.enabled = false;
                FindObjectOfType<SpawnerForObjects>().SpawnBlock();
                if (_canControlMovement == false)
                {
                    GameState.Instance.SetGameState(GameStateEnum.TetrisGameUnsolved);
                }
                GameState.Instance.SetGameState(GameStateEnum.CountdownBeingSolved);
            }
            previousTime = Time.time;
        }

        if (!_canControlMovement)
        {
            return;
        }
        
        if (Input.GetKeyDown(KeyCode.LeftArrow))
        {
            transform.position += new Vector3(-1, 0, 0);
            if(!validMove())
            {
                transform.position -= new Vector3(-1, 0, 0);
            }
        }
        
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
            Vector3 pos = child.position;
            int roundedX = Mathf.RoundToInt(pos.x);
            int roundedY = Mathf.RoundToInt(pos.y);

            if (roundedX < 0 || roundedX >= gridWidth || roundedY < 0 || roundedY >= gridHeight)
            {
                return false;
            }

            if (grid[roundedX, roundedY] != null)
            {
                return false;
            }
        }
        return true;
    }

    void AddToGrid()
    {
        foreach (Transform child in transform)
        {
            Vector3 pos = child.position;
            int roundedX = Mathf.RoundToInt(pos.x);
            int roundedY = Mathf.RoundToInt(pos.y);

            roundedX = Mathf.Clamp(roundedX, 0, gridWidth - 1);
            roundedY = Mathf.Clamp(roundedY, 0, gridHeight - 1);

            grid[roundedX, roundedY] = child;
        }
    }
        
    void checkForLine()
    {
        for(int i = gridHeight - 2; i >= 0; i--)
        {
            if(HasLine(i))
            {
                DeleteLine(i);
                RowDown(i);
            }
        }
    }

    bool HasLine(int i)
    {
        for(int j = 0; j < gridWidth; j++)
        {
            if(grid[j,i] == null)
            {
                return false;
            }
        }
        return true;
    }

    void DeleteLine(int i)
    {
        for(int j = 0; j < gridWidth; j++)
        {
            Destroy(grid[j,i].gameObject);
            grid[j,i] = null;
        }
    }

    void RowDown(int i)
    {
        for(int y = i; y < gridHeight; y++)
        {
            for(int j = 0; j < gridWidth; j++)
            {
                if(grid[j,y] != null)
                {
                    grid[j,y-1] = grid[j,y];
                    grid[j,y] = null;
                    grid[j,y-1].transform.position -= new Vector3(0,1,0);
                }
            }
        }
    }
}
