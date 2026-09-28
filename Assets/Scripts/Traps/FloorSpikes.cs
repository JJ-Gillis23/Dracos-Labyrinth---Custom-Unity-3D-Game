using UnityEngine;

public class FloorSpikes : MonoBehaviour
{
    public GameObject spikePrefab;
    public int spikeCount = 5;
    public float spacing = 1.5f;
    public float riseHeight = 2f;
    public float riseSpeed = 3f;
    public float stayDuration = 1f;
    public float returnSpeed = 2f;
    public float repeatInterval = 3f;
    public float groundY = 11.5f;
    public int totalCycles = 5;
    public float rowSpacing = 10f;

    private bool isActive = false;
    private int currentCycle = 0;
    private Vector3 capturedSpawnBase;
    private Vector3 capturedPlayerRight;
    private Vector3 capturedForward;

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && !isActive)
        {
            capturedPlayerRight = new Vector3(transform.right.x, 0, transform.right.z).normalized;
            capturedForward = new Vector3(transform.forward.x, 0, transform.forward.z).normalized;
            capturedSpawnBase = transform.position + transform.forward * 20f + transform.right * 5f;

            isActive = true;
            currentCycle = 0;
            InvokeRepeating("StartCycle", 0f, repeatInterval);
        }
    }

    void StartCycle()
    {
        if (currentCycle >= totalCycles)
        {
            CancelInvoke("StartCycle");
            isActive = false;
            return;
        }

        currentCycle++;

        for (int row = 0; row < 2; row++)
        {
            Vector3 rowBase = capturedSpawnBase - capturedForward * rowSpacing * row;

            for (int i = 0; i < spikeCount; i++)
            {
                float offset = (i - spikeCount / 2) * spacing;
                Vector3 spawnPos = new Vector3(
                    rowBase.x + capturedPlayerRight.x * offset,
                    groundY,
                    rowBase.z + capturedPlayerRight.z * offset
                );

                GameObject spike = Instantiate(spikePrefab, spawnPos, Quaternion.identity);
                SpikeAnimator spikeAnim = spike.AddComponent<SpikeAnimator>();
                spikeAnim.Init(riseHeight, riseSpeed, stayDuration, returnSpeed);
            }
        }
    }
}