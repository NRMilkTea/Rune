using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Mockup
{
    public class GameManager : MonoBehaviour
    {// Singleton instance of the GameManager
        public static GameManager Instance { get; private set; }

        // Enum to define game states
        public enum GameState
        {
            EditingGrid,
            OnPlayerTurn,
            OnEnemyTurn,
        }

        // Current game state
        public GameState CurrentState { get; private set; }

        void Awake()
        {
            // Ensure only one instance of GameManager exists
            if (Instance == null)
            {
                Instance = this;
                DontDestroyOnLoad(gameObject);
            }
            else
            {
                Destroy(gameObject);
            }
        }

        void Start()
        {
            // Initialize game
            InitializeGame();
        }

        // Method to initialize the game
        void InitializeGame()
        {
            // Set initial game state
            SetGameState(GameState.EditingGrid);
            // Give initial spells
        }

        // Method to change game state
        public void SetGameState(GameState newState)
        {
            CurrentState = newState;

            // Perform actions based on the new state
            switch (newState)
            {
                case GameState.EditingGrid:
                    // Allow player to edit the grid; only happen between
                    break;
                case GameState.OnPlayerTurn:
                    // Player's turn in a battle; allow player to control the snake
                    break;
                case GameState.OnEnemyTurn:
                    // Enemy's turn in a battle
                    break;
            }
        }
    }
}