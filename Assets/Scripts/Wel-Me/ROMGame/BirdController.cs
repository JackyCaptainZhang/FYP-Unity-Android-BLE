using UnityEngine;
using UnityEngine.UI;

public class BirdController : MonoBehaviour
{
    public RectTransform bird;
    public float minY;
    public float maxY;

    private Image birdImage;
    public Sprite[] sprites;
    private int spritesIndex = 0;

    private void Awake()
    {
        birdImage = bird.GetComponent<Image>(); // Get the Image component
    }

    private void Start()
    {
        InvokeRepeating(nameof(AnimateSprite), 0.15f, 0.15f);
    }

    /// <summary>
    /// Update the position of the bird.
    /// </summary>
    private void Update()
    {

        // Calculate the range of ROM data
        float ROMDataRange = BleDataStorage.MaxROM - BleDataStorage.MinROM;

        // Clamp the received ROM data value within the MinROM and MaxROM range
        float clampedValue = Mathf.Clamp(BleDataStorage.Float1, BleDataStorage.MinROM, BleDataStorage.MaxROM);

        // Normalize the clamped value to a range between 0 and 1
        float normalizedValue = (clampedValue - BleDataStorage.MinROM) / ROMDataRange;

        // Interpolate the normalized value to get the bird's y position between minY and maxY
        float yPosition = Mathf.Lerp(minY, maxY, normalizedValue);

        // Update the bird's position
        bird.anchoredPosition = new Vector2(bird.anchoredPosition.x, yPosition);

    }

    /// <summary>
    /// Birds animation.
    /// </summary>
    private void AnimateSprite()
    {
        spritesIndex++;
        if (spritesIndex >= sprites.Length)
        {
            spritesIndex = 0; // Reset the index to loop through the sprites
        }
        birdImage.sprite = sprites[spritesIndex]; // Set the sprite to the Image component
    }

    /// <summary>
    /// Detect the collision type and call the corresponding function.
    /// </summary>
    private void OnTriggerEnter2D(Collider2D collision)
    {
        ROMGameCanvas romGameCanvas = FindObjectOfType<ROMGameCanvas>();
        if (collision.gameObject.tag == "Obstacle")
        {
            romGameCanvas.GameOver();
        }else if(collision.gameObject.tag == "Scoring")
        {
            romGameCanvas.IncreaseScore();
        }
    }
}
