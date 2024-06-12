using UnityEngine;

public class DestroyOffScreen : MonoBehaviour
{
    private RectTransform rectTransform;
    private RectTransform canvasRect;

    void Start()
    {
        rectTransform = GetComponent<RectTransform>();
        Canvas parentCanvas = GetComponentInParent<Canvas>();

        if (parentCanvas != null)
        {
            canvasRect = parentCanvas.GetComponent<RectTransform>();
        }

        if (canvasRect == null)
        {
            Debug.LogError("DestroyOffScreen: Canvas RectTransform not found!");
        }
    }

    void Update()
    {
        if (rectTransform != null && canvasRect != null)
        {
            if (rectTransform.anchoredPosition.x < -canvasRect.rect.width / 2 - rectTransform.rect.width / 2)
            {
                Destroy(gameObject);
            }
        }
    }
}
