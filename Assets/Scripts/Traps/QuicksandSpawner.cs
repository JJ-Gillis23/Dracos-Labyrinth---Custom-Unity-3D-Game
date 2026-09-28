using UnityEngine;

public class QuicksandSpawner : MonoBehaviour
{
    public GameObject quicksandPrefab;
    public float spawnDistance = 20f;
    public Vector3 respawnPoint;
    public float groundY = 11.5f;
    public int quicksandCount = 5;
    public float spacing = 1.5f;
    public float rowSpacing = 10f;
    public float lifetime = 10f;
    public int rows = 3;

    private bool isActive = false;
    private Vector3 capturedSpawnBase;
    private Vector3 capturedPlayerRight;
    private Vector3 capturedForward;

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && !isActive)
        {
            isActive = true;
            capturedPlayerRight = new Vector3(transform.right.x, 0, transform.right.z).normalized;
            capturedForward = new Vector3(transform.forward.x, 0, transform.forward.z).normalized;
            capturedSpawnBase = transform.position + transform.forward * spawnDistance + transform.right * 5f;
            SpawnQuicksand();
        }
    }

public bool permanent = false;

    void SpawnQuicksand()
    {
        for (int row = 0; row < rows; row++)
        {
            Vector3 rowBase = capturedSpawnBase - capturedForward * rowSpacing * row;

            for (int i = 0; i < quicksandCount; i++)
            {
                float offset = (i - quicksandCount / 2) * spacing + 5f;
                Vector3 spawnPos = new Vector3(
                    rowBase.x + capturedPlayerRight.x * offset,
                    groundY,
                    rowBase.z + capturedPlayerRight.z * offset
                );

                GameObject qs = Instantiate(quicksandPrefab, spawnPos, Quaternion.identity);
                Quicksand quicksand = qs.GetComponent<Quicksand>();
                if (quicksand != null)
                    quicksand.respawnPoint = respawnPoint;

                if (!permanent)
                    Destroy(qs, lifetime);
            }
        }
    }
}