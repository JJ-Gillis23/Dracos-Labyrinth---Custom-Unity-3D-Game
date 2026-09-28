using UnityEngine;

public class PlatformResetter : MonoBehaviour
{
    private GameObject prefab;
    private Vector3 position;
    private Quaternion rotation;
    private float delay;

    public void Init(GameObject prefab, Vector3 position, Quaternion rotation, float delay)
    {
        this.prefab = prefab;
        this.position = position;
        this.rotation = rotation;
        this.delay = delay;
        Invoke("Respawn", delay);
    }

    void Respawn()
    {
        Instantiate(prefab, position, rotation);
        Destroy(gameObject);
    }
}