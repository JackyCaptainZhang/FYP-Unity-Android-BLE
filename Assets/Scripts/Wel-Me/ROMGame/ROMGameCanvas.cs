using UnityEngine;
using UnityEngine.UI;


/// <summary>
/// Main controller for ROM game.
/// </summary>
public class ROMGameCanvas : MonoBehaviour
{
    public BirdController bird;
    public Text scoreText;
    public GameObject playButton;
    public GameObject gameOver;
    public Text highestScoreText;
    public Text ROMDifficultyText;



    // This score will be 0 when exit the app or game over.
    // Any manual or auto disconnection pause will keep this score unchanged.
    // Is used to control the game difficulty.
    public static int currentROMscore;

    private int lastROMScore;

    // Is used to control the game difficulty.
    public static int gameoverTimeROM;

    // This highestScore is set to public and can be stored in database when exit the app.
    // ONLY be 0 when exit the app.
    private int highestROMScore;


    /// <summary>
    /// Always let user start the game when the canvas is called.
    /// Update score and highestScore text.
    /// </summary>
    private void OnEnable()
    {
        gameOver.SetActive(false);
        Application.targetFrameRate = 60;
        ROMDifficultyText.text = "Level: 0";
        scoreText.text = "Score: " + currentROMscore.ToString();
        highestScoreText.text = "Highest Score: " + highestROMScore.ToString();
        DifficultyManager.difficultyLevel = 0;
        Pause();
    }

    
    private void Awake()
    {
        gameOver.SetActive(false);
        Application.targetFrameRate = 60;
        currentROMscore = 0;
        lastROMScore = 0;
        highestROMScore = 0;
        gameoverTimeROM = 0;
        DifficultyManager.difficultyLevel = 0;
        Pause();
    }

    /// <summary>
    /// Start the paused game.
    /// Update the score text.
    /// </summary>
    public void Play()
    {
        scoreText.text = "Score: " + currentROMscore.ToString();
        playButton.SetActive(false);
        gameOver.SetActive(false);
        Time.timeScale = 1;
        bird.enabled = true;
        // Destory all the exsiting pipes
        PipeMovement[] pipes = FindObjectsOfType<PipeMovement>();
        foreach (PipeMovement pipe in pipes)
        {
            Destroy(pipe.gameObject);
        }
    }

    /// <summary>
    /// Pause the game frame.
    /// </summary>
    public void Pause()
    {
        gameOver.SetActive(false);
        playButton.SetActive(true);
        Time.timeScale = 0;
        bird.enabled = false;
    }

    /// <summary>
    /// Is called when collide with object with tag "Obstacle".
    /// Update Gameover UI.
    /// Record the last score and update the highest score and gameoverTimeROM.
    /// </summary>
    public void GameOver()
    {
        Pause();
        gameOver.SetActive(true);
        lastROMScore = 0;
        lastROMScore = currentROMscore;
        currentROMscore = 0;
        if(lastROMScore > highestROMScore)
        {
            highestROMScore = lastROMScore;
        }
        highestScoreText.text = "Highest Score: " + highestROMScore.ToString();
        gameoverTimeROM++;
    }

    /// <summary>
    /// Is called when collide with object with tag "Scoring".
    /// Update the score text.
    /// </summary>
    public void IncreaseScore()
    {
        currentROMscore++;
        scoreText.text = "Score: " + currentROMscore.ToString();
    }
    
}
