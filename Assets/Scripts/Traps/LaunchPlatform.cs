using UnityEngine;

public class LaunchPlatform : MonoBehaviour
{
    public float launchUpForce = 15f;
    public float launchBackForce = 10f;

    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            Rigidbody playerRb = collision.gameObject.GetComponent<Rigidbody>();
            if (playerRb != null)
            {
                Vector3 launchDirection = -transform.forward * launchBackForce + Vector3.up * launchUpForce;                
                playerRb.linearVelocity = Vector3.zero;
                playerRb.AddForce(launchDirection, ForceMode.Impulse);
            }
        }
    }
}