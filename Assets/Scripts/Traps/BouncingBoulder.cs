using UnityEngine;

public class BouncingBoulder : MonoBehaviour
{
    public float rollSpeed = 15f;
    private Rigidbody rb;
    private Vector3 respawnPoint;
    private AudioSource audioSource;
    private static bool audioPlaying = false; // shared across all boulders

    public void Init(Vector3 respawn)
    {
        respawnPoint = respawn;
    }

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        audioSource = GetComponent<AudioSource>();

        PhysicsMaterial bounceMat = new PhysicsMaterial();
        bounceMat.bounciness = 0.6f;
        bounceMat.frictionCombine = PhysicsMaterialCombine.Minimum;
        bounceMat.bounceCombine = PhysicsMaterialCombine.Maximum;
        GetComponent<Collider>().material = bounceMat;
        //Ignore fan layer
        Physics.IgnoreLayerCollision(gameObject.layer, LayerMask.NameToLayer("Fan"), true);

        rb.linearVelocity = -transform.forward * rollSpeed;

        // Only play audio if no other boulder is playing
        if (!audioPlaying && audioSource != null)
        {
            audioPlaying = true;
            audioSource.Play();
        }

        Destroy(gameObject, 20f);
    }

    void OnDestroy()
    {
        // Free up audio when this boulder is destroyed
        if (audioSource != null && audioSource.isPlaying)
            audioPlaying = false;
    }

    void FixedUpdate()
    {
        Vector3 vel = rb.linearVelocity;
        vel.x = Mathf.Clamp(vel.x, -rollSpeed, rollSpeed);
        vel.z = Mathf.Clamp(vel.z, -rollSpeed, rollSpeed);
        rb.linearVelocity = vel;

        if (CameraShake.Instance != null)
            CameraShake.Instance.Shake(0.1f, 0.05f);
    }

    void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject.CompareTag("Player"))
        {
            GameObject player = GameObject.FindGameObjectWithTag("Player");
            if(player != null)
                player.transform.position = respawnPoint;
            Destroy(gameObject);
        }
    }
}