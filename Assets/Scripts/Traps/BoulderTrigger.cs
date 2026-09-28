using UnityEngine;

public class BoulderTrigger : MonoBehaviour
{
    private BouncingBoulderSpawner[] spawners;

    void Start()
    {
        spawners = GetComponentsInChildren<BouncingBoulderSpawner>();
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            foreach (BouncingBoulderSpawner spawner in spawners)
                spawner.StartSpawning();
        }
    }
}