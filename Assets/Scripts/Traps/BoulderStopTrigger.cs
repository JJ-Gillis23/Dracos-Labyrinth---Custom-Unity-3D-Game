using UnityEngine;

public class BoulderStopTrigger : MonoBehaviour
{
    public BouncingBoulderSpawner [] spawners;

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
            foreach (BouncingBoulderSpawner s in spawners)
                s.StopSpawning();
    }
}