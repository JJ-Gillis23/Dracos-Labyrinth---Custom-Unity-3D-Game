using UnityEngine;

public class FlyingObstacle : MonoBehaviour
{
    public float moveSpeed = 2f;
    public float pushForce = 10f;

    private Vector3 pointA;
    private Vector3 pointB;
    private Vector3 currentTarget;
    private bool initialized = false;

    public void Init(Vector3 a, Vector3 b)
    {
        pointA = a;
        pointB = b;
        currentTarget = pointB;
        initialized = true;
    }

    void Update()
    {
        if (!initialized) return;

        transform.position = Vector3.MoveTowards(
            transform.position,
            currentTarget,
            moveSpeed * Time.deltaTime
        );

        if (Vector3.Distance(transform.position, currentTarget) < 0.05f)
            currentTarget = (currentTarget == pointA) ? pointB : pointA;
    }

    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            Rigidbody playerRb = collision.gameObject.GetComponent<Rigidbody>();
            if (playerRb != null)
            {
                Vector3 pushDirection = (collision.transform.position - transform.position).normalized;
                pushDirection.y = 0f;
                playerRb.linearVelocity = Vector3.zero;
                playerRb.AddForce(pushDirection * pushForce + Vector3.up * 3f, ForceMode.Impulse);
            }
            Destroy(gameObject);
        }

        if (collision.gameObject.CompareTag("Projectile"))
            Destroy(gameObject);
    }
}