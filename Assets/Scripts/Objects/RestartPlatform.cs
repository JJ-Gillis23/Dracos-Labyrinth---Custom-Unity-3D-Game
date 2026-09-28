using UnityEngine;

public class RestartPlatform : MonoBehaviour
{
    public Vector3 respawnPoint;
    public float rotationY = -90f;
    void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject.CompareTag("Player"))
        {
            collision.gameObject.transform.position = respawnPoint;
            collision.gameObject.transform.rotation = Quaternion.Euler(0, rotationY, 0);
            
            Rigidbody rb = collision.gameObject.GetComponent<Rigidbody>();
            if(rb != null) rb.linearVelocity = Vector3.zero;
            //Destroy any boulders on the platform to prevent softlock
            foreach(GameObject boulder in GameObject.FindGameObjectsWithTag("Boulder"))
            {
                    Destroy(boulder);
            }

        }
        
    }
}