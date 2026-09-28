using UnityEngine;

public class DebugInit : MonoBehaviour
{
    void Awake()
    {
        if(StatTracker.Instance == null)
            new GameObject("StatTracker").AddComponent<StatTracker>();
        
        if(SaveSystem.Instance == null)
            new GameObject("SaveSystem").AddComponent<SaveSystem>();

        if(Inventory.Instance == null)
            new GameObject("Inventory").AddComponent<Inventory>();
    }
}