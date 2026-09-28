using UnityEngine;

public class MovingTarget : MonoBehaviour
{
    //Have the target move back and forth
    public float speed = 5f;
    public float distance = 10f;
    private Vector3 startPosition;
    void Update()
    {
        if (startPosition == Vector3.zero)
            startPosition = transform.position;

        float newX = Mathf.PingPong(Time.time * speed, distance) + startPosition.x;
        transform.position = new Vector3(newX, transform.position.y, transform.position.z);
    }
}