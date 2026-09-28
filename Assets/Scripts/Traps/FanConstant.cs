using UnityEngine;

public class FanConstant : MonoBehaviour
{
    public float force = 500f;
    private ParticleSystem windParticles;

    void Start()
    {
        windParticles = GetComponentInChildren<ParticleSystem>();
        if (windParticles != null) windParticles.Play();
    }

    void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            Rigidbody rb = other.GetComponent<Rigidbody>();
            if (rb != null)
                rb.AddForce(-transform.forward * force, ForceMode.Force);
        }
    }
}