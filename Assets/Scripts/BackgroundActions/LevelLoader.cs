using UnityEngine;
using System.Collections;

public class LevelLoader : MonoBehaviour
{
    public GameObject gauntletPrefab;

    void Start()
    {
        StartCoroutine(MoveToSpawn());

        PlayerPrefs.DeleteKey("TeleportHintShown");
        PlayerPrefs.DeleteKey("GrappleHintShown");
        PlayerPrefs.DeleteKey("AntManHintShown");
    }

    IEnumerator MoveToSpawn()
    {
        yield return new WaitForEndOfFrame();

        int currentLevel = UnityEngine.SceneManagement.SceneManager.GetActiveScene().buildIndex;

        GameObject spawnPoint = GameObject.Find("SpawnPoint");
        GameObject player = GameObject.FindWithTag("Player");

        if (player == null)
            yield break;

        // Reset player abilities every level
        PlayerController pc = player.GetComponent<PlayerController>();
        if (pc != null)
        {
            if(!SaveSystem.Instance.loadingFromSave)
            {
                pc.jumpForce = 15f;
                pc.sprintSpeed = 15f;
            }
        }
        if(!SaveSystem.Instance.loadingFromSave)
        {
            TeleportTool teleport = player.GetComponent<TeleportTool>();
            if (teleport != null)
                teleport.enabled = false;

            GrapplingHook grapple = player.GetComponent<GrapplingHook>();
            if (grapple != null)
                grapple.enabled = false;

            AntMan antMan = player.GetComponent<AntMan>();
            if (antMan != null)
            {
                antMan.ResetToNormal();
                antMan.enabled = false;
            }
        }

        // Restore gauntlet
        Inventory.Instance.hasGauntlet = true;

        GunController gun = player.GetComponent<GunController>();
        if (gun != null)
            gun.enabled = true;

        Gauntlet existingGauntlet = player.GetComponentInChildren<Gauntlet>(true);

        if (existingGauntlet == null && gauntletPrefab != null)
        {
            Transform cameraTransform = player.GetComponentInChildren<Camera>().transform;

            GameObject spawnedGauntlet = Instantiate(gauntletPrefab, cameraTransform);
            spawnedGauntlet.transform.localPosition = new Vector3(0.3f, -0.3f, 0.7f);
            spawnedGauntlet.transform.localRotation = Quaternion.identity;

            Collider col = spawnedGauntlet.GetComponent<Collider>();
            if (col != null)
                col.enabled = false;

            Rigidbody rb = spawnedGauntlet.GetComponent<Rigidbody>();
            if (rb != null)
                rb.isKinematic = true;
        }

        // CONTINUE GAME
        if (SaveSystem.Instance.loadingFromSave)
        {
            SaveSystem.Instance.Load();
            SaveSystem.Instance.loadingFromSave = false;
        }
        else
        {
            // Spawn at level spawn point
            if (spawnPoint != null)
            {
                player.transform.position = spawnPoint.transform.position;
                player.transform.rotation = spawnPoint.transform.rotation;
            }

            StatTracker.Instance.level = currentLevel;

            if (currentLevel == 1)
                StatTracker.Instance.StartTimer();

            // Save checkpoint for this level
            SaveSystem.Instance.Save();
        }

        if (currentLevel <= 7)
        {
            PlayerUI.Instance.ShowAchievement("Level " + currentLevel);
        }
    }
}