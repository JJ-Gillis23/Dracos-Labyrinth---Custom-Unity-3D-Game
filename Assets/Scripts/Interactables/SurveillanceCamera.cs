using UnityEngine;
using System.Collections.Generic;

public class SurveillanceCamera : Interactables
{
    public float rotationSpeed = 30f;
    public float maxAngle = 45f;
    private float currentAngle = 0f;
    private int direction = 1;
    public GameObject laserPrefab;
    public int laserCount = 5;
    public float spacing = 1.5f;
    public float repeatInterval = 3f;
    public float groundY = 11.5f;
    public int totalCycles = 5;
    public float spawnInterval = 3f;
    public float forwardOffset = 20f;
    public float rightOffset = 5f;

    private bool playerInRange = false;
    private float spawnCooldown = 0f;
    private bool isActive = false;
    private int currentCycle = 0;
    private Vector3 capturedSpawnBase;
    private Vector3 capturedPlayerRight;
    private Vector3 capturedForward;
    private List<GameObject> activeLasers = new List<GameObject>();

    void Update()
    {
        currentAngle += rotationSpeed * direction * Time.deltaTime;

        if (currentAngle >= maxAngle)
        {
            currentAngle = maxAngle;
            direction = -1;
        }
        else if (currentAngle <= -maxAngle)
        {
            currentAngle = -maxAngle;
            direction = 1;
        }

        transform.localRotation = Quaternion.Euler(0, currentAngle, 0);

        if (playerInRange)
        {
            spawnCooldown -= Time.deltaTime;
            if (spawnCooldown <= 0f)
            {
                if (!isActive)
                {
                    capturedPlayerRight = new Vector3(transform.parent.parent.right.x, 0, transform.parent.parent.right.z).normalized;
                    capturedForward = new Vector3(transform.parent.parent.forward.x, 0, transform.parent.parent.forward.z).normalized;
                    capturedSpawnBase = transform.parent.parent.position + transform.parent.parent.forward * forwardOffset + transform.parent.parent.right * rightOffset;

                    isActive = true;
                    currentCycle = 0;
                    InvokeRepeating("StartCycle", 0f, repeatInterval);
                }
                spawnCooldown = spawnInterval;
            }
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            playerInRange = true;
            spawnCooldown = 0f;
            PlayerUI.Instance.UpdatePersistentText(string.Empty); // clear any existing text
            PlayerUI.Instance.UpdatePromptText(string.Empty); // clear prompt text if it was showing
            PlayerUI.Instance.ShowPersistentText("Camera detected you! Find a way to disable it!");
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            playerInRange = false;
            spawnCooldown = 0f;
        }
    }

    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Projectile"))
        {
            foreach (GameObject laser in activeLasers)
            {
                if (laser != null)
                    Destroy(laser);
            }
            activeLasers.Clear();

            StatTracker.Instance.CameraDestroyed();
            PlayerUI.Instance.ShowAchievement("*Camera Destroyed*");
            CancelInvoke("StartCycle");
            isActive = false;
            Destroy(transform.parent.parent.gameObject);
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

        Vector3 spawnPos = new Vector3(
            capturedSpawnBase.x,
            groundY + 10f,
            capturedSpawnBase.z
        );

        Quaternion laserRotation = Quaternion.Euler(90f, 90f, 0f);
        GameObject laser = Instantiate(laserPrefab, spawnPos, laserRotation);
        activeLasers.Add(laser);
    }
}