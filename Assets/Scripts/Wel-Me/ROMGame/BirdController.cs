using UnityEngine;
using UnityEngine.UI;

public class BirdController : MonoBehaviour
{
    public RectTransform bird;
    public float minY = -300f;
    public float maxY = 300f;

    private Image birdImage; // Use Image instead of SpriteRenderer
    public Sprite[] sprites; // Corrected the spelling mistake
    private int spritesIndex = 0;

    private void Awake()
    {
        birdImage = bird.GetComponent<Image>(); // Get the Image component from the RectTransform
    }

    private void Start()
    {
        InvokeRepeating(nameof(AnimateSprite), 0.15f, 0.15f); // Corrected the parameters
    }

    private void Update()
    {
        // Assuming BleDataStorage.Float1 is in the range of 0 to 100
        float normalizedValue = Mathf.Clamp01(BleDataStorage.Float1 / 100f);
        float yPosition = Mathf.Lerp(minY, maxY, normalizedValue);
        bird.anchoredPosition = new Vector2(bird.anchoredPosition.x, yPosition);
    }

    private void AnimateSprite()
    {
        spritesIndex++;
        if (spritesIndex >= sprites.Length)
        {
            spritesIndex = 0; // Reset the index to loop through the sprites
        }
        birdImage.sprite = sprites[spritesIndex]; // Set the sprite to the Image component
    }
}
