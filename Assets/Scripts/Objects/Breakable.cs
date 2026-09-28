using UnityEngine;
public class Breakable : MonoBehaviour
{
    private AudioSource audioSource;
    public GameObject particles;
    void Start()
    {
        if(GetComponent<AudioSource>() != null)
        {
            audioSource = GetComponent<AudioSource>();
        }
    }
    void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject.CompareTag("Projectile"))
        {
            if(audioSource != null)
                AudioSource.PlayClipAtPoint(audioSource.clip, transform.position);

            if(particles != null)
                Instantiate(particles, transform.position, Quaternion.identity);

            Destroy(gameObject);
        }
    }
}