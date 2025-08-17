using UnityEngine;
using SQLite;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using System;

namespace Databases
{
    /// Game Data Manager for handling SQLite database operations
    public class GameDataManager : MonoBehaviour
    {
        [Header("Database Configuration")]
        [SerializeField] private string databaseName = "GameData.db";
        
        private SQLiteConnection _database;
        private string _databasePath;
        
        // Singleton pattern for easy access
        public static GameDataManager Instance { get; private set; }
        
        private void Awake()
        {
            if (Instance == null)
            {
                Instance = this;
                DontDestroyOnLoad(gameObject);
                InitializeDatabase();
            }
            else
            {
                Destroy(gameObject);
            }
        }
        
        /// TODO: Students will implement this method
        private void InitializeDatabase()
        {
            try
            {
                // TODO: Set up database path using Application.persistentDataPath
                _databasePath = System.IO.Path.Combine(Application.persistentDataPath, databaseName); // Unity finds right path to read and write data.
                
                // TODO: Create SQLite connection
                _database = new SQLiteConnection(_databasePath); // open connection if there is a database otherwise create database and a new connection. 
                // TODO: Create tables for game data
                _database.CreateTable<HighScore>(); //create a highscore table if dosen't exists, otherwise break;. 

                Debug.Log($"Database initialized at: {_databasePath}");
            }
            catch (Exception ex)
            {
                Debug.LogError($"Failed to initialize database: {ex.Message}");
            }
        }
        
        #region High Score Operations
        
        /// TODO: Students will implement this method
        public void AddHighScore(string playerName, int score, string levelName = "Default")
        {
            try
            {
                // TODO: Create a new HighScore object
                var highScore = new HighScore(playerName, score, levelName);
                // TODO: Insert it into the database using _database.Insert()
                _database.Insert(highScore);
                
                Debug.Log($"High score added: {playerName} - {score} points");
            }
            catch (Exception ex)
            {
                Debug.LogError($"Failed to add high score: {ex.Message}");
            }
        }
        
        /// TODO: Students will implement this method
        public List<HighScore> GetTopHighScores(int limit = 10)
        {
            try
            {
                // TODO: Query the database for top scores
                var scores = _database.Table<HighScore>()
                    .OrderByDescending(x => x.Score)
                    .Take(limit)
                    .ToList();
                
                //return new List<HighScore>(); // Placeholder - students will replace this
                return scores;
            }
            catch (Exception ex)
            {
                Debug.LogError($"Failed to get high scores: {ex.Message}");
                return new List<HighScore>();
            }
        }
        
        /// TODO: Students will implement this method
        public List<HighScore> GetHighScoresForLevel(string levelName, int limit = 10)
        {
            try
            {
                // TODO: Query the database for scores filtered by level
                var scores = _database.Table<HighScore>()
                    .Where(x => x.LevelName == levelName)
                    .OrderByDescending(x => x.Score)
                    .Take(limit)
                    .ToList();

                //return new List<HighScore>(); // Placeholder - students will replace this
                return scores;
            }
            catch (Exception ex)
            {
                Debug.LogError($"Failed to get level high scores: {ex.Message}");
                return new List<HighScore>();
            }
        }
        
        #endregion
        
        #region Database Utility Methods
        
        /// TODO: Students will implement this method
        public int GetHighScoreCount()
        {
            try
            {
                // TODO: Count the total number of high scores
                return _database.Table<HighScore>()
                    .Count(); //returning int based on how many rows in Highscore.
                
                // return 0; // Placeholder - students will replace this
            }
            catch (Exception ex)
            {
                Debug.LogError($"Failed to get high score count: {ex.Message}");
                return 0;
            }
        }
        
        /// TODO: Students will implement this method
        public void ClearAllHighScores()
        {
            try
            {
                // TODO: Delete all high scores from the database
                _database.DeleteAll<HighScore>();
                
                Debug.Log("All high scores cleared");
            }
            catch (Exception ex)
            {
                Debug.LogError($"Failed to clear high scores: {ex.Message}");
            }
        }
        
        /// <summary>
        /// Close the database connection when the application quits
        /// </summary>
        private void OnApplicationQuit()
        {
            _database?.Close();
        }
        
        #endregion
    }
}