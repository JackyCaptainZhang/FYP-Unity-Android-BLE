using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// This script is for background scrolling for ROM game.
/// The scroll can be paused or start by using 'Time.timeScale'.
/// </summary>
public class BackgroundScroller : MonoBehaviour
{
    public float scrollSpeed = 0.1f;
    private RawImage rawImage;
    private Rect uvRect;

    void Start()
    {
        rawImage = GetComponent<RawImage>();
        uvRect = rawImage.uvRect;
    }

    void Update()
    {
        uvRect.x += scrollSpeed * Time.deltaTime;
        rawImage.uvRect = uvRect;
    }
}
