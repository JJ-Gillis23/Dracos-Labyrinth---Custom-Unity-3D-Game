using UnityEngine;
using System.Collections;

public class FlyingObstaclesSpawner : MonoBehaviour
{
    public GameObject targetPrefab;
    public Transform[] spawnPoints;

    public float spawnDelay = 0.5f;
    public float cycleDelay = 2f;

    public bool loopForever = true;
    public int totalCycles = 3;

    public float pingPongDistance = 5f;

    private bool hasStarted = false;
    private FlyingObstacle[] activeObstacles;
    private int currentCycle = 0;

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && !hasStarted)
        {
            hasStarted = true;
            activeObstacles = new FlyingObstacle[spawnPoints.Length];
            StartCoroutine(SpawnRoutine());
        }
    }

    IEnumerator SpawnRoutine()
    {
        // Initial spawn - one at each point
        for (int i = 0; i < spawnPoints.Length; i++)
        {
            SpawnObstacle(i);
            yield return new WaitForSeconds(spawnDelay);
        }

        // Keep checking each slot and respawn if destroyed
        while (loopForever || currentCycle < totalCycles)
        {
            for (int i = 0; i < spawnPoints.Length; i++)
            {
                if (activeObstacles[i] == null)
                {
                    yield return new WaitForSeconds(cycleDelay);
                    SpawnObstacle(i);
                }
            }
            yield return null; // wait one frame between checks
        }
    }

    void SpawnObstacle(int index)
    {
        Vector3 spawnPos = spawnPoints[index].position;
        Vector3 targetB = spawnPos + spawnPoints[index].forward * pingPongDistance;

        GameObject obj = Instantiate(targetPrefab, spawnPos, spawnPoints[index].rotation);
        FlyingObstacle obstacle = obj.GetComponent<FlyingObstacle>();

        if (obstacle != null)
        {
            obstacle.Init(spawnPos, targetB); // no spawner reference needed
            activeObstacles[index] = obstacle;
        }
    }
}