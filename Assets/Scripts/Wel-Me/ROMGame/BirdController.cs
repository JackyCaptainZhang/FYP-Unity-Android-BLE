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

    private void Update()
    {
        // BleDataStorage.Float1 is in the range of 0 to 100
        float normalizedValue = Mathf.Clamp01(BleDataStorage.Float1 / 100f);
        float yPosition = Mathf.Lerp(minY, maxY, normalizedValue);


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
