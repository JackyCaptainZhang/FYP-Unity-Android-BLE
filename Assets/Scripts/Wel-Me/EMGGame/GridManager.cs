using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GridManager : MonoBehaviour
{
    public GameObject[] covers; // Cover objects in the grid
    public Image cursor; // Cursor object
    public Text instructionText;
    public GameObject playButton;

    private int currentIndex = 0;
    private float timer = 0f;
    private const float revealDelay = 4f; // Delay time to reveal cover
    private bool gameStart = false;

    private int expectedNumber = 1; // The next number to be revealed
    private bool clickProcessed = false; // Flag to track if a click has been processed

    private void OnEnable()
    {
        resetGame();
    }

    private void Update()
    {
        float data = BleDataStorage.Float2;

        // Check for single click (101) or double click (202) and process it only once
        if (!clickProcessed)
        {
            if (data == 101)
            {
                MoveCursorForward();
                timer = 0f; // Reset the timer on cursor movement
                clickProcessed = true;
                BleDataStorage.Float2 = 0;
            }
            else if (data == 202)
            {
                MoveCursorBackward();
                timer = 0f; // Reset the timer on cursor movement
                clickProcessed = true;
                BleDataStorage.Float2 = 0;
            }
        }

        if (gameStart)
        {
            // Update the cursor position
            UpdateCursor(currentIndex);

            // Check if currentIndex is within valid range before updating instruction text
            if (currentIndex >= 0 && currentIndex < covers.Length)
            {
                // Update the instruction text based on the current state
                UpdateInstructionText(currentIndex);
            }
        }

        // If the timer reaches the delay time, reveal the cover if it's the correct number
        if (timer >= revealDelay)
        {
            CheckNumberAndReveal(currentIndex);
            timer = 0f; // Reset the timer after revealing the cover
        }

        // Increment the timer
        timer += Time.deltaTime;

        // Reset the clickProcessed flag if no click is detected
        if (BleDataStorage.Float2 == 0)
        {
            clickProcessed = false;
        }

        if (AllCoversRevealed())
        {
            resetGame();
        }
    }

    private void resetGame()
    {
        ResetCovers();
        RandomizeNumbers();
        currentIndex = 0;
        timer = 0f;
        expectedNumber = 1;
        Pause();
    }

    /// <summary>
    /// Move the cursor forward by one position.
    /// </summary>
    private void MoveCursorForward()
    {
        currentIndex = (currentIndex + 1) % covers.Length;
    }

    /// <summary>
    /// Move the cursor backward by one position.
    /// </summary>
    private void MoveCursorBackward()
    {
        currentIndex = (currentIndex - 1 + covers.Length) % covers.Length;
    }

    /// <summary>
    /// Randomize the numbers from 1 to 9 and display them on the covers.
    /// </summary>
    private void RandomizeNumbers()
    {
        List<int> numbers = new List<int>();
        for (int i = 1; i <= 9; i++)
        {
            numbers.Add(i);
        }
        for (int i = 0; i < covers.Length; i++)
        {
            int randomIndex = Random.Range(0, numbers.Count);
            int number = numbers[randomIndex];
            numbers.RemoveAt(randomIndex);

            AddTextComponent(covers[i]); // Ensure Text component exists
            Text numberText = covers[i].GetComponentInChildren<Text>();
            if (numberText != null)
            {
                numberText.text = number.ToString();
            }
        }
    }

    /// <summary>
    /// Add Text component to cover if it doesn't already exist.
    /// </summary>
    private void AddTextComponent(GameObject cover)
    {
        Text numberText = cover.GetComponentInChildren<Text>();
        if (numberText == null)
        {
            GameObject textObject = new GameObject("NumberText");
            textObject.transform.SetParent(cover.transform);
            numberText = textObject.AddComponent<Text>();
            numberText.font = Resources.GetBuiltinResource<Font>("Arial.ttf");
            numberText.alignment = TextAnchor.MiddleCenter;
            RectTransform rectTransform = textObject.GetComponent<RectTransform>();
            rectTransform.anchoredPosition = Vector2.zero;
            rectTransform.sizeDelta = cover.GetComponent<RectTransform>().sizeDelta;
        }
    }

    /// <summary>
    /// Update the instruction text based on the current state.
    /// </summary>
    private void UpdateInstructionText(int index)
    {
        Text numberText = covers[index].GetComponentInChildren<Text>();
        if (numberText != null && int.Parse(numberText.text) == expectedNumber)
        {
            instructionText.text = "Please keep cursor in grid " + expectedNumber.ToString() + " for " + revealDelay.ToString() + "s to reveal this cover. Time: " + timer.ToString("F2") + "s.";
        }
        else
        {
            instructionText.text = "Please move to grid " + expectedNumber.ToString();
        }
    }

    /// <summary>
    /// Check if the number on the cover is the expected number and reveal the cover if correct.
    /// </summary>
    private void CheckNumberAndReveal(int index)
    {
        Text numberText = covers[index].GetComponentInChildren<Text>();
        if (numberText != null && int.Parse(numberText.text) == expectedNumber)
        {
            RevealCover(index);
            expectedNumber++;
        }
    }

    /// <summary>
    /// Reveal the cover at the specified index.
    /// </summary>
    private void RevealCover(int index)
    {
        if (covers[index].GetComponent<Image>().enabled)
        {
            // Hide the cover image.
            covers[index].GetComponent<Image>().enabled = false;

            // Hide the number text.
            Text numberText = covers[index].GetComponentInChildren<Text>();
            if (numberText != null)
            {
                numberText.enabled = false;
            }
        }
    }

    /// <summary>
    /// Reset all covers to their initial state.
    /// </summary>
    private void ResetCovers()
    {
        foreach (GameObject cover in covers)
        {
            Image coverImage = cover.GetComponent<Image>();
            // Recover the cover image.
            coverImage.enabled = true;

            // Recover the number text.
            Text numberText = cover.GetComponentInChildren<Text>();
            if (numberText != null)
            {
                numberText.enabled = true;
            }
        }
    }

    /// <summary>
    /// Update the cursor position to the specified index.
    /// </summary>
    private void UpdateCursor(int index)
    {
        if (index >= 0 && index < covers.Length)
        {
            cursor.transform.position = covers[index].transform.position;
        }
    }

    private bool AllCoversRevealed()
    {
        foreach (GameObject cover in covers)
        {
            Image coverImage = cover.GetComponent<Image>();
            if (coverImage != null && coverImage.enabled)
            {
                return false; // At least one cover is still covered
            }
        }
        return true; // All covers are revealed
    }

    private void Pause()
    {
        Time.timeScale = 0;
        instructionText.text = "Press button to start.";
        playButton.SetActive(true);
        cursor.enabled = false;
        gameStart = false;
    }

    
    /// <summary>
    /// Start the EMG game.
    /// </summary>
    public void Play()
    {
        Time.timeScale = 1;
        playButton.SetActive(false);
        cursor.enabled = true;
        gameStart = true;

        // Update instruction immediately after starting the game
        if (currentIndex >= 0 && currentIndex < covers.Length)
        {
            UpdateInstructionText(currentIndex);
        }
    }

}
