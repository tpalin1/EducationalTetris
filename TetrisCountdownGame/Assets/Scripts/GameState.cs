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
        
        /// <summary>
        /// </summary>
        /// <returns>The instance of the GameState singleton</returns>
        public GameState GetInstance()
        {
            if (_instance == null)
            {
                _instance = gameObject.AddComponent<GameState>();
            }

            return _instance;
        }

        /// <summary>
        /// Explicitly returns the current game state
        /// </summary>
        /// <returns></returns>
        public GameStateEnum GetGameState()
        {
            return _gameState;
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

    }

}
