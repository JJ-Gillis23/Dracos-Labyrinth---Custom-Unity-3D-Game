using UnityEngine;
using System.Collections;

public class MovingObstacleSpawner : MonoBehaviour
{
    public GameObject obstaclePrefab;

    void Start()
    {
        StartCoroutine(SpawnLoop());
    }

    IEnumerator SpawnLoop()
    {
        while (true)
        {
            GameObject obstacle = Instantiate(obstaclePrefab, transform.position, transform.rotation);
            yield return new WaitUntil(() => obstacle == null);
        }
    }
}