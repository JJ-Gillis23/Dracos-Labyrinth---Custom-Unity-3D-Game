using UnityEngine;
using System.Collections;

public class Fan : MonoBehaviour
{
    public float force = 1000f;
    public float activeTime = 3f;
    public float offTime = 3f;

    private bool isActive = false;
    private Collider fanCollider;
    private ParticleSystem windParticles;

    void Start()
    {
        fanCollider = GetComponent<Collider>();
        windParticles = GetComponentInChildren<ParticleSystem>();
        fanCollider.enabled = false;
        if (windParticles != null) windParticles.Stop();
        StartCoroutine(FanCycle());
    }

    IEnumerator FanCycle()
    {
        while (true)
        {
            isActive = true;
            fanCollider.enabled = true;
            if (windParticles != null) windParticles.Play();
            yield return new WaitForSeconds(activeTime);

            isActive = false;
            fanCollider.enabled = false;
            if (windParticles != null) windParticles.Stop();
            yield return new WaitForSeconds(offTime);
        }
    }

    void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("Player") && isActive)
        {
            Rigidbody rb = other.GetComponent<Rigidbody>();
            if (rb != null)
                rb.AddForce(-transform.forward * force, ForceMode.Force);
        }
    }
}