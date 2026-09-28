using UnityEngine;
using System.Collections.Generic;

[System.Serializable]
public class SaveData
{
    public int level;
    public int camerasDestroyed;
    public int itemsCollected;
    public List<string> inventory = new List<string>();

    public float playerX;
    public float playerY;
    public float playerZ;

    // NEW - Player Rotation
    public float playerRotX;
    public float playerRotY;
    public float playerRotZ;
    public float playerRotW;

    public bool hasGauntlet;
    public float elapsedTime;
    public bool hasTeleport;
    public bool hasGrapple;
    public bool hasAntMan;
    public float jumpForce;
    public float sprintSpeed;
}

public class SaveSystem : MonoBehaviour
{
    public static SaveSystem Instance { get; private set; }

    // Tells the LevelLoader if we're continuing from the main menu.
    public bool loadingFromSave = false;

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void Save()
    {
        SaveData data = new SaveData();

        GameObject player = GameObject.FindGameObjectWithTag("Player");
        PlayerController pc = player.GetComponent<PlayerController>();


        if (player != null)
        {
            TeleportTool teleport = player.GetComponent<TeleportTool>();
            GrapplingHook grapple = player.GetComponent<GrapplingHook>();
            AntMan antMan = player.GetComponent<AntMan>();
            
            data.hasTeleport = teleport != null && teleport.enabled;
            data.hasGrapple = grapple != null && grapple.enabled;
            data.hasAntMan = antMan != null && antMan.enabled;
            data.jumpForce = pc.jumpForce;
            data.sprintSpeed = pc.sprintSpeed;
            data.playerX = player.transform.position.x;
            data.playerY = player.transform.position.y;
            data.playerZ = player.transform.position.z;

            data.playerRotX = player.transform.rotation.x;
            data.playerRotY = player.transform.rotation.y;
            data.playerRotZ = player.transform.rotation.z;
            data.playerRotW = player.transform.rotation.w;

            data.hasGauntlet = Inventory.Instance.hasGauntlet;
        }

        data.level = StatTracker.Instance.level;
        data.camerasDestroyed = StatTracker.Instance.camerasDestroyed;
        data.itemsCollected = StatTracker.Instance.itemsCollected;
        data.inventory = Inventory.Instance.GetAllKeyIDs();
        data.elapsedTime = StatTracker.Instance.elapsedTime;

        string json = JsonUtility.ToJson(data);

        PlayerPrefs.SetString("SaveData", json);
        PlayerPrefs.Save();

    }

    public bool HasSave()
    {
        return PlayerPrefs.HasKey("SaveData");
    }

    public void Load()
    {
        if (!HasSave())
            return;

        string json = PlayerPrefs.GetString("SaveData");
        SaveData data = JsonUtility.FromJson<SaveData>(json);

        StatTracker.Instance.level = data.level;
        StatTracker.Instance.camerasDestroyed = data.camerasDestroyed;
        StatTracker.Instance.itemsCollected = data.itemsCollected;
        StatTracker.Instance.elapsedTime = data.elapsedTime;

        Inventory.Instance.hasGauntlet = data.hasGauntlet;

        Inventory.Instance.ClearKeys();

        foreach (string keyID in data.inventory)
        {
            Inventory.Instance.AddKey(keyID);
        }

        GameObject player = GameObject.FindGameObjectWithTag("Player");

        if (player != null)
        {
            player.transform.position = new Vector3(
                data.playerX,
                data.playerY,
                data.playerZ);

            player.transform.rotation = new Quaternion(
                data.playerRotX,
                data.playerRotY,
                data.playerRotZ,
                data.playerRotW);
            TeleportTool teleport = player.GetComponent<TeleportTool>();
            if (teleport != null)
                teleport.enabled = data.hasTeleport;

            GrapplingHook grapple = player.GetComponent<GrapplingHook>();
            if (grapple != null)
                grapple.enabled = data.hasGrapple;

            AntMan antMan = player.GetComponent<AntMan>();
            if (antMan != null)
            {
                antMan.ResetToNormal();      // Make sure player isn't left shrunk
                antMan.enabled = data.hasAntMan;
            }
            PlayerController pc = player.GetComponent<PlayerController>();

            if (pc != null)
            {
                pc.jumpForce = data.jumpForce;
                pc.sprintSpeed = data.sprintSpeed;
            }
        }

    }

    public void DeleteSave()
    {
        PlayerPrefs.DeleteKey("SaveData");
    }

    public int GetSavedLevel()
    {
        if (!HasSave())
            return 1;

        string json = PlayerPrefs.GetString("SaveData");
        SaveData data = JsonUtility.FromJson<SaveData>(json);

        return data.level;
    }
}