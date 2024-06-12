using UnityEngine;

public class PipeMovement : MonoBehaviour
{
    public float speed; 

    void Update()
    {
        // Move the pipe to the left
        transform.Translate(Vector2.left * speed * Time.deltaTime);
    }
}
