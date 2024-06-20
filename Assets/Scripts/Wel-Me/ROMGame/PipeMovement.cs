using UnityEngine;

/// <summary>
/// This script is for movement of the pipes.
/// Can be paused or start by using 'Time.timeScale'.
/// </summary>
public class PipeMovement : MonoBehaviour
{
    public static float speed = 12f; 

    void Update()
    {
        // Move the pipe to the left
        transform.Translate(Vector2.left * speed * Time.deltaTime);
    }
}
