using UnityEngine;
using UnityEngine.UI;

public class DifficultyManager : MonoBehaviour
{
    public static bool inROMGame;
    public static bool inEMGGame;
    public static int difficultyLevel;
    public Text ROMDifficultyText;

    private float maxSpawnRate = 5.0f;
    private float minSpawnRate = 2.0f;
    private float maxMoveSpeed = 30f;
    private float minMoveSpeed = 10f;
    private int scoreIncrement = 5;
    private int gameoverIncrement = 2;
    private bool increasedDifficulty = false;
    private bool decreasedDifficulty = false;

    /// <summary>
    /// Check game status and activate corresponding difficulty controller
    /// </summary>
    private void Update()
    {
        if (inROMGame)
        {
            Debug.Log("ROM Parameter: " + PipeMovement.speed + " + " + PipeSpawner.spawnRate);
            ROMGameDifficultyController();
        }
        if (inEMGGame)
        {
            Debug.Log("EMG Parameter: ");
            EMGGameDifficultyController();
        }
    }

    /// <summary>
    /// This ROM difficulty method relies on gameoverTimeROM and currentROMscore in ROMGameCanvas.
    /// And is only activated when inROMGame is true.
    /// </summary>
    private void ROMGameDifficultyController()
    {
        // Increase difficulty logic.
        // Is triggered each time the currentROMscore increment 5.
        if (ROMGameCanvas.currentROMscore != 0f && ROMGameCanvas.currentROMscore % scoreIncrement == 0)
        {
            if (!increasedDifficulty)
            {
                difficultyLevel += 1;
                ROMDifficultyText.text = "Level: " + difficultyLevel.ToString();
                ROMGameHarder();
                increasedDifficulty = true;
            }
        }
        else
        {
            increasedDifficulty = false;
        }

        // Decrease difficulty logic.
        // Is triggered each time the gameoverTimeROM increment 2.
        if (ROMGameCanvas.gameoverTimeROM != 0f && ROMGameCanvas.gameoverTimeROM % gameoverIncrement == 0)
        {
            if (!decreasedDifficulty)
            {
                if (difficultyLevel > 0)
                {
                    difficultyLevel -= 1;
                }
                else
                {
                    difficultyLevel = 0;
                }
                ROMDifficultyText.text = "Level: " + difficultyLevel.ToString();
                ROMGameEasier();
                decreasedDifficulty = true;
            }
        }
        else
        {
            decreasedDifficulty = false;
        }
    }

    /// <summary>
    /// Method that makes the ROM game harder.
    /// Increase the spawn rate (+0.1) and the moving speed (+2) of the pipes.
    /// Is triggered each time the currentROMscore increment 5.
    /// </summary>
    private void ROMGameHarder()
    {
        if (PipeSpawner.spawnRate < maxSpawnRate)
        {
            PipeSpawner.spawnRate += 0.1f;
        }
        if (PipeMovement.speed < maxMoveSpeed)
        {
            PipeMovement.speed += 2f;
        }
    }

    /// <summary>
    /// Method that makes the ROM game easier.
    /// Decrease the spawn rate (-0.1) and the moving speed (-2) of the pipes.
    /// Is triggered each time the gameoverTimeROM increment 2.
    /// </summary>
    private void ROMGameEasier()
    {
        if (PipeSpawner.spawnRate > minSpawnRate)
        {
            PipeSpawner.spawnRate -= 0.1f;
        }
        if (PipeMovement.speed > minMoveSpeed)
        {
            PipeMovement.speed -= 2f;
        }
    }

    /// <summary>
    /// This EMG difficulty method relies on .......
    /// And is only activated when inEMGGame is true.
    /// </summary>
    private void EMGGameDifficultyController()
    {
        // Implement EMG game difficulty logic here
    }
}
