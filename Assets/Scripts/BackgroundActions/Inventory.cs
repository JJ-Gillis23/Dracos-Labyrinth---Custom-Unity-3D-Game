using UnityEngine;
using System.Collections.Generic;
using System.Linq;

public class Inventory : MonoBehaviour
{
    public static Inventory Instance { get; private set; }
    private Dictionary<string, int> keys = new Dictionary<string, int>();
    private List<string> keyOrder = new List<string>(); // tracks insertion order
    public bool hasGauntlet = false;

    private const int MaxDisplayedKeys = 5;

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

    public void AddKey(string keyID, int uses = 1)
    {
        if (keys.ContainsKey(keyID))
            keys[keyID] += uses;
        else
        {
            keys[keyID] = uses;
            keyOrder.Add(keyID); // track order of insertion
        }

        RefreshDisplay();
    }

    public bool HasKey(string keyID)
    {
        return keys.ContainsKey(keyID) && keys[keyID] > 0;
    }

    public void UseKey(string keyID)
    {
        if (keys.ContainsKey(keyID) && keys[keyID] > 0)
        {
            keys[keyID]--;
            if (keys[keyID] <= 0)
            {
                keys.Remove(keyID);
                keyOrder.Remove(keyID);
            }

            RefreshDisplay();
        }
    }

    public List<string> GetAllKeyIDs()
    {
        return new List<string>(keys.Keys);
    }

    public void ClearKeys()
    {
        keys.Clear();
        keyOrder.Clear();
        if (PlayerUI.Instance != null)
            PlayerUI.Instance.ClearAllKeysUI();
    }

    void RefreshDisplay()
    {
        if (PlayerUI.Instance == null) return;

        PlayerUI.Instance.ClearAllKeysUI();

        // Show only the first MaxDisplayedKeys keys in insertion order
        List<string> toDisplay = keyOrder.Take(MaxDisplayedKeys).ToList();
        foreach (string keyID in toDisplay)
            PlayerUI.Instance.AddKeyUI(keyID, keys[keyID]);

        // If there are more keys than the display limit, show a count of the overflow
        int overflow = keyOrder.Count - MaxDisplayedKeys;
        if (overflow > 0 && PlayerUI.Instance != null)
            PlayerUI.Instance.ShowPersistentText($"+ {overflow} more key(s) in inventory");
    }
}