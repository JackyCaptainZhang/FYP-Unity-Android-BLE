using UnityEngine;
using UnityEngine.UI;

public class GridManager : MonoBehaviour
{
    public GameObject[] covers; // Cover objects in the grid
    public Image cursor; // Cursor object
    public Text instructionText;
    public GameObject playButton;


    private float[] thresholds;
    private int currentIndex = -1;
    private float timer = 0f; 

    // Delay time to reveal cover
    private const float revealDelay = 4f; 

    private void OnEnable()
    {
        CalculateThresholds();
        ResetCovers();
        currentIndex = -1;
        timer = 0f;
        Pause();
    }


    private void Update()
    {
        float data = BleDataStorage.Float2;
        int newIndex = GetIndexFromData(data);

        // If the selected cell changes, reset the timer
        if (newIndex != currentIndex)
        {
            currentIndex = newIndex;
            timer = 0f;
        }
        else
        {
            // Update the timer only when the index hasn't changed
            timer += Time.deltaTime;
        }

        // Update the cursor position
        UpdateCursor(currentIndex);

        // If the timer reaches the delay time, reveal the cover
        if (timer >= revealDelay)
        {
            RevealCover(currentIndex);
            // Reset the timer after revealing the cover
            timer = 0f; 
        }
        if(Time.timeScale != 0)
        {
            instructionText.text = "Please keep cursor in grid " + revealDelay.ToString() + "s to reveal this cover. Time: " + timer.ToString() + "s.";
        }
    }

    /// <summary>
    /// Calculate the thresholds for each range.
    /// </summary>
    void CalculateThresholds()
    {
        thresholds = new float[9];
        float interval = (BleDataStorage.MaxEMG - BleDataStorage.MinEMG) / 9f;
        for (int i = 0; i < 9; i++)
        {
            thresholds[i] = BleDataStorage.MinEMG + interval * (i + 1);
        }
    }

    /// <summary>
    /// Get the index of the cell based on the data.
    /// </summary>
    private int GetIndexFromData(float data)
    {
        for (int i = 0; i < thresholds.Length; i++)
        {
            if (data <= thresholds[i])
            {
                return i;
            }
        }
        return thresholds.Length - 1;
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
        }
    }

    /// <summary>
    /// Update the cursor position to the specified index.
    /// </summary>
    private void UpdateCursor(int index)
    {
        cursor.transform.position = covers[index].transform.position;
    }

    private void Pause()
    {
        Time.timeScale = 0;
        instructionText.text = "Press button to start.";
        playButton.SetActive(true);
        cursor.enabled = false;
    }

    /// <summary>
    /// Start the EMG game.
    /// </summary>
    public void Play()
    {
        Time.timeScale = 1;
        playButton.SetActive(false);
        cursor.enabled = true;
    }
}
