using UnityEngine;

public class MovingObstacle : MonoBehaviour
{
    public float moveSpeed = 10f;
    public float moveDistance = 50f;
    public float pushForce = 15f;

    private Vector3 startPos;
    private Rigidbody rb;

    void Start()
    {
        startPos = transform.position;
        rb = GetComponent<Rigidbody>();
        rb.isKinematic = true;
    }

    void FixedUpdate()
    {
        Vector3 newPos = rb.position + (-transform.forward * moveSpeed * Time.fixedDeltaTime);
        rb.MovePosition(newPos);

        if (Vector3.Distance(startPos, rb.position) >= moveDistance)
            Destroy(gameObject);
    }

    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            Rigidbody playerRb = collision.gameObject.GetComponent<Rigidbody>();
            if (playerRb != null)
            {
                Vector3 pushDirection = -collision.contacts[0].normal;
                pushDirection.y = 0f;
                playerRb.linearVelocity = Vector3.zero;
                playerRb.AddForce(pushDirection * pushForce + Vector3.up * 3f, ForceMode.Impulse);
            }
        }
    }
}