using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ROMGameCanvas : MonoBehaviour
{
    public BirdController bird;
    public Text scoreText;
    public GameObject playButton;
    public GameObject gameOver;
    public Text highestScoreText;


    // This score will be 0 when exit the app or game over.
    // Any manual or auto disconnection pause will keep this score unchanged.
    private int score;

    // This highestScore is set to public and can be stored in database when exit the app.
    // ONLY be 0 when exit the app.
    public int highestScore;
    private int lastScore;


    /// <summary>
    /// Always let user start the game when the canvas is called.
    /// Update score and highestScore text.
    /// </summary>
    private void OnEnable()
    {
        gameOver.SetActive(false);
        Application.targetFrameRate = 60;
        scoreText.text = "Score: " + score.ToString();
        highestScoreText.text = "Highest Score: " + highestScore.ToString();
        Pause();
    }

    
    private void Awake()
    {
        gameOver.SetActive(false);
        Application.targetFrameRate = 60;
        score = 0;
        lastScore = 0;
        highestScore = 0;
        Pause();
    }

    /// <summary>
    /// Start the paused game.
    /// Update the score text.
    /// </summary>
    public void Play()
    {
        scoreText.text = "Score: " + score.ToString();
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
    /// Is called when collide with object with tag "Obstacle"
    /// Update Gameover UI.
    /// Record the last score and update the highest score.
    /// </summary>
    public void GameOver()
    {
        Pause();
        gameOver.SetActive(true);
        lastScore = 0;
        lastScore = score;
        score = 0;
        if(lastScore > highestScore)
        {
            highestScore = lastScore;
        }
        highestScoreText.text = "Highest Score: " + highestScore.ToString();

    }

    /// <summary>
    /// Is called when collide with object with tag "Scoring".
    /// Update the score text.
    /// </summary>
    public void IncreaseScore()
    {
        score++;
        scoreText.text = "Score: " + score.ToString();
    }
    
}
