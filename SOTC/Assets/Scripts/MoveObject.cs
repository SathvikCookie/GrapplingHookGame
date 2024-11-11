using UnityEngine;

public class MoveObject : MonoBehaviour
{
    public float speed = 5.0f;  // Speed of the movement
    public Vector3 direction = new Vector3(1, 0 , 0);

    void Update()
    {
        // Move the object along the X-axis
        transform.Translate(direction * speed * Time.deltaTime);
    }
}
