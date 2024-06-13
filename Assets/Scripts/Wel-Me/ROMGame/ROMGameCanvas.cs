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


    private int score;

    private void Awake()
    {
        gameOver.SetActive(false);
        Application.targetFrameRate = 60;
        Pause();
        
    }

    public void Play()
    {
        score = 0;
        scoreText.text = "Score:" + score.ToString();

        playButton.SetActive(false);
        gameOver.SetActive(false);
        Time.timeScale = 1;
        bird.enabled = true;

        PipeMovement[] pipes = FindObjectsOfType<PipeMovement>();
        foreach (PipeMovement pipe in pipes)
        {
            Destroy(pipe.gameObject);
        }
    }

    public void Pause()
    {
        Time.timeScale = 0;
        bird.enabled = false;
    }


    public void GameOver()
    {
        gameOver.SetActive(true);
        playButton.SetActive(true);

        Pause();
    }

    public void IncreaseScore()
    {
        score++;
        scoreText.text = "Score:" + score.ToString();
    }
    
}
