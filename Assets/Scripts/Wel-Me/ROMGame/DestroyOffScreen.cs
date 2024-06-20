using UnityEngine;


/// <summary>
/// This script is for destroying the pipe objects when they are moving off the screen.
/// </summary>
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
