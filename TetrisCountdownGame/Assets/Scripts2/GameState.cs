using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GameStateSpace
{
    /// <summary>
    /// GameStateEnum represents the different states that the game can be in.
    ///
    /// When the game is loaded, we begin in game not started. When they press "start game" we move to
    /// CountDownBeingSolved.
    /// </summary>
    public enum GameStateEnum
    {
        GameNotStarted,
        TetrisPlayable,
        TetrisPlayableButPaused,
        TetrisGameOver,
        CountdownBeingSolved,

        TetrisGameUnsolved,
        CountdownBeingSolvedButPaused,
    }
    
    /// <summary>
    /// Scripts that want to be notified when the state changes should implement this interface.
    /// </summary>
    public interface IGameStateObserver
    {
        void OnGameStateChanged(GameStateEnum gameState);
    }
    

    /// <summary>
    /// The GameState class is responsible for keeping track of the current state of the game.
    ///
    /// It is a singleton.
    /// </summary>
    public class GameState : MonoBehaviour
    {
        private static GameState _instance;
        private GameStateEnum _gameState = GameStateEnum.GameNotStarted;
        private List<IGameStateObserver> _observers = new List<IGameStateObserver>();

        // Define gridWidth and gridHeight
        private const int gridWidth = 10;
        private const int gridHeight = 20;

        // Define grid array
        private Transform[,] grid = new Transform[gridWidth, gridHeight];

        //Score
        public int currentScore = 0;

        public static GameState Instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = FindObjectOfType<GameState>();
                    if (_instance == null)
                    {
                        GameObject gameObject = new GameObject();
                        _instance = gameObject.AddComponent<GameState>();
                        DontDestroyOnLoad(gameObject);
                    }
                }
                return _instance;
            }
        }


        
        public void Awake()
        {
            if (_instance == null)
            {
                _instance = this;
                DontDestroyOnLoad(gameObject);
            }
            else
            {
                Destroy(gameObject);
            }
        }

        // Add a method in GameState class to reset the game state
        public void ResetGame()
        {
            SetScore(0);
            SetGameState(GameStateEnum.GameNotStarted);
            // Clear the grid
            for (int x = 0; x < gridWidth; x++)
            {
                for (int y = 0; y < gridHeight; y++)
                {
                    grid[x, y] = null;
                }
            }
            
        }

        /// <summary>
        /// Explicitly returns the current game state
        /// </summary>
        /// <returns></returns>
        public GameStateEnum GetGameState()
        {
            return _gameState;
        }

        public void SetScore(int score)
        {
            currentScore = score;
        }

        public int GetScore()
        {
            return currentScore;
        }

        /// <summary>
        /// Update the game state and notify all the listeners of this change.
        /// </summary>
        /// <param name="gameStateEnum"></param>
        public void SetGameState(GameStateEnum gameStateEnum)
        {
            _gameState = gameStateEnum;
            
            // notify listeners
            foreach (IGameStateObserver observer in _observers)
            {
                observer.OnGameStateChanged(gameStateEnum);
            }
        }

        /// <summary>
        /// Called by a script that wants to be notified of game state changes.
        ///
        /// Used as such:
        ///   GameState gameState = GameState.GetInstance();
        ///   gameState.Subscribe(this);
        /// </summary>
        /// <param name="observer"></param>
        public void Subscribe(IGameStateObserver observer)
        {
            if (!_observers.Contains(observer))
            {
                _observers.Add(observer);
            }
        }
        
        /// <summary>
        /// Called by a script if it does not want to be notified of game state changes anymore.
        /// </summary>
        /// <param name="observer"></param>
        public void UnSubscribe(IGameStateObserver observer)
        {
            if (_observers.Contains(observer))
            {
                _observers.Remove(observer);
            }
        }

        public bool IsBlockAtTop()
        {
            // Check if the top row of the grid contains any blocks
            for (int x = 0; x < gridWidth; x++)
            {
                if (grid[x, gridHeight - 1] != null)
                {
                    // If a block is found in the top row, return true
                    return true;
                }
            }
            // If no block is found in the top row, return false
            return false;
        }
    }
}