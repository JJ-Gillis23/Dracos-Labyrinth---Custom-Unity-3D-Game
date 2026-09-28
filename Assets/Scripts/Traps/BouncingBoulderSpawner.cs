using UnityEngine;
using System.Collections;

public class BouncingBoulderSpawner : MonoBehaviour
{
    public GameObject boulderPrefab;
    public Vector3 respawnPoint;
    public float spawnDelay = 1.5f;
    public int bouldersPerCycle = 1;
    public float cycleDelay = 5f;
    public bool loopForever = true;
    public int totalCycles = 3;
    private bool hasStarted = false;


    public void StartSpawning()
    {
        if (!hasStarted)
        {
            hasStarted = true;
            StartCoroutine(SpawnLoop());
        }
    }

    IEnumerator SpawnLoop()
    {
        int currentCycle = 0;

        while (loopForever || currentCycle < totalCycles)
        {
            for (int i = 0; i < bouldersPerCycle; i++)
            {
                GameObject boulder = Instantiate(boulderPrefab, transform.position, transform.rotation);
                BouncingBoulder bb = boulder.GetComponent<BouncingBoulder>();
                if (bb != null)
                    bb.Init(respawnPoint);
                 yield return new WaitForSeconds(spawnDelay);
            }

            currentCycle++;
            yield return new WaitForSeconds(cycleDelay);
        }
    }
    public void StopSpawning()
    {
        StopAllCoroutines();
        hasStarted = false;
    }
}