using UnityEngine;
using System.Collections;

public class Laser : MonoBehaviour
{
    public float activeTime = 3f;
    public float offTime = 3f;

    private Collider laserCollider;
    private Renderer laserRenderer;

    void Start()
    {
        laserCollider = GetComponent<Collider>();
        laserRenderer = GetComponent<Renderer>();
        StartCoroutine(LaserCycle());
    }

    IEnumerator LaserCycle()
    {
        while (true)
        {
            laserCollider.enabled = true;
            laserRenderer.enabled = true;
            yield return new WaitForSeconds(activeTime);
            laserCollider.enabled = false;
            laserRenderer.enabled = false;
            yield return new WaitForSeconds(offTime);
        }
    }

    void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            Rigidbody playerRb = other.gameObject.GetComponent<Rigidbody>();
            if (playerRb != null)
            {
                playerRb.linearVelocity = Vector3.zero;
                playerRb.AddForce(-transform.right * 30f + Vector3.up * 5f, ForceMode.Impulse);
            }
        }
    }
}