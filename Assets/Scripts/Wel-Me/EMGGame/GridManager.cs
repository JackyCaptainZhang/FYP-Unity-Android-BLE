using UnityEngine;
using UnityEngine.UI;

public class NineGridManager : MonoBehaviour
{
    public GameObject[] covers; // Cover objects in the grid
    public Image cursor; // Cursor object
    private float[] thresholds; 

    private int currentIndex = -1;
    private float timer = 0f; 

    // Delay time to reveal cover
    private const float revealDelay = 4f; 

    private void OnEnable()
    {
        CalculateThresholds();
        ResetCovers();
        Time.timeScale = 1;
        currentIndex = -1;
        timer = 0f; 
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
}
