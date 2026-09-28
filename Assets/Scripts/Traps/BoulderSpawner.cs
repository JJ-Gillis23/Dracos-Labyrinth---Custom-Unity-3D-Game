using UnityEngine;
using System.Collections;

public class BoulderSpawner : MonoBehaviour
{
    public GameObject boulderPrefab;
    public Transform boulderSpawnPoint;
    public Vector3 respawnPoint;
    public float spawnCooldown = 5f;
    public bool showwarning = true;
    public bool reverseDirection = false;
    private bool onCooldown = false;

    void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.CompareTag("Player") && !onCooldown)
            StartCoroutine(SpawnWithCooldown());
    }

    IEnumerator SpawnWithCooldown()
    {
        onCooldown = true;
        GameObject boulder = Instantiate(boulderPrefab, boulderSpawnPoint.position, boulderSpawnPoint.rotation);
        if(showwarning)
            PlayerUI.Instance.ShowPersistentText("Watch out behind you!");
        RollingBoulder rb = boulder.GetComponent<RollingBoulder>();
        if(rb != null)
            rb.Init(respawnPoint, reverseDirection);

        yield return new WaitForSeconds(spawnCooldown);
        onCooldown = false;
    }
}