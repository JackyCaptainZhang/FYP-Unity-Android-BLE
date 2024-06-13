using UnityEngine;

public class PipeSpawner : MonoBehaviour
{
    public GameObject pipePrefab;
    public Canvas canvas;
    public RectTransform canvasRect;
    public float spawnRate; 
    public float minY; 
    public float maxX;
    

    void Start()
    {
        
        if (canvas == null)
        {
            Debug.LogError("Canvas is not assigned!");
            return;
        }

        if (canvasRect == null)
        {
            Debug.LogError("Canvas RectTransform is not assigned!");
            return;
        }

        if (pipePrefab == null)
        {
            Debug.LogError("Pipe Prefab is not assigned!");
            return;
        }

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
        if (canvasRect == null)
        {
            Debug.LogError("Canvas RectTransform is null in SpawnPipe!");
            return;
        }

        if (pipePrefab == null)
        {
            Debug.LogError("Pipe Prefab is null in SpawnPipe!");
            return;
        }

        // Generate random Y position within specified range
        float yPosition = Random.Range(minY, maxY);

        // Instantiate pipePrefab
        GameObject newPipe = Instantiate(pipePrefab, canvasRect);

        // Set the position of the new pipe using RectTransform
        RectTransform pipeRectTransform = newPipe.GetComponent<RectTransform>();

        if (pipeRectTransform == null)
        {
            Debug.LogError("Pipe Prefab does not have a RectTransform component!");
            return;
        }

        // Set the pipe to the right most of the canvas
        float canvasWidth = canvasRect.rect.width;
        float xPosition = canvasWidth / 2 + pipeRectTransform.rect.width / 2;
        pipeRectTransform.anchoredPosition = new Vector2(xPosition, yPosition);     
    }
}
