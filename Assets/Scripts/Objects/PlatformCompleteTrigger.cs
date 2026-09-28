using UnityEngine;

public class PlatformCompleteTrigger : MonoBehaviour
{
    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            BreakablePlatform[] platforms = FindObjectsByType<BreakablePlatform>();
            foreach (BreakablePlatform bp in platforms)
                bp.SetCompleted();
        }
    }
}