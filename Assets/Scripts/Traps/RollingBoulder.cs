using UnityEngine;

public class RollingBoulder : MonoBehaviour
{
    public float rollSpeed = 20f;
    public bool reverseDirection = false;
    private Rigidbody rb;
    private Vector3 respawnPoint;

    public void Init(Vector3 respawn, bool reverse = false)
    {
        respawnPoint = respawn;
        reverseDirection = reverse;
    }

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        int wallLayer = LayerMask.NameToLayer("Walls");
        int boulderLayer = LayerMask.NameToLayer("Boulder");
        Physics.IgnoreLayerCollision(boulderLayer, wallLayer);
    }

    void FixedUpdate()
    {
        rb.linearVelocity = (reverseDirection ? Vector3.right : Vector3.left) * rollSpeed;
        rb.angularVelocity = (reverseDirection ? -Vector3.forward : Vector3.forward) * rollSpeed;

        if (CameraShake.Instance != null)
            CameraShake.Instance.Shake(0.1f, 0.04f);
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
        if(collision.gameObject.CompareTag("End"))
        {
            Destroy(gameObject);
        }
    }
}