using UnityEngine;

public class BreakablePlatform : MonoBehaviour
{
    public float breakDelay = 2f;
    public float resetDelay = 2f;
    public GameObject platformPrefab;
    private Vector3 originalPosition;
    private Quaternion originalRotation;
    private bool isBreaking = false;
    public bool needReset = true;

    void Start()
    {
        originalPosition = transform.position;
        originalRotation = transform.rotation;
    }

    void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject.CompareTag("Player") && !isBreaking)
        {
            isBreaking = true;
            Invoke("BreakPlatform", breakDelay);
        }
    }

    void BreakPlatform()
    {
        if (needReset)
        {
            GameObject manager = new GameObject("PlatformResetter");
            PlatformResetter resetter = manager.AddComponent<PlatformResetter>();
            resetter.Init(platformPrefab, originalPosition, originalRotation, resetDelay);
        }
        Destroy(gameObject);
    }

    private bool isCompleted = false;

    public void SetCompleted()
    {
        isCompleted = true;
    }

    public void ForceReset()
    {
        if (isCompleted) return;

        CancelInvoke("BreakPlatform");
        isBreaking = false;

        if (needReset) // only respawn if flagged
        {
            GameObject manager = new GameObject("PlatformResetter");
            PlatformResetter resetter = manager.AddComponent<PlatformResetter>();
            resetter.Init(platformPrefab, originalPosition, originalRotation, 0f);
        }

        Destroy(gameObject);
    }
}