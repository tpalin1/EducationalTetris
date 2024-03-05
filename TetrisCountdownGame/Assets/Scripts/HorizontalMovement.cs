using System.Collections;
using System.Collections.Generic;
using GameStateSpace;
using Spawner;
using UnityEngine;
using UnityEngine.UIElements;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;


public class HorizontalMovement : MonoBehaviour, IGameStateObserver
{   
    private float previousTime;
    public float fallSpeed = 1.0f; // Speed at which the block falls. Adjust as needed.
    private float timer = 0f;
    public Vector3 rotationPoint;

    public static int gridWidth = 10;
    public static int gridHeight = 20;
    public static Transform[,] grid = new Transform[gridWidth, gridHeight];
   
    private int currentScore = 0;
    public int scoreOneline = 40; // scoring rules
    public int scoreTwoline = 200;
    public int scoreThreeline = 300; 
    public int scoreFourline = 1000;
    public Text hud_score;
    private int numberofRowsThisTurn = 0;
    private bool _canControlMovement = false;

    public Text Hud_score { get => hud_score; set => hud_score = value; }

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


    void Start()
    {
        GameState.Instance.Subscribe(this);
    }
    
    public void UpdateUI()
    {
        Hud_score.text = currentScore.ToString();
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
        currentScore += scoreOneline;
    }

    public void ClearedTwoLines()
    {
        currentScore += scoreTwoline;
    }

    public void ClearedThreeLines()
    {
        currentScore += scoreThreeline;
    }

    public void ClearedFourLines()
    {
        currentScore += scoreFourline;
    }

    public void OnGameStateChanged(GameStateEnum gameState)
    {
        if (gameState == GameStateEnum.CountdownBeingSolved)
        {
            GameState.Instance.SetGameState(GameStateEnum.TetrisPlayable);
        }
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


        if (_canControlMovement)
        {
            if (Input.GetKeyDown(KeyCode.DownArrow))
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
        }


        if (_canControlMovement)
        {
            if (Input.GetKeyDown(KeyCode.LeftArrow))
            {
                transform.position += new Vector3(-1, 0, 0);
                if (!validMove())
                {
                    transform.position -= new Vector3(-1, 0, 0);
                }
            }
            else if (Input.GetKeyDown(KeyCode.RightArrow))
            {
                transform.position += new Vector3(1, 0, 0);
                if (!validMove())
                {
                    transform.position -= new Vector3(1, 0, 0);
                }
            }
            else if (Input.GetKeyDown(KeyCode.UpArrow))
            {
                Rotate();
            }
        }

        UpdateScore();
        UpdateUI();

        // Check if the squares are outside the grid
        if (CheckIsAboveGrid(transform))
        {
            FindObjectOfType<HorizontalMovement>().GameOver();
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
        // Since we found a full row, we increment the full roll variable.
        numberofRowsThisTurn++;
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

    bool IsBlockAtTop()
    {
        foreach (Transform child in transform)
        {
            Vector3 pos = child.position;
            int roundedY = Mathf.RoundToInt(pos.y);

            if (roundedY >= gridHeight)
            {
                return true;
            }
        }
        return false;
    }

    public void GameOver()
    {
        SceneManager.LoadScene("GameOver"); 
    }  
}