using UnityEngine;


/// <summary>
/// This script is for pipe auto and random spawning.
/// </summary>
public class PipeSpawner : MonoBehaviour
{
    public GameObject pipePrefab;
    public Canvas canvas;
    public RectTransform canvasRect;
    public static float spawnRate = 3.5f; 

    // Spawn position range of the pipes.
    // Should match with the move range of the birds!
    public float minY; 
    public float maxY;
    

    void Start()
    {  
        if (canvas.renderMode != RenderMode.ScreenSpaceCamera)
        {
            canvas.renderMode = RenderMode.ScreenSpaceCamera;
            canvas.worldCamera = Camera.main;
            canvas.planeDistance = 100f;
        }

        canvasRect.anchorMin = new Vector2(0, 0);
        canvasRect.anchorMax = new Vector2(1, 1);
        canvasRect.pivot = new Vector2(0.5f, 0.5f);

        InvokeRepeating(nameof(SpawnPipe), 2.0f, spawnRate);
    }

    void SpawnPipe()
    {       
        // Generate random Y position within specified range.
        float yPosition = Random.Range(minY, maxY);

        // Instantiate pipePrefab.
        GameObject newPipe = Instantiate(pipePrefab, canvasRect);

        // Set the position of the new pipe using RectTransform.
        RectTransform pipeRectTransform = newPipe.GetComponent<RectTransform>();

        // Set the pipe to the right most of the canvas.
        float canvasWidth = canvasRect.rect.width;
        float xPosition = canvasWidth / 2 + pipeRectTransform.rect.width / 2;
        pipeRectTransform.anchoredPosition = new Vector2(xPosition, yPosition);     
    }
}
