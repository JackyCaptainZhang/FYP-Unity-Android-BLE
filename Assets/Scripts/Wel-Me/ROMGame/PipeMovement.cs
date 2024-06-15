using UnityEngine;

public class PipeMovement : MonoBehaviour
{
    public static float speed = 12f; 

    void Update()
    {
        // Move the pipe to the left
        transform.Translate(Vector2.left * speed * Time.deltaTime);
    }
}
