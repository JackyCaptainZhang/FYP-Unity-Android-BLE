using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DifficultyManager : MonoBehaviour
{
    public static ROM_GameSettings ROM_Game = new ROM_GameSettings();
    public static EMG_GameSettings EMG_Game = new EMG_GameSettings();

    public static bool GameMode = false;

    // ROM_GameSettings Class
    public class ROM_GameSettings
    {
        private float difficultyLevel = 1.0f;

        public void Harder()
        {
            if (GameMode)
            {
                difficultyLevel += 0.1f;
                if (difficultyLevel > 1.5f)
                {
                    difficultyLevel = 1.5f;
                }
                Debug.Log($"ROM Game Difficulty Increased to: {difficultyLevel}");
                // Implement the logic to make the game harder
            }
        }

        public void Easier()
        {
            if (GameMode)
            {
                difficultyLevel -= 0.1f;
                if (difficultyLevel < 0.5f)
                {
                    difficultyLevel = 0.5f;
                }
                Debug.Log($"ROM Game Difficulty Decreased to: {difficultyLevel}");
                // Implement the logic to make the game easier
            }
        }

        public float GetDifficultyLevel()
        {
            return difficultyLevel;
        }
    }

    // EMG_GameSettings Class
    public class EMG_GameSettings
    {
        private float difficultyLevel = 1.0f;

        public void Harder()
        {
            if (GameMode)
            {
                difficultyLevel += 0.1f;
                if (difficultyLevel > 1.5f)
                {
                    difficultyLevel = 1.5f;
                }
                Debug.Log($"EMG Game Difficulty Increased to: {difficultyLevel}");
                // Implement the logic to make the game harder
            }
        }

        public void Easier()
        {
            if (GameMode)
            {
                difficultyLevel -= 0.1f;
                if (difficultyLevel < 0.5f)
                {
                    difficultyLevel = 0.5f;
                }
                Debug.Log($"EMG Game Difficulty Decreased to: {difficultyLevel}");
                // Implement the logic to make the game easier
            }
        }

        public float GetDifficultyLevel()
        {
            return difficultyLevel;
        }
    }
}
