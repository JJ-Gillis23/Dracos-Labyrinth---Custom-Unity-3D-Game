using UnityEngine;

public class AlternateRestartPlatform : MonoBehaviour
{
    [Header("References")]
    public Transform spawnPoint;
    //Specific rotation for the player to face when respawning, if desired
    public Quaternion respawnRotation = Quaternion.identity;
    public string keyID = "key_01";

    [Header("Key Object")]
    public GameObject keyObject; // assign the KeyHolder parent object

    [Header("Platforms")]
    public GameObject platformPrefab;
    public GameObject[] platformInstances;

    private Vector3[] cachedPositions;
    private Quaternion[] cachedRotations;
    private Vector3[] cachedScales;

    void Start()
    {
        // Disable key instead of destroying so we can re-enable it
        if (keyObject != null)
            keyObject.SetActive(true); // make sure it starts active

        cachedPositions = new Vector3[platformInstances.Length];
        cachedRotations = new Quaternion[platformInstances.Length];
        cachedScales    = new Vector3[platformInstances.Length];

        for (int i = 0; i < platformInstances.Length; i++)
        {
            if (platformInstances[i] != null)
            {
                cachedPositions[i] = platformInstances[i].transform.position;
                cachedRotations[i] = platformInstances[i].transform.rotation;
                cachedScales[i]    = platformInstances[i].transform.localScale;
            }
        }
    }

    void OnCollisionEnter(Collision collision)
    {
        if (!collision.gameObject.CompareTag("Player")) return;

        // 1. Remove key from inventory
        if (Inventory.Instance.HasKey(keyID))
            Inventory.Instance.UseKey(keyID); // wipes all keys + UI in one call

        // 2. Re-enable the key pickup
        if (keyObject != null)
            keyObject.SetActive(true);

        // 3. Destroy only THIS puzzle's platforms
        foreach (GameObject instance in platformInstances)
        {
            if (instance != null)
                Destroy(instance);
        }

        // Also clean up any respawned versions by checking cached positions
        foreach (BreakablePlatform alive in FindObjectsByType<BreakablePlatform>())
        {
            for (int i = 0; i < cachedPositions.Length; i++)
            {
                if (Vector3.Distance(alive.transform.position, cachedPositions[i]) < 0.5f)
                {
                    Destroy(alive.gameObject);
                    break;
                }
            }
        }

        // 4. Reinstantiate all platforms from cached transforms
        for (int i = 0; i < cachedPositions.Length; i++)
        {
            GameObject newPlatform = Instantiate(platformPrefab, cachedPositions[i], cachedRotations[i]);
            newPlatform.transform.localScale = cachedScales[i];
        }

        // 5. Move player back to spawn point
        CharacterController cc = collision.gameObject.GetComponent<CharacterController>();
        if (cc != null)
        {
            cc.enabled = false;
            collision.gameObject.transform.position = spawnPoint.position;
            collision.gameObject.transform.rotation = respawnRotation;
            cc.enabled = true;
        }
        else
        {
            collision.gameObject.transform.position = spawnPoint.position;
            collision.gameObject.transform.rotation = respawnRotation;
        }

        // 6. Reset player velocity
        Rigidbody rb = collision.gameObject.GetComponent<Rigidbody>();
        if (rb != null)
            rb.linearVelocity = Vector3.zero;
    }
}