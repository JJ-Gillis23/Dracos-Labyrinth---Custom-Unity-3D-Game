using UnityEngine;
using UnityEngine.SceneManagement;
public class SpikeTrap : MonoBehaviour
{
    public int damage = 20; // Amount of damage to deal to the player
    private AudioSource audioSource; // Audio source for hurt activation sound
    private AudioSource audiosource2;
    public Vector3 respawnpoint;

    void Start()
    {
        audiosource2 = GetComponent<AudioSource>();
        //audiosource2.Play();
    }

    void Update()
    {

        // Destroy the trap if it falls below a certain point to prevent clutter
        transform.Translate(Vector3.down * Time.deltaTime * 5f, Space.World);
        if (transform.position.y < -5f)
        {
            Destroy(gameObject);
        }
    }
    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            Destroy(gameObject); // Destroy the trap after activation
            //Respawn the player
            RespawnPlayer();

        }
        if(collision.gameObject.CompareTag("Projectile"))
        {
            Destroy(gameObject); // Destroy the trap if hit by a projectile
        }
    }
void RespawnPlayer()
{
    GameObject player = GameObject.FindGameObjectWithTag("Player");
    if(player != null)
    {
        player.transform.position = respawnpoint;
    }
}
}